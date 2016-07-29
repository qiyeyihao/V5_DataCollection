using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_DataCollection.Forms.Publish;

namespace V5_Model {
    /// <summary>
    /// 采集任务实体类
    /// </summary>
    [Serializable]
    public class ModelTask {
        #region Model
        private int _id;
        private string _taskname;
        private int? _isspiderurl;
        private int? _isspidercontent;
        private int? _ispublishcontent;
        private string _pageencode;
        private int? _collectiontype;
        private string _collectioncontent;
        private string _linkurlmustincludestr;
        private string _linkurlnomustincludestr;
        private string _linkurlcutareastart;
        private string _linkurlcutareaend;
        private string _testviewurl;
        private int? _iswebonlinepublish1;
        private int? _issavelocal2;
        private string _savefileformat2;
        private string _savedirectory2;
        private string _savehtmltemplate2;
        private int? _saveiscreateindex2;
        private int? _issavedatabase3;
        private int? _savedatatype3;
        private string _savedataurl3;
        private string _savedatasql3;
        private int? _issavesql4;
        private string _savesqlcontent4;
        private string _savesqldirectory4;
        private string _guid;


        /// <summary>
        /// 任务ID
        /// </summary>
        public int ID {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 任务分类ID
        /// </summary>
        public int TaskClassID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName {
            set { _taskname = value; }
            get { return _taskname; }
        }
        /// <summary>
        /// 是否采集列表地址
        /// </summary>
        public int? IsSpiderUrl {
            set { _isspiderurl = value; }
            get { return _isspiderurl; }
        }
        /// <summary>
        /// 是否采集内容
        /// </summary>
        public int? IsSpiderContent {
            set { _isspidercontent = value; }
            get { return _isspidercontent; }
        }
        /// <summary>
        /// 是否发布内容
        /// </summary>
        public int? IsPublishContent {
            set { _ispublishcontent = value; }
            get { return _ispublishcontent; }
        }
        /// <summary>
        /// 页面编码
        /// </summary>
        public string PageEncode {
            set { _pageencode = value; }
            get { return _pageencode; }
        }
        /// <summary>
        /// 采集类型
        /// </summary>
        public int? CollectionType {
            set { _collectiontype = value; }
            get { return _collectiontype; }
        }
        /// <summary>
        /// 采集列表内容
        /// </summary>
        public string CollectionContent {
            set { _collectioncontent = value; }
            get { return _collectioncontent; }
        }
        /// <summary>
        /// 链接包含
        /// </summary>
        public string LinkUrlMustIncludeStr {
            set { _linkurlmustincludestr = value; }
            get { return _linkurlmustincludestr; }
        }
        /// <summary>
        /// 链接不包含
        /// </summary>
        public string LinkUrlNoMustIncludeStr {
            set { _linkurlnomustincludestr = value; }
            get { return _linkurlnomustincludestr; }
        }
        /// <summary>
        /// 列表截取开始区域
        /// </summary>
        public string LinkUrlCutAreaStart {
            set { _linkurlcutareastart = value; }
            get { return _linkurlcutareastart; }
        }
        /// <summary>
        /// 列表截取结束区域
        /// </summary>
        public string LinkUrlCutAreaEnd {
            set { _linkurlcutareaend = value; }
            get { return _linkurlcutareaend; }
        }
        /// <summary>
        /// 测试列表地址
        /// </summary>
        public string TestViewUrl {
            set { _testviewurl = value; }
            get { return _testviewurl; }
        }
        /// <summary>
        /// 是否在线发布
        /// </summary>
        public int? IsWebOnlinePublish1 {
            set { _iswebonlinepublish1 = value; }
            get { return _iswebonlinepublish1; }
        }
        /// <summary>
        /// 是否保存本地
        /// </summary>
        public int? IsSaveLocal2 {
            set { _issavelocal2 = value; }
            get { return _issavelocal2; }
        }
        /// <summary>
        ///
        /// </summary>
        public string SaveFileFormat2 {
            set { _savefileformat2 = value; }
            get { return _savefileformat2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SaveDirectory2 {
            set { _savedirectory2 = value; }
            get { return _savedirectory2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SaveHtmlTemplate2 {
            set { _savehtmltemplate2 = value; }
            get { return _savehtmltemplate2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SaveIsCreateIndex2 {
            set { _saveiscreateindex2 = value; }
            get { return _saveiscreateindex2; }
        }
        /// <summary>
        /// 是否保存数据库
        /// </summary>
        public int? IsSaveDataBase3 {
            set { _issavedatabase3 = value; }
            get { return _issavedatabase3; }
        }
        /// <summary>
        /// 保存数据库类型
        /// </summary>
        public int? SaveDataType3 {
            set { _savedatatype3 = value; }
            get { return _savedatatype3; }
        }
        /// <summary>
        /// 保存数据库地址
        /// </summary>
        public string SaveDataUrl3 {
            set { _savedataurl3 = value; }
            get { return _savedataurl3; }
        }
        /// <summary>
        /// 保存数据库SQL语句
        /// </summary>
        public string SaveDataSQL3 {
            set { _savedatasql3 = value; }
            get { return _savedatasql3; }
        }
        /// <summary>
        /// 是否保存SQL语句
        /// </summary>
        public int? IsSaveSQL4 {
            set { _issavesql4 = value; }
            get { return _issavesql4; }
        }
        /// <summary>
        /// SQL内容
        /// </summary>
        public string SaveSQLContent4 {
            set { _savesqlcontent4 = value; }
            get { return _savesqlcontent4; }
        }
        /// <summary>
        /// 保存目录
        /// </summary>
        public string SaveSQLDirectory4 {
            set { _savesqldirectory4 = value; }
            get { return _savesqldirectory4; }
        }

        /// <summary>
        /// 生成Guid
        /// </summary>
        public string Guid {
            get { return _guid; }
            set { _guid = value; }
        }

        string _PluginSpiderUrl;
        /// <summary>
        /// 采集地址结束后插件
        /// </summary>
        public string PluginSpiderUrl {
            get { return _PluginSpiderUrl; }
            set { _PluginSpiderUrl = value; }
        }
        string _PluginSpiderContent;
        /// <summary>
        /// 采集内容结束后插件
        /// </summary>
        public string PluginSpiderContent {
            get { return _PluginSpiderContent; }
            set { _PluginSpiderContent = value; }
        }
        string _PluginSaveContent;
        /// <summary>
        /// 保存内容结束后插件
        /// </summary>
        public string PluginSaveContent {
            get { return _PluginSaveContent; }
            set { _PluginSaveContent = value; }
        }
        string _PluginPublishContent;
        /// <summary>
        /// 发布内容前插件
        /// </summary>
        public string PluginPublishContent {
            get { return _PluginPublishContent; }
            set { _PluginPublishContent = value; }
        }
        #endregion Model

        /// <summary>
        /// 任务标签
        /// </summary>

        public List<ModelTaskLabel> ListTaskLabel { set; get; }
        /// <summary>
        /// 任务发布模块
        /// </summary>
        public List<ModelWebPublishModule> ListModelWebPublishModule { set; get; }


        private int _CollectionContentThreadCount = 1;
        /// <summary>
        /// 采集内容线程个数
        /// </summary>
        public int CollectionContentThreadCount {
            get { return _CollectionContentThreadCount; }
            set { _CollectionContentThreadCount = value; }
        }

        private int _CollectionContentStepTime = 500;
        /// <summary>
        /// 采集内容间隔时间
        /// </summary>
        public int CollectionContentStepTime {
            get { return _CollectionContentStepTime; }
            set { _CollectionContentStepTime = value; }
        }

        private int _PublishContentThreadCount = 1;
        /// <summary>
        /// 发布内容线程个数
        /// </summary>
        public int PublishContentThreadCount {
            get { return _PublishContentThreadCount; }
            set { _PublishContentThreadCount = value; }
        }

        private int _PublishContentStepTimeMin = 500;
        /// <summary>
        /// 发布内容间隔最小值
        /// </summary>
        public int PublishContentStepTimeMin {
            get { return _PublishContentStepTimeMin; }
            set { _PublishContentStepTimeMin = value; }
        }

        private int _PublishContentStepTimeMax = 5000;
        /// <summary>
        /// 发布内容间隔最大值
        /// </summary>
        public int PublishContentStepTimeMax {
            get { return _PublishContentStepTimeMax; }
            set { _PublishContentStepTimeMax = value; }
        }

        private int _Status = 0;
        /// <summary>
        /// 任务状态
        /// </summary>
        public int Status {
            get { return _Status; }
            set { _Status = value; }
        }


        int _IsHandGetUrl = 0;
        /// <summary>
        /// 是否手动获取地址
        /// </summary>
        public int IsHandGetUrl {
            get { return _IsHandGetUrl; }
            set { _IsHandGetUrl = value; }
        }


        string _HandCollectionUrlRegex = string.Empty;
        /// <summary>
        /// 手动获取地址表达式
        /// </summary>
        public string HandCollectionUrlRegex {
            get { return _HandCollectionUrlRegex; }
            set { _HandCollectionUrlRegex = value; }
        }

        string _DemoListUrl = string.Empty;
        /// <summary>
        /// 测试列表地址
        /// </summary>
        public string DemoListUrl {
            get { return _DemoListUrl; }
            set { _DemoListUrl = value; }
        }

        private int _IsPlan;

        public int IsPlan {
            get { return _IsPlan; }
            set { _IsPlan = value; }
        }

        private string _PlanFormat;

        public string PlanFormat {
            get { return _PlanFormat; }
            set { _PlanFormat = value; }
        }

    }
}
