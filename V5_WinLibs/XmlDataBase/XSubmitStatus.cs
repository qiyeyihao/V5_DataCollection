using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDatabase
{
    /// <summary>
    /// 该类型表示了提交的状态
    /// </summary>
    public class XSubmitStatus
    {
        #region 私有成员
        private XSubmitStatus() { }
        private bool _error;
        private string _message;
        #endregion

        internal XSubmitStatus(bool error, string message) {
            _error = error;
            _message = message;
        }

        /// <summary>
        /// 指示当前状态是否包含了错误
        /// </summary>
        public bool HasError
        {
            get { return _error; }
        }
        /// <summary>
        /// 指示当前状态的完整消息
        /// </summary>
        public string Message
        {
            get { return _message; }
        }
    }
}
