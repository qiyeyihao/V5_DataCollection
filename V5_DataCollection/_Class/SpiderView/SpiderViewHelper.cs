using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_DataCollection._Class.Common;
using V5_DataCollection._Class.Gather;

using V5_Model;
using V5_Utility.Core;
using V5_Utility.Utility;
using V5_WinLibs.DBHelper;
using V5_WinLibs.Utility;

namespace V5_DataCollection._Class.SpiderView {
    /// <summary>
    /// 采集结果显示
    /// </summary>
    public class SpiderViewHelper {
        #region 属性访问器
        private ModelTask model = new ModelTask();
        /// <summary>
        /// 任务模型
        /// </summary>
        public ModelTask ModelTask {
            get { return model; }
            set { model = value; }
        }

        private Queue<ModelLinkUrl> m_ListLinkUrl = new Queue<ModelLinkUrl>();

        public Queue<ModelLinkUrl> ListLinkUrl {
            get { return m_ListLinkUrl; }
            set { m_ListLinkUrl = value; }
        }
        #endregion


        private GatherEvents.GatherLinkEvents gatherEv = new GatherEvents.GatherLinkEvents();
        private cGatherFunction gatherWork = new cGatherFunction();

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

        ThreadMultiHelper th;
        int TaskCount = 0;
        public int TaskIndex { get; set; }
        /// <summary>
        /// 采集内容
        /// </summary>
        private void Start() {
            if (ModelTask.IsSpiderContent == 1 && ListLinkUrl.Count > 0) {
                MessageShow("开始采集列表地址详细内容!");
                //开始采集内容
                int workcount = ListLinkUrl.Count == 0 ? 1 : ListLinkUrl.Count;
                TaskCount = workcount;
                th = new ThreadMultiHelper(TaskCount, 12);
                th.WorkMethod += Run;
                th.CompleteEvent += Complete;
                th.Start();
            }
            else {
                MessageShow("采集网站内容选项关闭!或者采集列表的地址为0!不需要采集!");
            }
        }
        private int ProressNum = 0;

        private void Run(int index, int threadindex) {
            if (ModelTask.IsSpiderContent == 1) {
                if (ListLinkUrl.Count > 0) {
                    ProressNum++;
                    if (OutPutTaskProgressBarDelegate != null) {
                        MainEvents.OutPutTaskProgressBarEventArgs ea = new MainEvents.OutPutTaskProgressBarEventArgs();
                        ea.ProgressNum = ProressNum;
                        ea.RecordNum = TaskCount;
                        ea.TaskIndex = TaskIndex;
                        OutPutTaskProgressBarDelegate(this, ea);
                    }
                    ModelLinkUrl mlink = ListLinkUrl.Dequeue();
                    string url = mlink.Url;
                    string SQL = string.Empty, cutContent = string.Empty;
                    string pageContent = CollectionHelper.Instance.GetHttpPage(url, 1000, Encoding.GetEncoding(ModelTask.PageEncode));
                    string title = CollectionHelper.Instance.CutStr(pageContent, "<title>([\\S\\s]*?)</title>")[0];
                    StringBuilder sb1 = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    StringBuilder strSql = new StringBuilder();
                    StringBuilder sb3 = new StringBuilder();
                    foreach (ModelTaskLabel m in ModelTask.ListTaskLabel) {
                        string regContent = HtmlHelper.Instance.ParseCollectionStrings(m.LabelNameCutRegex);
                        regContent = CommonHelper.ReplaceSystemRegexTag(regContent);
                        string CutContent = CollectionHelper.Instance.CutStr(pageContent, regContent)[0];
                        #region 替换内容中的链接为远程链接
                        string[] TagImgList = CollectionHelper.Instance.GetImgTag(CutContent);
                        foreach (string tagimg in TagImgList) {
                            if (string.IsNullOrEmpty(tagimg)) {
                                break;
                            }
                            string newTagImg = CollectionHelper.Instance.FormatUrl(ModelTask.TestViewUrl, tagimg);
                            CutContent = CutContent.Replace(tagimg, newTagImg);
                            #region 保存远程图片
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
                                CutContent = CollectionHelper.Instance.DefiniteUrl(sUrl, ModelTask.TestViewUrl);//地址
                                CutContent = CollectionHelper.Instance.GetHttpPage(CutContent, 1000, Encoding.GetEncoding(ModelTask.PageEncode));
                                regContent = HtmlHelper.Instance.ParseCollectionStrings(m.LabelValueLinkUrlRegex);
                                regContent = regContent.Replace("\\(\\*)", ".+?");
                                regContent = regContent.Replace("\\[参数]", "([\\S\\s].*?)");
                                CutContent = CollectionHelper.Instance.CutStr(CutContent, regContent)[0];
                            }
                        }
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
                            string downImgPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\" + ModelTask.TaskName + "\\Images\\";
                            CutContent = ImageDownHelper.SaveUrlPics(CutContent, downImgPath);
                        }
                    }

                    strSql.Append("insert into Content(HrefSource,");
                    strSql.Append(sb1.ToString().Remove(sb1.Length - 1));
                    strSql.Append(")");
                    strSql.Append(" values ('" + url + "',");
                    strSql.Append(sb2.ToString().Remove(sb2.Length - 1));
                    strSql.Append(")");

                    string LocalSQLiteName = "Data\\Collection\\" + ModelTask.TaskName + "\\SpiderResult.db";
                    SQLiteHelper.Execute(LocalSQLiteName,strSql.ToString());
                    title = title.Replace('\\', ' ').Replace('/', ' ').Split(new char[] { '_' })[0].Split(new char[] { '-' })[0];
                    gatherEv.Message = mlink.Url + "=" + title;
                    GatherWorkDelegate(this, gatherEv);
                }
                else {
                    MessageShow("没有采集到任何地址！不需要采集!");
                }
            }
        }

        public void Complete() {
            ListLinkUrl.Clear();
            MessageShow("采集网站Url内容完成！");
        }

        /// <summary>
        /// 消息输出
        /// </summary>
        private void MessageShow(string strMsg) {
            if (GatherWorkDelegate != null) {
                gatherEv.Message = strMsg;
                GatherWorkDelegate(this, gatherEv);
            }
        }
    }
}
