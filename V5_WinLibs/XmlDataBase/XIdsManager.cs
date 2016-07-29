using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDatabase
{
    /// <summary>
    /// 这个类负责管理所有现在在内存中的对象Id
    /// </summary>
    class XIdsManager
    {
        private XIdsManager() { }

        private static Dictionary<string, XIdsManager> instances = new
            Dictionary<string, XIdsManager>();

        /// <summary>
        /// 这是Singleton模式，不管有多少个XDatabase，但只要是数据库名称是一样的，就只有一个编号管理器
        /// </summary>
        /// <returns></returns>
        public static XIdsManager GetInstance(string db)
        {
            if (!instances.Keys.Contains(db))
                instances.Add(db, new XIdsManager());

            return instances[db];
        }


        private Dictionary<object,Guid> ids = new Dictionary<object,Guid>();

        public Guid GetObjectId(object o)
        {
            if (ids.Keys.Contains(o))
                return ids[o];

            return Guid.Empty;
        }


        public void Set(object o, Guid id) {
            if (ids.Values.Contains(id)) {
                ids.Remove(ids.FirstOrDefault(k => k.Value == id).Key);
                Set(o, id);
                return;
            }
            if (!ids.Keys.Contains(o))
            {
                ids.Add(o, id);
            }
        }

        public void Remove(object o)
        {
            ids.Remove(o);
        }

        public void Clear() {
            ids.Clear();
        }
    }
}
