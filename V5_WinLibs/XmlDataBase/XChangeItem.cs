using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDatabase
{
    class XChangeItem
    {
        public XChangeAction Action { get; set; }
        public object UserData { get; set; }
    }

    enum XChangeAction
    {
        AddOrUpdate,
        Delete,
        Clear
    }
}
