using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDatabase
{

    //备注：目前该类还没用过

    /// <summary>
    /// 数据库事务
    /// </summary>
    public class XTransaction
    {
        private XTransaction() { }
        private XDatabase database = null;
        internal XTransaction(XDatabase db) {
            database = db;
        }
    }
}
