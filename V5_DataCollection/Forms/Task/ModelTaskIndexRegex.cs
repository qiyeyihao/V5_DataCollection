using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_Model;

namespace V5_DataCollection.Forms.Task {
    public class ModelTaskIndexRegex {
        public string Guid { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 任务地址
        /// </summary>
        public string TaskUrl { get; set; }
        /// <summary>
        /// 任务编码
        /// </summary>
        public string TaskEncode { get; set; }
        /// <summary>
        /// 任务Cookies
        /// </summary>
        public string TaskCookies { get; set; }
        /// <summary>
        /// 截取标签
        /// </summary>
        public List<ModelTaskLabel> ListTaskLabel = new List<ModelTaskLabel>();
    }
}
