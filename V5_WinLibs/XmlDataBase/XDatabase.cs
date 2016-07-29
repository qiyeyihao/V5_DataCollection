using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Xml;
using System.Reflection;
using System.Xml.Linq;
using System.Threading;


namespace XmlDatabase
{
    /// <summary>
    /// 这是数据库对象，是XML Database数据库引擎中其他任何操作的起点。
    /// </summary>
    public sealed class XDatabase:IDisposable
    {
        #region 常量定义
        /// <summary>
        /// 数据库配置文件名称
        /// </summary>
        private static string DATABASECONFIGFILE = "CORE.XML";
        #endregion

        #region 内部变量
        //internal XTransactionManager TransactionManager = null;
        #endregion

        #region 公有属性
        /// <summary>
        /// 数据库引擎的版本号
        /// </summary>
        public string Version
        {
            get
            {
                return XUtility.EngineVersion;
            }
        }

        private string fullName;
        /// <summary>
        /// 数据库的完整路径，包含了文件夹路径。
        /// </summary>
        public string FullName
        {
            get { return fullName; }
        }


        private bool autoSubmit = true;
        /// <summary>
        /// 是否立即提交，也就是一个操作是马上就生效还是要批处理，默认为true。如果从语法简洁来说，设置为true的话很自然。但如果说要进行一次批量的操作，例如要循环插入1000个对象，那么这种时候就不应该是立即提交，而是应该设置为false，实施批量提交。这样的性能是有很大的提高的
        /// </summary>
        public bool AutoSubmitMode
        {
            get
            { return autoSubmit; 
            }
            set {
                autoSubmit = value;
            }
        }

        /// <summary>
        /// 这是一个日志编写器，主要是为了调试方便。例如在控制台程序中可以用将Log=Console.Out
        /// 如果该属性指定的，就重定向到对应的TextWriter中去，否则，应该自动地设置一个日志文本文件，在数据库的日志目录中
        /// 每天一个日志文件，也是XML的文件（既然是XML数据库，任何内容都是XML的，呵呵）
        /// </summary>
        public TextWriter Log { get; set; }

        /// <summary>
        /// 这是应用程序名称，记录这个信息是有助于在日志中标识，默认会读取到目前调用数据库的程序名称，用户也可以设置
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 这是用户名，默认会读取到当前主体身份。可以进行设置。
        /// </summary>
        public string UserName { get; set; }
        #endregion

        #region 缓存管理
        //虽然可以在一个应用程序中创建多个XDatabase的实例，但是它们仍然是共享一个缓存管理器。就是说不会重复读数据
        //private XBufferManager Cache;
        #endregion

        #region 任务管理
        private XTaskManager taskManager;
        #endregion

        #region 编号管理
        internal XIdsManager ids;
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数设置为private，目的是为了避免用户通过new关键字对其进行实例化
        /// </summary>
        private XDatabase() {}
        /// <summary>
        /// 根据一个数据库目录名称进行构造
        /// </summary>
        /// <param name="_fullName">数据库完整名称，其实是一个路径名称</param>
        private XDatabase(string _fullName):this() {
            fullName = _fullName;
            //TODO:这里进行必要的校验

            //给几个属性初始化
            ApplicationName = AppDomain.CurrentDomain.FriendlyName;
            UserName = Thread.CurrentPrincipal.Identity.Name;
            Log = new XLogWriter(this);
            
            //TransactionManager = new XTransactionManager(this);
            //Cache = XBufferManager.GetInstance(this.FullName);
            ids = XIdsManager.GetInstance(this.FullName);
            taskManager = new XTaskManager(this);

            //编写日志
            Log.WriteEx(XmlDataBase.Properties.Resource1.DatabaseOpened,true);
        }

        #endregion

        #region 管理维护

        /// <summary>
        /// 创建或者打开数据库，如果数据库存在即打开，如果不存在就创建
        /// </summary>
        /// <param name="database">这个名字不区分相对路径和绝对路径，由客户程序自己决定。因为考虑到每种客户程序对于路径的处理不尽相同</param>
        /// <returns>数据库对象</returns>
        /// <exception cref="System.IO.FileNotFoundException">如果文件夹里面不存在core.xml，则引发一个数据库异常</exception>
        /// <exception cref="XDatabaseException">其他未捕获异常会引发一个数据库异常</exception>
        public static XDatabase Open(string database) {
            DATABASECONFIGFILE = Path.Combine(database, "core.xml");
            
            try
            {
                if (Directory.Exists(database))
                {
                    //如果该目录存在，则检查下面是否有一个数据库配置文件，如果没有则出错
                    //TODO:应该对该文件进行一定的校验
                    //该配置文件名规定为_core.xml
                    if (File.Exists(DATABASECONFIGFILE))
                    {
                        return new XDatabase(database);
                    }
                    else
                    {
                        throw new FileNotFoundException();
                    }
                }
                else
                { 
                    //创建该目录，并创建一个core.xml文件
                    Directory.CreateDirectory(database);//创建该文件夹
                    XDocument x = new XDocument(
                        new XElement("Database",
                            new XAttribute("CreateTime", DateTime.Now),
                            new XAttribute("Version",XUtility.EngineVersion),
                            new XElement("Types")
                            ));
                    x.Save(DATABASECONFIGFILE);
                    return new XDatabase(database);
                }
            }
            catch(Exception ex) {
                throw new XDatabaseException("GeneralException", ex.Message);
            }
        }

        /// <summary>
        /// 关闭数据库。数据库关闭之前要进行必要的资源清理工作，或者关闭一些打开的对象
        /// </summary>
        public void Close() { 
            //TODO:数据库关闭之前要进行必要的资源清理工作，或者关闭一些打开的对象
            Log.WriteLine(XmlDataBase.Properties.Resource1.DatabaseClosed);
            Log.Close();
        }

        #endregion

        #region 配置操作
        /// <summary>
        /// 配置某种对象在单文件中存放多少个，这样做的目的是为了分开存放，以提高效率
        /// </summary>
        /// <param name="items">要进行配置的类型定义集合。</param>
        public void Configure(XTypeRegistration[] items) {

            XDocument x = XDocument.Load(DATABASECONFIGFILE);
            XElement root = x.Root.Element("Types");
            foreach (var item in items)
            {
                string typeFolder = Path.Combine(this.FullName, "Entities\\"+item.FullName);
                Directory.CreateDirectory(typeFolder);
                Directory.CreateDirectory(typeFolder + "\\Data");
                Directory.CreateDirectory(typeFolder + "\\Index");

                //如果存在，即更新，否则新增一条类型注册
                XElement found = root.Elements("Type").FirstOrDefault(
                        f => f.Attribute("FullName").Value == item.FullName
                    );
                if (found != null)
                {
                    found = item.ConvertToXml("Type",Guid.Empty);
                }
                else
                {
                    root.Add(item.ConvertToXml("Type",Guid.Empty));
                }

                Log.WriteEx(string.Format(XmlDataBase.Properties.Resource1.TypeConfiguration, item.FullName));
            }

            Log.Flush();

            x.Save(DATABASECONFIGFILE);
        }

        /// <summary>
        /// 取得所有已经注册的类型信息
        /// </summary>
        public List<XTypeRegistration> Types
        {
            get
            {
                XDocument x = XDocument.Load(DATABASECONFIGFILE);
                XElement root = x.Root.Element("Types");
                var query = from t in root.Elements("Type")
                            select (XTypeRegistration)t.ConvertToObject<XTypeRegistration>().Instance;

                return query.ToList();            
            }
        }

        #endregion

        #region 对象操作
        /// <summary>
        /// 插入或者更新一个对象
        /// 如果对象具有一个uuid，则更新，否则插入
        /// </summary>
        /// <param name="instance">对象实例，用object类型是为了尽可能地简化操作</param>
        public void Store(object instance) {
            XDocument x = XDocument.Load(DATABASECONFIGFILE);
            XElement typeRegistraion = x.Root.
                Element("Types").Elements("Type").
                Where(e => e.Attribute("FullName").Value == instance.GetType().FullName).FirstOrDefault();

            if (typeRegistraion == null)
            {
                Configure(new[] { new XTypeRegistration() { FullName = instance.GetType().FullName } });
                Store(instance, false);
            }
            else
                Store(instance, bool.Parse(typeRegistraion.Attribute("SingleRowPerFile").Value));

        }

        /// <summary>
        /// 插入或者更新一个对象，指示是否要单独存放一个文件，这个比较适合于一些特别大的对象
        /// </summary>
        /// <param name="instance">对象实例，用object类型是为了尽可能地简化操作</param>
        /// <param name="singleFile">是否要用单独的一个文件存储一个对象</param>
        public void Store(object instance, bool singleFile) {

            string typeFullName=instance.GetType().FullName;
            string typeName = instance.GetType().Name;

            XDocument x = XDocument.Load(DATABASECONFIGFILE);
            XElement typeRegistraion = x.Root.
                Element("Types").Elements("Type").
                Where(e => e.Attribute("FullName").Value == typeFullName).FirstOrDefault();
            if (typeRegistraion == null)
                Configure(new[] { new XTypeRegistration() { FullName = typeFullName } });

            //TODO:目前并不拆成多个文件存放，下一阶段要考虑对此进行设计
            string dataFile = 
                string.Format("{4}\\{0}\\{1}\\{2}\\{3}.xml", 
                    "Entities", 
                    typeFullName, 
                    "Data", 
                    typeFullName,
                    fullName);

            if (autoSubmit)
            {
                //如果当前数据库设置为立即提交
                XDocument data = File.Exists(dataFile) ? XDocument.Load(dataFile) :
                    new XDocument(
                        new XElement("XMLDatabase-Entities"));

                Guid id = ids.GetObjectId(instance);

                if (id != Guid.Empty)
                {
                    //更新
                    //查找到在文件中的那个元素
                    XElement element = data.Root.Elements(typeName).
                        Where(e => e.Attribute("_uuid").Value == id.ToString()).
                        FirstOrDefault();
                    //产生一个新的元素
                    XElement replace = instance.ConvertToXml(id);
                    //进行替换
                    element.ReplaceWith(replace);

                    //编写日志
                    Log.WriteEx(string.Format(XmlDataBase.Properties.Resource1.UpdateObject, id, instance.ToString()), true);

                }
                else
                { 
                    //新增

                    id=Guid.NewGuid();
                    XElement newInstance = instance.ConvertToXml(id);
                    data.Root.Add(newInstance);

                    //编写日志
                    Log.WriteEx(string.Format(XmlDataBase.Properties.Resource1.InsertObject, id, instance.ToString()), true);
                }
                data.Save(dataFile);
            }
            else
            { 
                //如果为延迟提交（或者我们称为批量提交）
                taskManager.AddTask(new XChangeItem() { Action = XChangeAction.AddOrUpdate, UserData = instance });
                
            }
        }


        /// <summary>
        /// 查询对象，这是返回指定类型的所有对象实例
        /// </summary>
        /// <typeparam name="T">要查询的类型</typeparam>
        /// <returns>对象实例的集合</returns>
        public T[] Query<T>() {
            
            //读取的时候，应该将对象的引用放在内存中
            //TODO:目前所有对象都放在一个文件里面，以后要考虑分开存放

            string typeFullName =typeof(T).FullName;
            //TODO:目前并不拆成多个文件存放，下一阶段要考虑对此进行设计
            //TODO:这里的查询还要改进，得可以先过滤一部分，不要全部输出过来。当然，如果一组对象是按一个文件存放的话，那么全部读过来反而是最好的。因为一份XML文档本来就必须全部加载，不可能加载一部分

            string dataFile =
                string.Format("{4}\\{0}\\{1}\\{2}\\{3}.xml",
                    "Entities",
                    typeFullName,
                    "Data",
                    typeFullName,
                    fullName);

            if (!File.Exists(dataFile))
                return null;

            XDocument x = XDocument.Load(dataFile);
            var query = from e in x.Root.Elements()
                        select e.ConvertToObject<T>();

            List<T> results = new List<T>();
            foreach (var item in query)
            {
                ids.Set(item.Instance, item.Id);
                results.Add((T)item.Instance);
            }

            Log.WriteEx(string.Format(XmlDataBase.Properties.Resource1.QueryObject, results.Count()), true);
            return results.ToArray();
        }

        /// <summary>
        /// 删除某个对象，如果对象没有uuid，则表示该对象根本就没有存在于数据库中，不进行任何操作
        /// </summary>
        /// <param name="instance">要删除的对象实例</param>
        public void Delete(object instance) {
            if (autoSubmit)
            {
                Guid id = ids.GetObjectId(instance);
                if (id == Guid.Empty) return;

                ids.Remove(instance);
                string typeFullName = instance.GetType().FullName;
                string dataFile =
                    string.Format("{4}\\{0}\\{1}\\{2}\\{3}.xml",
                        "Entities",
                        typeFullName,
                        "Data",
                        typeFullName,
                        fullName);

                if (!File.Exists(dataFile))
                    return;

                XDocument x = XDocument.Load(dataFile);
                x.Root.Elements().FirstOrDefault(e => e.Attribute("_uuid").Value == id.ToString()).Remove();
                x.Save(dataFile);

                Log.WriteEx(string.Format(XmlDataBase.Properties.Resource1.DeleteObject, id, instance), true);
            }
            else
                taskManager.AddTask(new XChangeItem() { Action = XChangeAction.Delete, UserData = instance });
        }

        /// <summary>
        /// 删除一批对象，同样的逻辑，如果对象没有uuid，则忽略
        /// 循环调用了Delete方法。如果数据库是自动提交的话，则直接就删除了。否则就进入任务队列
        /// </summary>
        /// <param name="objects">要删除的对象集合</param>
        public void Delete(object[] objects) {
            foreach (var item in objects)
            {
                Delete(item);
            }
        }

        /// <summary>
        /// 一次性提交多个更改
        /// </summary>
        /// <param name="continueOnError">失败时是否继续。因为是批量操作，那么可能其中某些操作会失败，该参数指示如果失败，则是否应该继续执行后面的操作</param>
        /// <remarks></remarks>
        public XSubmitStatus SubmitChanges(bool continueOnError) {
            return taskManager.Execute(continueOnError,false);
        }

        /// <summary>
        /// 一次性提交多个更改，失败时继续执行。但会把失败的消息返回
        /// </summary>
        /// <returns></returns>
        public XSubmitStatus SubmitChanges() {
            return SubmitChanges(true);
        }

        /// <summary>
        /// 用事务的方式一次性提交多个更改。这种情况下，只要有任何一项操作不成功，整个所有的操作都将撤销。
        /// 这个方法有点类似于SubmitChanges(false)，也就是说失败时不继续执行后面的操作。但不同的是，如果带有事务性的话，则不光是不执行后面的操作，而且要撤销前面的操作。
        /// </summary>
        /// <param name="tran"></param>
        /// <remarks></remarks>
        public XSubmitStatus SubmitChangesWithTransaction() {
            return taskManager.Execute(false,true);
        }
        #endregion

        #region 事务管理

        //TODO:这个功能已经简化了，暂时用不着。但先留着看看以后要不要修改

        /// <summary>
        /// 开始一个新的事务。在一个事务里面的所有操作，都不会立即提交，而是等到事务提交的时候依次执行，并且它们要么一起成功，要么一起失败。
        /// </summary>
        /// <returns>事务对象，一旦事务开启之后，要加入该事务的操作中就要添加对该事务的引用</returns>
        public XTransaction BeginTransaction() {
            return new XTransaction(this);//显式打开一个事务，所有的操作都必须指定该对象，并只有在commit的时候才真正写入到数据库
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 数据库对象的销毁
        /// </summary>
        public void Dispose()
        {
            
        }

        #endregion
    }
}
