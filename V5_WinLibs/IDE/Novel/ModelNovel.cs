using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V5_IDE._Class.Novel {
    public class ModelNovel {

        private string _Id;
        /// <summary>
        /// 索引Id
        /// </summary>
        public string Id {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Content;
        /// <summary>
        /// 索引内容
        /// </summary>
        public string Content {
            get { return _Content; }
            set { _Content = value; }
        }

        private DateTime _CreateTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }


    }
}
