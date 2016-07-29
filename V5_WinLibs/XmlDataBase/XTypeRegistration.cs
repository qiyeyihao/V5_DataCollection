using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDatabase
{
    /// <summary>
    /// 数据实体注册信息
    /// </summary>
    public class XTypeRegistration
    {

        #region 私有成员
        private int rowCount = 1000;
        private bool singleRow = false;

        #endregion

        /// <summary>
        /// 这个目标的数据实体类型
        /// </summary>
        public string FullName { get; set; }


        /// <summary>
        /// 指定在一个文件中保存几个对象。默认为1000
        /// </summary>
        public int RowCountPerFile { get { return rowCount; } set { rowCount = value; } }

        /// <summary>
        /// 指定是否要用一个文件保存单一的对象。这种情况虽然少一些，但确实有可能。典型的场景就是一个完整的表单对象，例如采购订单信息，我们可以用一个对象表示，然后每个订单保存一个文件。默认为false
        /// </summary>
        public bool SingleRowPerFile { get { return singleRow; } set { singleRow = value; } }

    }
}
