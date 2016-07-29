using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using V5_DataCollection._Class.Common;
using V5_DataCollection._Class.Publish;
using V5_Model;
using V5_WinLibs;

using System.Data.Linq;
using V5_DataCollection._Class.DAL;
using System.Data.Linq;
using System.IO;
using V5_WinLibs.DBHelper;
using V5_WinLibs.Utility;
using V5_WinLibs.Core;
using V5_Utility.Utility;

namespace V5_DataCollection._Class.Gather {
    /// <summary>
    /// 采集管理
    /// </summary>
    public class SpiderHelper {

        #region 访问器变量
        /// <summary>
        /// 是否停止
        /// </summary>
        public bool Stopped { get; set; }
        /// <summary>
        /// 任务索引
        /// </summary>
        public int TaskIndex { get; set; }
        private ModelTask _modelTask = new ModelTask();
        /// <summary>
        /// 任务模型
        /// </summary>
        public ModelTask modelTask {
            get { return _modelTask; }
            set { _modelTask = value; }
        }
        #endregion

        #region 委托变量
        /// <summary>
        /// 采集过程中
        /// </summary>
        public GatherEventHandler.GatherWorkHandler GatherWorkDelegate;
        /// <summary>
        /// 采集完成
        /// </summary>
        public GatherEventHandler.GatherComplateHandler GatherComplateDelegate;
        /// <summary>
        /// 采集进度条
        /// </summary>
        public MainEventHandler.OutPutTaskProgressBarHandler OutPutTaskProgressBarDelegate;
        #endregion

        #region 私有变量
        private GatherEvents.GatherLinkEvents gatherEv = new GatherEvents.GatherLinkEvents();

        private Queue<ModelLinkUrl> _listLinkUrl = new Queue<ModelLinkUrl>();
        private ThreadMultiHelper _tmViewUrl, _tmLinkUrl;
        private cGatherFunction _gatherWork = new cGatherFunction();
        #endregion

        #region 构造函数

        public SpiderHelper() {

        }

        ~SpiderHelper() {
            modelTask = null;
        }

        #endregion

        /// <summary>
        /// 消息输出
        /// </summary>
        private void MessageOut(string strMsg) {
            if (GatherWorkDelegate != null) {
                gatherEv.Message = strMsg;
                GatherWorkDelegate(this, gatherEv);
            }
        }

        public void Stop() {
            this.Stopped = true;
            modelTask = null;
            _tmLinkUrl.Stop();
        }
        /// <summary>
        /// 开始采集网址列表
        /// </summary>
        public void Start() {
            if (this.Stopped || modelTask == null) {
                return;
            }
            MessageOut("开始采集数据！请稍候...");
            _tmLinkUrl = new ThreadMultiHelper(1, 1);
            _tmLinkUrl.WorkMethod += Run_Task;
            _tmLinkUrl.CompleteEvent += Complete_Task;
            _tmLinkUrl.Start();
        }

        #region 列表采集
        /// <summary>
        /// 采集网址列表
        /// </summary>
        private void GetAllLinkUrl(string urlList) {
            string pageContent = CollectionHelper.Instance.GetHttpPage(urlList, 100000, Encoding.GetEncoding(modelTask.PageEncode));
            if (pageContent == "$StartFalse$" || pageContent == "$EndFalse$") {
                MessageOut(urlList + "采集地址失败!结果:" + pageContent);
                return;
            }
            if (modelTask.LinkUrlCutAreaStart != null && modelTask.LinkUrlCutAreaEnd != null) {
                pageContent = HtmlHelper.Instance.ParseCollectionStrings(pageContent);
                pageContent = CollectionHelper.Instance.GetBody(pageContent,
                    HtmlHelper.Instance.ParseCollectionStrings(modelTask.LinkUrlCutAreaStart),
                    HtmlHelper.Instance.ParseCollectionStrings(modelTask.LinkUrlCutAreaEnd),
                    false,
                    false);
                pageContent = HtmlHelper.Instance.UnParseCollectionStrings(pageContent);
            }
            string regexHref = cRegexHelper.RegexATag;
            if (modelTask.IsHandGetUrl == 1) {
                regexHref = modelTask.HandCollectionUrlRegex;
                regexHref = HtmlHelper.Instance.ParseCollectionStrings(regexHref);
                regexHref = regexHref.Replace("\\(\\*)", ".+?");
                regexHref = regexHref.Replace("\\[参数]", "([\\S\\s].*?)");
            }
            Match mch = null;
            Regex reg = new Regex(regexHref, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string url = string.Empty, title = string.Empty;

            for (mch = reg.Match(pageContent); mch.Success; mch = mch.NextMatch()) {
                Thread.Sleep(1);
                title = mch.Groups[2].Value;
                if (string.IsNullOrEmpty(title)) {
                    continue;
                }
                url = CollectionHelper.Instance.FormatUrl(urlList, mch.Groups[1].Value);
                url = url.Replace("\\", "");
                bool isLoop = false;
                if (modelTask.LinkUrlMustIncludeStr != null) {
                    //包含
                    if (url.IndexOf(Convert.ToString(modelTask.LinkUrlMustIncludeStr)) == -1) {
                        continue;
                    }
                }
                //不包含
                if (modelTask.LinkUrlNoMustIncludeStr != null) {
                    foreach (string str in modelTask.LinkUrlNoMustIncludeStr.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)) {
                        if (url.IndexOf(str) > -1) {
                            isLoop = true;
                            break;
                        }
                    }
                }
                if (isLoop) {
                    continue;
                }
                ModelLinkUrl m = new ModelLinkUrl();
                m.Url = url;
                m.Title = title;
                //添加Url
                bool addFlag = true;
                foreach (var item in _listLinkUrl.ToArray()) {
                    if (item.Url == url) {
                        addFlag = false;
                        break;
                    }
                }
                if (addFlag) {
                    //开始过滤数据库存在的数据
                    string msg = url + "==" + HtmlHelper.Instance.ParseTags(title);
                    if (!DALContentHelper.ChkExistSpiderResult(modelTask.TaskName, url)) {
                        _listLinkUrl.Enqueue(m);
                    }
                    else {
                        msg += "采集地址存在!不需要采集!";
                    }
                    MessageOut(msg);
                }
            }
        }
        /// <summary>
        /// 分析网址列表
        /// </summary>
        private void Run_Task(int index, int threadindex) {
            if (modelTask == null) {
                return;
            }
            if (modelTask.IsSpiderUrl == 1) {
                try {
                    MessageOut("正在分析采集列表个数!");
                    foreach (string linkUrl in modelTask.CollectionContent.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                        try {
                            List<string> TestListLinkUrl = _gatherWork.SplitWebUrl(linkUrl);
                            foreach (string str in TestListLinkUrl) {
                                GetAllLinkUrl(str);
                                //暂停
                                Thread.Sleep(1 * 1000);
                            }
                        }
                        catch (Exception ex1) {
                            Log4Helper.Write(V5_Utility.Utility.LogLevel.Error, ex1.StackTrace, ex1);
                            continue;
                        }
                    }
                }
                catch (Exception ex) {
                    //MessageOut("分析网页列表出错!" + ex.Message);
                    Log4Helper.Write(V5_Utility.Utility.LogLevel.Error, ex.StackTrace, ex);
                }
            }
        }
        /// <summary>
        /// 采集列表完成
        /// </summary>
        private void Complete_Task() {
            MessageOut("分析获取网页个数为" + _listLinkUrl.Count + "个！");
            MessageOut("采集网站列表完成!");
            Bind_SpiderContentStart();
        }

        private int TaskCount = 0;
        #endregion

        #region 内容采集
        /// <summary>
        /// 采集内容
        /// </summary>
        private void Bind_SpiderContentStart() {
            if (modelTask.IsSpiderContent == 1 && _listLinkUrl.Count > 0) {
                MessageOut("开始采集列表地址详细内容!");
                //开始采集内容
                int workcount = _listLinkUrl.Count == 0 ? 1 : _listLinkUrl.Count;
                TaskCount = workcount;
                _tmViewUrl = new ThreadMultiHelper(TaskCount, 12);
                _tmViewUrl.WorkMethod += Run_ViewUrl;
                _tmViewUrl.CompleteEvent += Complete_ViewUrl;
                _tmViewUrl.Start();
            }
            else {
                MessageOut("采集网站内容选项关闭!或者采集列表的地址为0!不需要采集!");
                //开始发布内容
                this.Bind_PublishContentStart();
            }
        }
        private int ProressNum = 0;

        private void Run_ViewUrl(int index, int threadindex) {
            if (modelTask.IsSpiderContent == 1) {
                if (_listLinkUrl.Count > 0) {
                    ProressNum++;
                    if (OutPutTaskProgressBarDelegate != null) {
                        MainEvents.OutPutTaskProgressBarEventArgs ea = new MainEvents.OutPutTaskProgressBarEventArgs();
                        ea.ProgressNum = ProressNum;
                        ea.RecordNum = TaskCount;
                        ea.TaskIndex = TaskIndex;
                        OutPutTaskProgressBarDelegate(this, ea);
                    }
                    ModelLinkUrl mlink = _listLinkUrl.Dequeue();
                    string url = mlink.Url;
                    string SQL = string.Empty, cutContent = string.Empty;
                    string pageContent = CollectionHelper.Instance.GetHttpPage(url, 1000, Encoding.GetEncoding(modelTask.PageEncode));
                    string title = CollectionHelper.Instance.CutStr(pageContent, "<title>([\\S\\s]*?)</title>")[0];
                    StringBuilder sb1 = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    StringBuilder strSql = new StringBuilder();
                    StringBuilder sb3 = new StringBuilder();
                    foreach (ModelTaskLabel m in modelTask.ListTaskLabel) {
                        string regContent = HtmlHelper.Instance.ParseCollectionStrings(m.LabelNameCutRegex);
                        regContent = CommonHelper.ReplaceSystemRegexTag(regContent);
                        string CutContent = CollectionHelper.Instance.CutStr(pageContent, regContent)[0];
                        #region 替换内容中的链接为远程链接
                        string[] TagImgList = CollectionHelper.Instance.GetImgTag(CutContent);
                        foreach (string tagimg in TagImgList) {
                            if (string.IsNullOrEmpty(tagimg)) {
                                break;
                            }
                            //远程连接
                            string newTagImg = CollectionHelper.Instance.FormatUrl(modelTask.TestViewUrl, tagimg);
                            //替换连接
                            CutContent = CutContent.Replace(tagimg, newTagImg);
                            #region 保存远程图片
                            if (m.IsDownResource == 1) {
                                //替换时间格式连接
                                FileInfo fImg = new FileInfo(newTagImg);
                                string ext = fImg.Extension;
                                ext = string.IsNullOrEmpty(ext) ? ".jpg" : ext;
                                string newTimeImg = "images/"+DateTime.Now.ToString("yyyyMMddHHmmss") + ext;

                                lock (QueueHelper.lockObj) {
                                    var d = new Dictionary<string, string>();
                                    d.Add(newTagImg, newTimeImg);
                                    QueueHelper.Q_DownImgResource.Enqueue(d);
                                }
                            }
                            #endregion
                        }
                        #endregion
                        if (m.IsLoop == 1) {
                            string[] LabelString = CollectionHelper.Instance.CutStr(pageContent, regContent);
                            foreach (string s in LabelString) {
                                CutContent += s + "$$$$";
                            }
                            int n = CutContent.LastIndexOf("$$$$");
                            CutContent = CutContent.Remove(n, 4);
                        }
                        if (m.IsLinkUrl == 1) {
                            string[] CutContentArr = CutContent.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string sUrl in CutContentArr) {
                                CutContent = CollectionHelper.Instance.DefiniteUrl(sUrl, modelTask.TestViewUrl);//地址
                                CutContent = CollectionHelper.Instance.GetHttpPage(CutContent, 1000, Encoding.GetEncoding(modelTask.PageEncode));
                                regContent = HtmlHelper.Instance.ParseCollectionStrings(m.LabelValueLinkUrlRegex);
                                regContent = regContent.Replace("\\(\\*)", ".+?");
                                regContent = regContent.Replace("\\[参数]", "([\\S\\s].*?)");
                                CutContent = CollectionHelper.Instance.CutStr(CutContent, regContent)[0];
                            }
                        }
                        #region 标签是分页
                        if (m.IsPager == 1) {
                            regContent = HtmlHelper.Instance.ParseCollectionStrings(m.LabelValuePagerRegex);
                            regContent = regContent.Replace("\\(\\*)", ".+?");
                            regContent = regContent.Replace("\\[参数]", "([\\S\\s].*?)");
                            string[] LabelString = CollectionHelper.Instance.CutStr(pageContent, regContent);

                            foreach (string pageUrl in LabelString) {
                                string url1 = CollectionHelper.Instance.DefiniteUrl(pageUrl, url);
                                string pageContentPager = CollectionHelper.Instance.GetHttpPage(url1, 100000);
                                if (pageContent.Equals("$UrlIsFalse$") || pageContent.Equals("$GetFalse$")) {

                                    CutContent += "=====分页内容=======================================================\r\n";
                                    CutContent += "远程链接内容失败!";
                                }
                                else {
                                    //重新截取标签
                                    string regContent1 = HtmlHelper.Instance.ParseCollectionStrings(m.LabelNameCutRegex);
                                    regContent1 = CommonHelper.ReplaceSystemRegexTag(regContent1);
                                    string CutContent1 = CollectionHelper.Instance.CutStr(pageContentPager, regContent1)[0];

                                    CutContent += "=====分页内容=======================================================\r\n";
                                    CutContent += CutContent1;
                                }
                            }
                        }
                        #endregion
                        #region 过滤Html
                        if (!string.IsNullOrEmpty(m.LabelHtmlRemove)) {
                            //CutContent = HtmlHelper.ReplaceNormalHtml(CutContent, model.TestViewUrl, false);
                            string[] arr = m.LabelHtmlRemove.Split(new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string str in arr) {
                                if (str == "all") {
                                    CutContent = CollectionHelper.Instance.NoHtml(CutContent);
                                    break;
                                }
                                else if (str == "table") {
                                    CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "table", 2);
                                }
                                else if (str == "font<span>") {
                                    CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "font", 3);
                                    CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "span", 3);
                                }
                                else if (str == "a") {
                                    CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "a", 3);
                                }
                            }
                        }
                        #endregion
                        #region 排除字符
                        if (!string.IsNullOrEmpty(m.LabelRemove)) {
                            foreach (string str in m.LabelRemove.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                                CutContent = CutContent.Replace(str, "");
                            }
                        }
                        #endregion
                        #region 替换字符
                        if (!string.IsNullOrEmpty(m.LabelReplace)) {
                            foreach (string str in m.LabelReplace.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                                string[] ListStr = str.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                                CutContent = CutContent.Replace(ListStr[0], ListStr[1]);
                            }
                        }
                        #endregion
                        sb1.Append("" + m.LabelName.Replace("'", "''") + ",");
                        sb2.Append("'" + CutContent.Replace("'", "''") + "',");
                        if (CutContent.Replace("'", "''").Length < 100) {
                            sb3.Append(" " + m.LabelName.Replace("'", "''") + "='" + CutContent.Replace("'", "''") + "' and");
                        }
                        //添加文件下载功能  开关打开的时候
                        if (m.IsDownResource == 1) {
                            string[] imgExtArr = m.DownResourceExts.Split(new string[] { ";" }, StringSplitOptions.None);
                            foreach (string s in imgExtArr) {

                            }
                            string downImgPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\" + modelTask.TaskName + "\\Images\\";
                            CutContent = ImageDownHelper.SaveUrlPics(CutContent, downImgPath);
                        }
                    }

                    string LocalSQLiteName = "Data\\Collection\\" + modelTask.TaskName + "\\SpiderResult.db";
                    string sql = " Select Count(1) From Content Where HrefSource='" + url + "' ";
                    object o = SQLiteHelper.ExecuteScalar(LocalSQLiteName,sql);
                    if (Convert.ToInt32("0" + o) == 0) {

                        strSql.Append("insert into Content(HrefSource,");
                        strSql.Append(sb1.ToString().Remove(sb1.Length - 1));
                        strSql.Append(")");
                        strSql.Append(" values ('" + url + "',");
                        strSql.Append(sb2.ToString().Remove(sb2.Length - 1));
                        strSql.Append(")");

                        SQLiteHelper.Execute(LocalSQLiteName,strSql.ToString());
                    }



                    title = title.Replace('\\', ' ').Replace('/', ' ').Split(new char[] { '_' })[0].Split(new char[] { '-' })[0];
                    gatherEv.Message = mlink.Url + "=" + title;
                    GatherWorkDelegate(this, gatherEv);
                }
                else {
                    gatherEv.Message = "没有采集到任何地址！不需要采集!";
                    GatherWorkDelegate(this, gatherEv);
                }
                //暂停
                var r = new Random();
                var stepNext = r.Next(1, 4);
                Thread.Sleep(stepNext * 2000);
            }
        }

        public void Complete_ViewUrl() {
            _listLinkUrl.Clear();
            MessageOut("采集网站Url内容完成！");
            Bind_PublishContentStart();
        }
        #endregion

        #region 发布数据
        private void Bind_PublishContentStart() {
            if (modelTask.IsPublishContent == 1) {
                PublishHelper publich = new PublishHelper();
                publich.ModelTask = modelTask;
                publich.PublishCompalteDelegate = GatherWorkDelegate;

                MessageOut("正在开始发布数据!");
                publich.PublishCompalteDelegate = PublishCompalteDelegate;
                ThreadPool.QueueUserWorkItem(new WaitCallback(publich.Start));
            }
            else {
                GatherComplateDelegate(modelTask);
                MessageOut("发布数据没有开启!不需要发布数据!");
                this.Stop();
            }
        }

        private void PublishCompalteDelegate(object sender, GatherEvents.GatherLinkEvents e) {
            MessageOut(e.Message);
            if (GatherComplateDelegate != null)
                GatherComplateDelegate(modelTask);
            this.Stop();
        }
        #endregion
    }
}
