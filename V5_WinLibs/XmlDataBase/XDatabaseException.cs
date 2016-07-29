using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;

namespace XmlDatabase
{
    /// <summary>
    /// 这是一个数据库异常。它将包含一个异常消息属性。
    /// </summary>
    public class XDatabaseException:Exception
    {
        private XDatabaseException() { }
        
        /// <summary>
        /// 数据库异常的构造函数
        /// </summary>
        /// <param name="key">错误消息的资源键</param>
        /// <param name="place">占位符文本。用来替换掉错误消息中的一些占位符，占位符的格式为{0}，以此类推</param>
        public XDatabaseException(string key,params string[] place) {
            StringBuilder sb = new StringBuilder(XmlDataBase.Properties.Resource1.ResourceManager.GetString(key));
            if (place.Length > 0) {
                int i=0;
                foreach (var item in place)
                {
                    sb.Replace("{" + (i++) + "}", item);
                }
            }
            message = sb.ToString();
        }

        private string message;
        /// <summary>
        /// 该异常的错误消息
        /// </summary>
        public override string Message
        {
            get
            {
                return message;
            }
        }
    }
}
