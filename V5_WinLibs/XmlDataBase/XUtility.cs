using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDatabase
{
    class XUtility
    {
        private XUtility() { }
        public static string EngineVersion {
            get {
                return typeof(XUtility).Assembly.GetName().Version.ToString();
            }
        }
    }
}
