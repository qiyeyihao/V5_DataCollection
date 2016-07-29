using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace XmlDatabase
{
    //备注，目前这个类还没有用到。


    /// <summary>
    /// 这是缓存管理器，即便一个程序内部会创建多个XDatabase的实例，但它们仍然是共享一个缓存管理器。
    /// 这是在进程内共享的
    /// </summary>
    class XBufferManager
    {
        private XBufferManager() { }

        private static Dictionary<string, XBufferManager> instances = new 
            Dictionary<string, XBufferManager>();

        

        /// <summary>
        /// 这是Singleton模式，不管有多少个XDatabase，但只要是数据库名称是一样的，就只有一个缓存管理器实例
        /// </summary>
        /// <returns></returns>
        public static XBufferManager GetInstance(string db) {
            if (!instances.Keys.Contains(db))
                instances.Add(db, new XBufferManager());

            return instances[db];
        }
    }
}
