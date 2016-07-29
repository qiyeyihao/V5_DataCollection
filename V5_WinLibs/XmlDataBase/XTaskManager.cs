using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;


namespace XmlDatabase
{
    class XTaskManager
    {
        private XTaskManager() { }
        private XDatabase internaldb = default(XDatabase);
        private List<XChangeItem> tasks = new List<XChangeItem>();


        public XTaskManager(XDatabase db) {
            internaldb = db;
        }
        public void AddTask(XChangeItem task) {
            tasks.Add(task);
        }
        private Dictionary<string, FileStream> fs;
        private Dictionary<string, XDocument> docs;

        public XSubmitStatus Execute(bool continueOnError,bool hasTransaction) {
            //不用事务的方式进行处理任务
            StringBuilder sb = new StringBuilder();//用来保存错误消息
            bool hasError = false;

            //批量更新的时候，为了避免并发问题，采用悲观并发处理，直接将文件锁住.此时，其他的操作只能对文件进行那个读取
            //在目前的设计中，并发这样简单地处理一下。以目前的考虑，该数据库主要用于客户端数据缓存和单用户的嵌入式系统中，所以存在用户并发的情况很少。

            fs = new Dictionary<string, FileStream>();
            docs = new Dictionary<string, XDocument>();

            foreach (var item in tasks)
            {
                try
                {
                    switch (item.Action)
                    {
                        case XChangeAction.AddOrUpdate:
                            AddOrUpdateObject(item.UserData);
                            break;
                        case XChangeAction.Delete:
                            DeleteObject(item.UserData);
                            break;
                        case XChangeAction.Clear:
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    hasError = true;
                    sb.AppendFormat(XmlDataBase.Properties.Resource1.ChangeActionError,
                        item.UserData.ToString(),
                        item.Action,
                        ex.Message);
                    sb.AppendLine();

                    if (continueOnError)
                        continue;
                    else
                        break;
                }
            }

            foreach (var item in fs)
            {
                item.Value.Close();//确保关闭这些文件，释放锁
            }

            if (hasError && hasTransaction)
            {
                //回滚，所以不写入基础文件，就是说所有的操作其实都撤销了
            }
            else
            {
                foreach (var item in docs)
                {
                    item.Value.Save(item.Key);//并将所有的更新写入到基础文件中去
                }
            }

            fs.Clear();
            docs.Clear();
            tasks.Clear();

            internaldb.Log.Flush();

            return new XSubmitStatus(hasError, sb.ToString());
        }
        private void AddOrUpdateObject(object o) {
            string typeFullName = o.GetType().FullName;
            string typeName = o.GetType().Name;

            string dataFile =
                string.Format("{4}\\{0}\\{1}\\{2}\\{3}.xml",
                    "Entities",
                    typeFullName,
                    "Data",
                    typeFullName,
                    internaldb.FullName);

            if (File.Exists(dataFile))
            {
                if (!fs.Keys.Contains(dataFile))
                {
                    FileStream temp = new FileStream(dataFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    fs.Add(dataFile, temp);
                    docs.Add(dataFile, XDocument.Load(new StreamReader(temp)));
                }
            }
            else
            {
                XDocument temp = new XDocument(
                    new XElement("XMLDatabase-Entities"));
                temp.Save(dataFile);
                docs.Add(dataFile, temp);
                fs.Add(dataFile, new FileStream(dataFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None));
            }


            XDocument data = docs[dataFile];

            Guid id = internaldb.ids.GetObjectId(o);
            if (id != Guid.Empty)
            {
                //更新

                //查找到在文件中的那个元素
                XElement element = data.Root.Elements(typeName).
                    Where(e => e.Attribute("_uuid").Value == id.ToString()).
                    FirstOrDefault();
                //产生一个新的元素
                XElement replace = o.ConvertToXml(id);
                //进行替换
                element.ReplaceWith(replace);

                //编写日志
                internaldb.Log.WriteEx(string.Format(XmlDataBase.Properties.Resource1.UpdateObject, id, o.ToString()));
            }
            else
            { 
                //新增

                id = Guid.NewGuid();
                XElement newInstance = o.ConvertToXml(id);
                data.Root.Add(newInstance);

                //编写日志
                internaldb.Log.WriteEx(string.Format(XmlDataBase.Properties.Resource1.InsertObject, id, o.ToString()));
            }


        }
        private void DeleteObject(object o) {
            Guid id = internaldb.ids.GetObjectId(o);

            string typeFullName = o.GetType().FullName;
            string typeName = o.GetType().Name;

            string dataFile =
                string.Format("{4}\\{0}\\{1}\\{2}\\{3}.xml",
                    "Entities",
                    typeFullName,
                    "Data",
                    typeFullName,
                    internaldb.FullName);

            if (id == Guid.Empty || !File.Exists(dataFile))
            {
                //return;//如果一个对象没有id，或者文件根本不存在，直接忽略
                throw new XDatabaseException("DeleteObjectError", o.ToString());
            }

            if (!fs.Keys.Contains(dataFile))
            {
                FileStream temp = new FileStream(dataFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                fs.Add(dataFile, temp);
                docs.Add(dataFile, XDocument.Load(new StreamReader(temp)));
            }

            XDocument data = docs[dataFile];
            XElement removeObject = data.Root.Elements().Where(e => e.Attribute("_uuid").Value == id.ToString()).FirstOrDefault();

            if (removeObject != null)
                removeObject.Remove();

            internaldb.ids.Remove(o);

            internaldb.Log.WriteEx(string.Format(XmlDataBase.Properties.Resource1.DeleteObject, id, o));

        }
    }
}
