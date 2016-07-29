using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_Model {
    /// <summary>
    /// 采集任务标签实体类
    /// </summary>
    public class ModelTaskLabel {
        #region Model
        private int _id;
        private string _labelname;
        private string _labelcutstart;
        private string _labelcutend;
        private string _labelhtmlremove;
        private string _labelremove;
        private string _labelreplace;
        private int? _taskid;
        private string _guidnum;
        private int? _orderid;
        private string _createtime;
        private string _spiderLabelPlugin;
        /// <summary>
        /// 标签ID
        /// </summary>
        public int ID {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string LabelName {
            set { _labelname = value; }
            get { return _labelname; }
        }
        /// <summary>
        /// 标签截取表达式
        /// </summary>
        public string LabelNameCutRegex {
            set { _labelcutstart = value; }
            get { return _labelcutstart; }
        }
        /// <summary>
        /// 标签Html移除
        /// </summary>
        public string LabelHtmlRemove {
            set { _labelhtmlremove = value; }
            get { return _labelhtmlremove; }
        }
        /// <summary>
        /// 标签内容移除
        /// </summary>
        public string LabelRemove {
            set { _labelremove = value; }
            get { return _labelremove; }
        }
        /// <summary>
        /// 标签内容过滤
        /// </summary>
        public string LabelReplace {
            set { _labelreplace = value; }
            get { return _labelreplace; }
        }
        /// <summary>
        /// 所属任务ID
        /// </summary>
        public int? TaskID {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GuidNum {
            set { _guidnum = value; }
            get { return _guidnum; }
        }
        /// <summary>
        /// 排序ID
        /// </summary>
        public int? OrderID {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime {
            set { _createtime = value; }
            get { return _createtime; }
        }


        private int? _IsLoop = 0;
        /// <summary>
        /// 标签是循环标签
        /// </summary>
        public int? IsLoop {
            get { return _IsLoop; }
            set { _IsLoop = value; }
        }
        int? _IsNoNull = 0;
        /// <summary>
        /// 标签不得为空
        /// </summary>
        public int? IsNoNull {
            get { return _IsNoNull; }
            set { _IsNoNull = value; }
        }

        int? _IsLinkUrl = 0;
        /// <summary>
        /// 标签是链接
        /// </summary>
        public int? IsLinkUrl {
            get { return _IsLinkUrl; }
            set { _IsLinkUrl = value; }
        }
        int? _IsPager = 0;
        /// <summary>
        /// 标签是分页
        /// </summary>
        public int? IsPager {
            get { return _IsPager; }
            set { _IsPager = value; }
        }
        string _LabelValueLinkUrlRegex = string.Empty;
        /// <summary>
        /// 标签是链接表达式
        /// </summary>
        public string LabelValueLinkUrlRegex {
            get { return _LabelValueLinkUrlRegex; }
            set { _LabelValueLinkUrlRegex = value; }
        }
        string _LabelValuePagerRegex = string.Empty;
        /// <summary>
        /// 标签是分页表达式
        /// </summary>
        public string LabelValuePagerRegex {
            get { return _LabelValuePagerRegex; }
            set { _LabelValuePagerRegex = value; }
        }
        /// <summary>
        /// 采集标签插件
        /// </summary>
        public string SpiderLabelPlugin {
            get { return _spiderLabelPlugin; }
            set { _spiderLabelPlugin = value; }
        }
        #endregion Model


        private int _IsDownResource;

        public int IsDownResource {
            get { return _IsDownResource; }
            set { _IsDownResource = value; }
        }

        private string _DownResouceExts;

        public string DownResourceExts {
            get { return _DownResouceExts; }
            set { _DownResouceExts = value; }
        }
        

    }
}
