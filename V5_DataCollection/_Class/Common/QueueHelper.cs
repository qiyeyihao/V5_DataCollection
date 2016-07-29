using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_DataCollection._Class.Common {
    /// <summary>
    /// 队列列表
    /// </summary>
    public class QueueHelper {
        public readonly static object lockObj = new object();
        /// <summary>
        /// 下载图片资源
        /// </summary>
        public static Queue<Dictionary<string, string>> Q_DownImgResource = new Queue<Dictionary<string, string>>();

    }
}
