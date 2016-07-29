using System;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace XmlDatabase
{
    class XLogWriter:TextWriter
    {
        private string logFile = default(string);
        private XDocument doc = null;
        private XDatabase db = null;

        internal XLogWriter(XDatabase database) {
            
            db = database;

            string dir = Path.Combine(db.FullName, "SystemLogs");
            string date=DateTime.Now.ToString("yyyy-MM-dd");
            logFile = Path.Combine(dir, string.Format("{0}.xml", date ));
            //TODO:日志文件可以绑定一个特定的XSLT文件,可以直接查看
            Directory.CreateDirectory(dir);

            if (!File.Exists(logFile))
            {
                doc = new XDocument(
                    new XElement("XMLDatabase-SystemLogs",
                        new XAttribute("Date", date),
                        new XAttribute("EngineVersion", db.Version)
                        ));
                doc.Save(logFile);
            }
            else
            {
                doc = XDocument.Load(logFile);
            }
        }

        public override void Write(string value)
        {
            WriteLine(value);
        }

        public override void WriteLine(string value)
        {
            doc.Root.Add(
                new XElement("LogEntry",
                    new XAttribute("Time", DateTime.Now),
                    new XAttribute("Message", value),
                    new XAttribute("Application", db.ApplicationName),
                    new XAttribute("UserName", db.UserName)));
        }

        public override void Flush()
        {
            //将内容一次性写入到文件中去
            doc.Save(logFile);
            //TODO:这样的做法是将日志内容一直放在内存中，较之每次都打开文件，然后写，然后关闭的方式，更有效率。但如果内容太多的话，是否有问题呢？

        }

        public override void Close()
        {
            doc.Save(logFile);
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

    }
}
