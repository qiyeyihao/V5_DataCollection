using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using V5_Model;
using System.IO;
using V5_DataCollection._Class.Gather;
using V5_DataCollection.Forms.Publish;
using V5_DataCollection._Class.Common;
using V5_DataCollection.Forms.Task.DataBaseConfig;

using System.Diagnostics;
using V5_DataPlugins;
using V5_DataCollection._Class.PythonExt;
using V5_DataCollection.Forms.Task.Tools;
using V5_DataCollection._Class.DAL;
using V5_DataCollection._Class.Plan;
using V5_DataCollection._Class;
using V5_WinLibs.DBHelper;

using V5_Utility.Core;
using V5_WinUtility.Core;
using V5_WinUtility.Expand;
using V5_WinUtility;
using V5_Utility.Utility;
using V5_WinLibs.Core;

namespace V5_DataCollection.Forms.Task {
    public partial class FrmTaskEdit : Form {

        private cGatherFunction gatherWork = new cGatherFunction();

        private ThreadMultiHelper ThreadMulti = null;

        private SpiderHelper Gather = new SpiderHelper();

        #region 委托
        public TaskEventHandler.TaskOpHandler TaskOpDelegate;
        private TaskEvents.TaskOpEvents TaskOpEv = new TaskEvents.TaskOpEvents();
        #endregion

        #region 访问器
        private int _ID = 0;
        /// <summary>
        /// 任务ID
        /// </summary>
        public int ID {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _OldTaskName = string.Empty;
        /// <summary>
        /// 任务名称
        /// </summary>
        public string OldTaskName {
            get { return _OldTaskName; }
            set { _OldTaskName = value; }
        }

        private int _TaskIndex = 0;
        /// <summary>
        /// 任务索引
        /// </summary>
        public int TaskIndex {
            get { return _TaskIndex; }
            set { _TaskIndex = value; }
        }
        #endregion

        public FrmTaskEdit() {

            InitializeComponent();

            this.txtHiddenPlanFormat.Visible = false;
            this.txtHiddenPlanFormat.TextChanged += (object sender, EventArgs e) => {
                this.txtTaskSetFormat.Text = this.txtHiddenPlanFormat.Text;
            };
        }


        #region 绑定插件
        private void Bind_Plugins() {

            PluginUtility.LoadAllDlls();
            List<IPlugin> Plugins = PluginUtility.ListISpiderUrlPlugin;
            foreach (IPlugin item in Plugins) {
                this.cmbSpiderUrlPlugins.Items.Add(item.PluginName);
            }
            this.cmbSpiderUrlPlugins.Items.Insert(0, "不使用插件");
            this.cmbSpiderUrlPlugins.SelectedIndex = 0;
            //
            Plugins = PluginUtility.ListISpiderContentPlugin;
            foreach (IPlugin item in Plugins) {
                this.cmbSpiderContentPlugins.Items.Add(item.PluginName);
            }
            this.cmbSpiderContentPlugins.Items.Insert(0, "不使用插件");
            this.cmbSpiderContentPlugins.SelectedIndex = 0;
            //
            Plugins = PluginUtility.ListISaveContentPlugin;
            foreach (IPlugin item in Plugins) {
                this.cmbSaveConentPlugins.Items.Add(item.PluginName);
            }
            this.cmbSaveConentPlugins.Items.Insert(0, "不使用插件");
            this.cmbSaveConentPlugins.SelectedIndex = 0;
            //
            Plugins = PluginUtility.ListIPublishContentPlugin;
            foreach (IPlugin item in Plugins) {
                this.cmbPublishContentPlugins.Items.Add(item.PluginName);
            }
            this.cmbPublishContentPlugins.Items.Insert(0, "不使用插件");
            this.cmbPublishContentPlugins.SelectedIndex = 0;
        }

        #endregion

        private void FrmTaskEdit_Load(object sender, EventArgs e) {
            Bind_UrlEncode();
            Bind_Plugins();
            Bind_SiteTreeClass();
            if (ID > 0) {
                this.ID = ID;
                this.Bind_DataEdit(ID);
                Bind_TaskLabel(" TaskID=" + ID + " ");
                Bind_WebPublishModule(" TaskID=" + ID + " ");

                Bind_DiyUrlWebList(" And SelfId=" + ID);
            }

            this.contextMenuStrip_Label.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip_Label_ItemClicked);

            this.contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
        }

        void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            string s = "[" + e.ClickedItem.Text + "]";
            //BindLabel_W(this.txtSaveSQLContent4, s);
        }

        void contextMenuStrip_Label_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            string s = "[" + e.ClickedItem.Text + "]";
            BindLabel_W(this.txtSaveDataSQL3, s);

        }

        private void BindLabel_W(V5RichTextBox rtb, string s) {
            int startPos = rtb.SelectionStart;
            int l = rtb.SelectionLength;

            rtb.Text = rtb.Text.Substring(0, startPos) + s + rtb.Text.Substring(startPos + l, rtb.Text.Length - startPos - l);

            rtb.SelectionStart = startPos + s.Length;
            rtb.ScrollToCaret();
        }

        #region 初始化
        /// <summary>
        /// 绑定编码
        /// </summary>
        private void Bind_UrlEncode() {
            var data = cPageEncode.GetPageEnCode(); ;
            //data.Insert(0, new ListItem("Auto", "自动编码"));
            this.ddlItemEncode.DataSource = data;
            this.ddlItemEncode.DisplayMember = "Text";
            this.ddlItemEncode.ValueMember = "Value";
        }

        private void Bind_SiteTreeClass() {
            DALTaskClass dal = new DALTaskClass();
            DataTable dt = dal.GetList("").Tables[0];
            this.cmbSiteClassID.DataSource = dt;
            this.cmbSiteClassID.DisplayMember = "TreeClassName";
            this.cmbSiteClassID.ValueMember = "ClassID";
        }


        #endregion

        #region LinkUrl
        /// <summary>
        /// 向导添加LinkUrl
        /// </summary>
        private void btnWizardEdit_Click(object sender, EventArgs e) {
            frmTaskUrl FormTaskUrl = new frmTaskUrl();
            FormTaskUrl.AddUrl = AddUrl;
            FormTaskUrl.ShowDialog(this);
        }
        /// <summary>
        /// LinkUrl 委托显示
        /// </summary>
        private void AddUrl(object sender, TaskEvents.AddLinkUrlEvents e) {
            //this.txtLinkUrlType.Text = e.LinkType.ToString();
            this.listBoxLinkUrl.Items.Clear();
            foreach (string item in e.LinkObject) {
                this.listBoxLinkUrl.Items.Insert(0, item);
            }
        }
        /// <summary>
        /// LinkUrl 删除
        /// </summary>
        private void txtLinkUrlDelete_Click(object sender, EventArgs e) {
            object item = this.listBoxLinkUrl.SelectedItem;
            if (item != null) {
                this.listBoxLinkUrl.Items.Remove(item);
            }
        }
        /// <summary>
        /// LinkUrl 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLinkUrlEdit_Click(object sender, EventArgs e) {
            object item = this.listBoxLinkUrl.SelectedItem;
            if (item != null) {
                frmTaskUrl FormTaskUrl = new frmTaskUrl();
                FormTaskUrl.AddUrl = AddUrl;
                FormTaskUrl.EditUrl = this.listBoxLinkUrl.Items;
                FormTaskUrl.ShowDialog(this);
            }
        }
        #endregion

        #region 测试内容地址采集
        private BackgroundWorker backgroundWorker_TestLinkUrl = new BackgroundWorker();
        private string TestLinkUrl = string.Empty, includeStr = string.Empty, encode = string.Empty, LinkUrlAreaStart = string.Empty, LinkUrlAreaEnd = string.Empty;
        private delegate void AddTreeNodeUrl(string msg, int nodeIndex);
        private string NoIncludeStr = string.Empty, HandCollectionUrlRegex = string.Empty;
        int IsHandGetUrl = 0;
        List<string> TestListLinkUrl;
        /// <summary>
        /// 测试链接
        /// </summary>
        private void btnLinkUrlTest_Click(object sender, EventArgs e) {
            object listItem = this.listBoxLinkUrl.SelectedItem;
            if (listItem == null) {
                MessageBox.Show("选择一个采集连接!");
                return;
            }
            this.tabControlTaskEdit.SelectTab(4);
            this.treeViewUrlTest.Nodes.Clear();
            object item = this.listBoxLinkUrl.SelectedItem;
            encode = ((ListItem)this.ddlItemEncode.SelectedItem).Value;
            includeStr = this.txtLinkUrlMustIncludeStr.Text;
            NoIncludeStr = this.txtLinkUrlNoMustIncludeStr.Text;
            LinkUrlAreaStart = this.txtLinkUrlCutAreaStart.Text;
            LinkUrlAreaEnd = this.txtLinkUrlCutAreaEnd.Text;
            TestLinkUrl = Convert.ToString(item);
            TestListLinkUrl = gatherWork.SplitWebUrl(TestLinkUrl);
            IsHandGetUrl = this.chkIsHandGetUrl.Checked ? 1 : 0;
            HandCollectionUrlRegex = this.txtHandCollectionUrlRegex.Text;
            foreach (string str in TestListLinkUrl) {
                TreeNode rootNode = new TreeNode();
                rootNode.Text = str;
                this.treeViewUrlTest.Nodes.Add(rootNode);
            }
            backgroundWorker_TestLinkUrl.DoWork += new DoWorkEventHandler(backgroundWorker_TestLinkUrl_DoWork);
            backgroundWorker_TestLinkUrl.WorkerReportsProgress = true;
            backgroundWorker_TestLinkUrl.WorkerSupportsCancellation = true;
            if (!this.backgroundWorker_TestLinkUrl.IsBusy) {
                this.backgroundWorker_TestLinkUrl.RunWorkerAsync();
            }
        }

        private void TestLinkUrlTest(string testUrl, int num) {
            string pageContent = CollectionHelper.Instance.GetHttpPage(testUrl, 100000, Encoding.GetEncoding(encode));
            if (LinkUrlAreaStart.Trim() != "" && LinkUrlAreaEnd.Trim() != "") {
                pageContent = HtmlHelper.Instance.ParseCollectionStrings(pageContent);
                pageContent = CollectionHelper.Instance.GetBody(pageContent,
                    HtmlHelper.Instance.ParseCollectionStrings(LinkUrlAreaStart),
                    HtmlHelper.Instance.ParseCollectionStrings(LinkUrlAreaEnd),
                    false,
                    false);
                if (pageContent == "$StartFalse$" || pageContent == "$EndFalse$") {
                    MessageBox.Show("采集失败!");
                    backgroundWorker_TestLinkUrl.CancelAsync();
                    return;
                }
                pageContent = HtmlHelper.Instance.UnParseCollectionStrings(pageContent);
            }
            string regexHref = cRegexHelper.RegexATag;
            if (IsHandGetUrl == 1) {
                regexHref = HandCollectionUrlRegex;
                regexHref = HtmlHelper.Instance.ParseCollectionStrings(regexHref);
                regexHref = regexHref.Replace("\\(\\*)", ".+?");
                regexHref = regexHref.Replace("\\[参数]", "([\\S\\s].*?)");
            }

            Match mch = null;
            Regex reg = new Regex(regexHref, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            AddTreeNodeUrl AddUrl = new AddTreeNodeUrl(AddNodeUrl);
            string url = string.Empty;
            string strUrl = string.Empty;
            for (mch = reg.Match(pageContent); mch.Success; mch = mch.NextMatch()) {
                url = CollectionHelper.Instance.FormatUrl(testUrl, mch.Groups[1].Value);
                url = url.Replace("\\", "");
                if (includeStr.Trim() != "") {
                    if (url.IndexOf(includeStr) == -1) {
                        continue;
                    }
                }
                if (NoIncludeStr.Trim() != "") {
                    bool isFlag = true;
                    foreach (string str in NoIncludeStr.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)) {
                        if (url.IndexOf(str) > -1) {
                            isFlag = false;
                            break;
                        }
                    }
                    if (isFlag)
                        AddUrl(url, num);
                }
                else {
                    AddUrl(url, num);
                }
            }
        }
        /// <summary>
        /// 测试LinkUrl线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_TestLinkUrl_DoWork(object sender, DoWorkEventArgs e) {
            TestListLinkUrl = gatherWork.SplitWebUrl(TestLinkUrl);
            int i = 0;
            foreach (string str in TestListLinkUrl) {
                TestLinkUrlTest(str, i);
                i++;
            }
        }
        /// <summary>
        /// 委托显示LinkUrl
        /// </summary>
        /// <param name="msg"></param>
        private void AddNodeUrl(string msg, int nodeIndex) {
            this.BeginInvoke(new MethodInvoker(delegate () {
                if (!this.treeViewUrlTest.Nodes[nodeIndex].Nodes.ContainsKey(msg))
                    this.treeViewUrlTest.Nodes[nodeIndex].Nodes.Add(msg, msg);
            }));
        }
        #endregion

        #region 测试ViewUrl
        private delegate void ViewUrlHandler(string msg);
        private string Test_ViewUrl = string.Empty;
        private ModelTaskLabel TestLabelModel = new ModelTaskLabel();
        private List<ListViewItem> Test_LabelList = new List<ListViewItem>();

        private void Work_TestViewUrl(int index, int threadindex) {
            ViewUrlHandler VU = new ViewUrlHandler(OutViewUrlTest);
            try {
                #region  采集测试
                String sContent = String.Empty;
                DALTaskLabel dal = new DALTaskLabel();
                string pageContent = CollectionHelper.Instance.GetHttpPage(Test_ViewUrl, 100000);
                if (pageContent.Equals("$UrlIsFalse$") || pageContent.Equals("$GetFalse$")) {
                    if (VU != null) {
                        VU("采集地址不正确!或者采集内容失败!");
                    }
                    return;
                }
                StringBuilder sbTest = new StringBuilder();

                foreach (ListViewItem item in Test_LabelList) {
                    sContent = string.Empty;
                    TestLabelModel = dal.GetModel(item.SubItems[0].Text, ID);
                    sContent += "【" + TestLabelModel.LabelName + "】： ";
                    string regContent = HtmlHelper.Instance.ParseCollectionStrings(TestLabelModel.LabelNameCutRegex);
                    regContent = CommonHelper.ReplaceSystemRegexTag(regContent);
                    string CutContent = CollectionHelper.Instance.CutStr(pageContent, regContent)[0];
                    #region 标签是循环
                    if (TestLabelModel.IsLoop == 1) {
                        string[] LabelString = CollectionHelper.Instance.CutStr(pageContent, regContent);
                        foreach (string s in LabelString) {
                            CutContent += s + "$$$$";
                        }
                        int n = CutContent.LastIndexOf("$$$$");
                        CutContent = CutContent.Remove(n, 4);
                    }
                    #endregion
                    #region 标签是链接
                    if (TestLabelModel.IsLinkUrl == 1) {
                        CutContent = CollectionHelper.Instance.DefiniteUrl(CutContent, Test_ViewUrl);
                        CutContent = CollectionHelper.Instance.GetHttpPage(CutContent, 1000, Encoding.GetEncoding(encode));
                        regContent = HtmlHelper.Instance.ParseCollectionStrings(TestLabelModel.LabelValueLinkUrlRegex);
                        //
                        regContent = regContent.Replace("\\(\\*)", ".+?");
                        regContent = regContent.Replace("\\[参数]", "([\\S\\s].*?)");
                        CutContent = CollectionHelper.Instance.CutStr(CutContent, regContent)[0];
                        //
                    }
                    #endregion
                    #region 标签是分页
                    if (TestLabelModel.IsPager == 1) {
                        regContent = HtmlHelper.Instance.ParseCollectionStrings(TestLabelModel.LabelValuePagerRegex);
                        regContent = regContent.Replace("\\(\\*)", ".+?");
                        regContent = regContent.Replace("\\[参数]", "([\\S\\s].*?)");
                        string[] LabelString = CollectionHelper.Instance.CutStr(pageContent, regContent);

                        foreach (string pageUrl in LabelString) {
                            string url = CollectionHelper.Instance.DefiniteUrl(pageUrl, Test_ViewUrl);
                            string pageContentPager = CollectionHelper.Instance.GetHttpPage(url, 100000);
                            if (pageContent.Equals("$UrlIsFalse$") || pageContent.Equals("$GetFalse$")) {

                                CutContent += "=====分页内容=======================================================\r\n";
                                CutContent += "远程链接内容失败!";
                            }
                            else {
                                //重新截取标签
                                string regContent1 = HtmlHelper.Instance.ParseCollectionStrings(TestLabelModel.LabelNameCutRegex);
                                regContent1 = CommonHelper.ReplaceSystemRegexTag(regContent1);
                                string CutContent1 = CollectionHelper.Instance.CutStr(pageContentPager, regContent1)[0];

                                CutContent += "=====分页内容=======================================================\r\n";
                                CutContent += CutContent1;
                            }
                        }
                    }
                    #endregion
                    #region 过滤Html
                    if (TestLabelModel.LabelHtmlRemove != null) {
                        //
                        string[] arr = TestLabelModel.LabelHtmlRemove.Split(new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string str in arr) {
                            if (str == "all") {
                                //替换标准标签包含 图片 视频  文档 压缩文件 什么的
                                CutContent = HtmlHelper.ReplaceNormalHtml(CutContent, Test_ViewUrl, false);
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
                            else if (str == "script") {
                                CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "script", 3);
                            }
                            else if (str == "a") {
                                CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "a", 3);
                            }
                        }
                    }
                    #endregion
                    #region 排除字符
                    if (TestLabelModel.LabelRemove != null) {
                        foreach (string str in TestLabelModel.LabelRemove.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                            CutContent = CutContent.Replace(str, "");
                        }
                    }
                    #endregion
                    #region 替换字符
                    if (TestLabelModel.LabelReplace != null) {
                        foreach (string str in TestLabelModel.LabelReplace.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                            string[] ListStr = str.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                            CutContent = CutContent.Replace(ListStr[0], ListStr[1]);
                        }
                    }
                    #endregion

                    CutContent = CutContent.Replace("\t", "");

                    //加载插件

                    string SpiderLabelPlugin = TestLabelModel.SpiderLabelPlugin;
                    if (SpiderLabelPlugin != "不使用插件" && SpiderLabelPlugin != string.Empty) {
                        CutContent = PythonExtHelper.RunPython(SpiderLabelPlugin, Test_ViewUrl, CutContent);
                    }

                    sContent += CutContent;
                    sbTest.AppendLine(sContent);
                }

                if (VU != null) {
                    VU(sbTest.ToString());
                }
                //VU = null;
                #endregion
            }
            catch (Exception ex) {
                OutViewUrlTest("测试网页采集失败!" + ex.Message);
            }
        }

        /// <summary>
        /// 测试采集内容
        /// </summary>
        private void btnTestViewUrl_Click(object sender, EventArgs e) {
            this.btnTestViewUrl.Enabled = false;
            this.txtTestViewUrlShow.Clear();
            Test_ViewUrl = this.txtTextViewUrl.Text;
            encode = ((ListItem)this.ddlItemEncode.SelectedItem).Value;
            ListView.ListViewItemCollection s = this.listViewTaskLabel.Items;
            foreach (ListViewItem ss in s) {
                if (!Test_LabelList.Contains(ss)) {
                    Test_LabelList.Add(ss);
                }
            }
            if (ThreadMulti == null) {
                ThreadMulti = new ThreadMultiHelper(1, 1);
                ThreadMulti.WorkMethod += Work_TestViewUrl;
                ThreadMulti.CompleteEvent += new ThreadMultiHelper.DelegateComplete(delegate () {
                    this.btnTestViewUrl.Invoke(new MethodInvoker(() => { this.btnTestViewUrl.Enabled = true; }));
                });
            }
            ThreadMulti.Start();
        }

        private void OutViewUrlTest(string msg) {
            this.txtTestViewUrlShow.Invoke(new MethodInvoker(delegate () {
                this.txtTestViewUrlShow.AppendText(msg);
                this.txtTestViewUrlShow.AppendText("\r\n");
            }));
        }
        #endregion

        #region 标签操作
        /// <summary>
        /// 加载任务标签
        /// </summary>
        private void Bind_TaskLabel(string strWhere) {
            this.listViewTaskLabel.Items.Clear();
            DALTaskLabel dal = new DALTaskLabel();
            DataTable dt = dal.GetList(strWhere + " Order by OrderID Asc").Tables[0];
            foreach (DataRow dr in dt.Rows) {
                ListViewItem li = new ListViewItem(dr["LabelName"].ToString());
                li.SubItems.Add(dr["LabelNameCutRegex"].ToString());
                li.Tag = dr["ID"].ToString();
                this.listViewTaskLabel.Items.Add(li);
            }
        }
        /// <summary>
        /// 添加标签
        /// </summary>
        private void btnCutLabelAdd_Click(object sender, EventArgs e) {
            frmTaskSpiderLabel FormTaskSpiderLabel = new frmTaskSpiderLabel();
            FormTaskSpiderLabel.ViewLabel = AddViewLabel;
            FormTaskSpiderLabel.TaskID = ID;
            FormTaskSpiderLabel.ShowDialog(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddViewLabel(object sender, TaskEvents.AddViewLabelEvents e) {
            Bind_TaskLabel(" TaskID=" + this.ID + " ");
        }
        /// <summary>
        /// 编辑标签
        /// </summary>
        private void btnCutLabelEdit_Click(object sender, EventArgs e) {
            var sel = this.listViewTaskLabel.SelectedItems;
            if (sel != null && sel.Count == 1) {
                frmTaskSpiderLabel FormTaskSpiderLabel = new frmTaskSpiderLabel();
                FormTaskSpiderLabel.EditItem = this.listViewTaskLabel.SelectedItems;
                FormTaskSpiderLabel.ViewLabel = AddViewLabel;
                FormTaskSpiderLabel.TaskID = ID;
                FormTaskSpiderLabel.ShowDialog(this);
            }
        }
        /// <summary>
        /// 删除标签
        /// </summary>
        private void btnCutLabelDel_Click(object sender, EventArgs e) {
            var sel = this.listViewTaskLabel.SelectedItems;
            if (sel != null && sel.Count == 1) {
                if (MessageBox.Show("你确定要删除吗?删除不可恢复!", "警告!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                    ListView.SelectedListViewItemCollection item = this.listViewTaskLabel.SelectedItems;
                    DALTaskLabel dal = new DALTaskLabel();
                    dal.Delete(int.Parse("0" + item[0].Tag));
                    this.Bind_TaskLabel(" TaskID=" + this.ID);
                }
            }
        }
        /// <summary>
        /// 标签上升
        /// </summary>
        private void btnLabelUp_Click(object sender, EventArgs e) {
            var sel = this.listViewTaskLabel.SelectedItems;
            if (sel != null && sel.Count == 1) {
                ListView.SelectedListViewItemCollection item = this.listViewTaskLabel.SelectedItems;
                int iID = int.Parse("0" + item[0].Tag);
                DALTaskLabel dal = new DALTaskLabel();
                dal.UpdateOrder(this.ID, iID, -1);
                this.Bind_TaskLabel(" TaskID=" + this.ID);
            }
        }
        /// <summary>
        /// 标签下降
        /// </summary>
        private void btnLabelDown_Click(object sender, EventArgs e) {
            var sel = this.listViewTaskLabel.SelectedItems;
            if (sel != null && sel.Count == 1) {
                ListView.SelectedListViewItemCollection item = this.listViewTaskLabel.SelectedItems;
                int iID = int.Parse("0" + item[0].Tag);
                DALTaskLabel dal = new DALTaskLabel();
                dal.UpdateOrder(this.ID, iID, 1);
                this.Bind_TaskLabel(" TaskID=" + this.ID);
            }
        }
        #endregion

        #region 本地保存

        private OpenFileDialog LocalFileDialog = new OpenFileDialog();
        private FolderBrowserDialog PublishDialog = new FolderBrowserDialog();
        private void btnSaveLocalPath_Click(object sender, EventArgs e) {
            if (PublishDialog.ShowDialog(this) == DialogResult.OK) {
                this.txtSaveDirectory2.Text = PublishDialog.SelectedPath;
            }
            //if (LocalFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            //{
            //    LocalFileDialog.l
            //    LocalFileDialog.OpenFile();
            //    string fileName = this.LocalFileDialog.FileName;
            //    string fileDir = System.IO.Path.GetDirectoryName(fileName);
            //    this.txtSaveLocalPath.Text = fileDir;
            //}
        }

        private void btnSaveLocalHtmlTemplatePath_Click(object sender, EventArgs e) {
            if (LocalFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                LocalFileDialog.OpenFile();
                string fileName = this.LocalFileDialog.FileName;
                this.txtSaveHtmlTemplate2.Text = fileName;
            }
        }

        private void ddlSaveLocalFileFormat_SelectedIndexChanged(object sender, EventArgs e) {
            string format = this.ddlSaveFileFormat2.Text;
            if (format == ".Html") {
                txtSaveHtmlTemplate2.Enabled = true;
                btnSaveLocalHtmlTemplatePath.Enabled = true;
            }
            else if (format == ".Sql") {
                txtSaveHtmlTemplate2.Enabled = true;
                this.btnSaveLocalHtmlTemplatePath.Enabled = true;
            }
            else {
                txtSaveHtmlTemplate2.Enabled = false;
                btnSaveLocalHtmlTemplatePath.Enabled = false;
            }
        }
        #endregion

        #region 保存插入SQL语句
        private void btnSaveSqlPath_Click(object sender, EventArgs e) {
            if (PublishDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                //PublishDialog.OpenFile();
                string fileName = PublishDialog.SelectedPath;
                //this.txtSaveSQLDirectory4.Text = fileName;
            }
        }
        #endregion

        #region Sql
        int dbType = 0;
        private void btnSaveDataBaseConfig_Click(object sender, EventArgs e) {
            if (this.rbtnAccess.Checked) {
                frmDataBaseConfigAccess formDataBaseConfigAccess = new frmDataBaseConfigAccess();
                formDataBaseConfigAccess.OutDBConfig = OutDbConfigUrl;
                formDataBaseConfigAccess.ShowDialog();
            }
            else if (this.rbtnMsSql.Checked) {
                frmDataBaseConfigSqlServer formDataBaseConfigSqlServer = new frmDataBaseConfigSqlServer();
                formDataBaseConfigSqlServer.OutDBConfig = OutDbConfigUrl;
                formDataBaseConfigSqlServer.ShowDialog();
            }
            else if (this.rbtnSqlite.Checked) {
                frmDataBaseConfigSQLite formDataBaseConfigSQLite = new frmDataBaseConfigSQLite();
                formDataBaseConfigSQLite.OutDBConfig = OutDbConfigUrl;
                formDataBaseConfigSQLite.ShowDialog();
            }
            else if (this.rbtnMySql.Checked) {
                frmDataBaseConfigMySql formDataBaseConfigMySql = new frmDataBaseConfigMySql();
                formDataBaseConfigMySql.OutDBConfig = OutDbConfigUrl;
                formDataBaseConfigMySql.ShowDialog();
            }
            else if (this.rbtnOracle.Checked) {
                frmDataBaseConfigOracle formDataBaseConfigOracle = new frmDataBaseConfigOracle();
                formDataBaseConfigOracle.OutDBConfig = OutDbConfigUrl;
                formDataBaseConfigOracle.ShowDialog();
            }
        }

        private void OutDbConfigUrl(string strDbUrl) {
            this.txtSaveDataUrl3.Invoke(new MethodInvoker(delegate () {
                this.txtSaveDataUrl3.Text = strDbUrl;
            }));

        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Hide();
            this.Close();
        }

        #region 编辑数据
        private void Bind_DataEdit(int ID) {

            this.Text = "编辑任务";

            ModelTask model = new DALTask().GetModel(ID);
            //
            this.txtID.Text = model.ID.ToString();
            this.cmbSiteClassID.SelectedValue = model.TaskClassID;
            this.txtTaskName.Text = model.TaskName;
            this.txtOldTaskName.Text = model.TaskName;
            this.chkIsSpiderUrl.Checked = model.IsSpiderUrl.ToString() == "1" ? true : false;
            this.chkIsSpiderContent.Checked = model.IsSpiderContent.ToString() == "1" ? true : false;
            this.chkIsPublishContent.Checked = model.IsPublishContent.ToString() == "1" ? true : false;
            ListItem item = new ListItem(model.PageEncode, model.PageEncode);
            this.ddlItemEncode.SelectedItem = item;
            if (!string.IsNullOrEmpty(model.CollectionContent)) {
                foreach (string str in model.CollectionContent.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                    this.listBoxLinkUrl.Items.Add(str);
                }
            }
            this.txtLinkUrlMustIncludeStr.Text = model.LinkUrlMustIncludeStr;
            this.txtLinkUrlNoMustIncludeStr.Text = model.LinkUrlNoMustIncludeStr;
            this.txtLinkUrlCutAreaStart.Text = model.LinkUrlCutAreaStart;
            this.txtLinkUrlCutAreaEnd.Text = model.LinkUrlCutAreaEnd;
            //
            this.txtTextViewUrl.Text = model.TestViewUrl;
            //
            this.chkPublish01.Checked = model.IsWebOnlinePublish1.ToString() == "1" ? true : false;
            this.chkPublish02.Checked = model.IsSaveLocal2.ToString() == "1" ? true : false;
            this.ddlSaveFileFormat2.Text = model.SaveFileFormat2;
            this.txtSaveDirectory2.Text = model.SaveDirectory2;
            this.txtSaveHtmlTemplate2.Text = model.SaveHtmlTemplate2;
            //this.chkSaveIsCreateIndex2.Checked = model.SaveIsCreateIndex2.ToString() == "1" ? true : false;
            this.chkPublish03.Checked = model.IsSaveDataBase3.ToString() == "1" ? true : false;
            string SaveDataType3 = model.SaveDataType3.ToString();
            switch (SaveDataType3) {
                case "1":
                    this.rbtnAccess.Checked = true;
                    break;
                case "2":
                    this.rbtnMsSql.Checked = true;
                    break;
                case "3":
                    this.rbtnSqlite.Checked = true;
                    break;
                case "4":
                    this.rbtnMySql.Checked = true;
                    break;
                case "5":
                    this.rbtnOracle.Checked = true;
                    break;
            }
            this.txtSaveDataUrl3.Text = model.SaveDataUrl3;
            this.txtSaveDataSQL3.Text = model.SaveDataSQL3;
            this.chkPublish04.Checked = model.IsSaveSQL4.ToString() == "1" ? true : false;
            //this.txtSaveSQLContent4.Text = model.SaveSQLContent4;
            //this.txtSaveSQLDirectory4.Text = model.SaveSQLDirectory4;
            //
            this.cmbSpiderUrlPlugins.Text = model.PluginSpiderUrl;
            this.cmbSpiderContentPlugins.Text = model.PluginSpiderContent;
            this.cmbSaveConentPlugins.Text = model.PluginSaveContent;
            this.cmbPublishContentPlugins.Text = model.PluginPublishContent;

            //
            this.nudCollectionContentThreadCount.Value = model.CollectionContentThreadCount;
            this.nudCollectionContentStepTime.Value = model.CollectionContentStepTime;
            this.nudPublishContentThreadCount.Value = model.PublishContentThreadCount;
            this.nudPublishContentStepTimeMin.Value = model.PublishContentStepTimeMin;
            this.nudPublishContentStepTimeMax.Value = model.PublishContentStepTimeMax;

            this.chkIsHandGetUrl.Checked = model.IsHandGetUrl == 1 ? true : false;
            this.txtHandCollectionUrlRegex.Text = model.HandCollectionUrlRegex;

            this.txtDemoListUrl.Text = model.DemoListUrl;

            //
            this.chkTaskSetStatus.Checked = model.IsPlan == 1 ? true : false;
            this.txtHiddenPlanFormat.Text = model.PlanFormat;

        }
        #endregion

        #region 创建任务
        private void btnSubmit_Click(object sender, EventArgs e) {
            DALTask dal = new DALTask();
            ModelTask model = new ModelTask();

            errorProvider.Clear();
            if (string.IsNullOrEmpty(this.cmbSiteClassID.Text)) {
                errorProvider.SetError(this.cmbSiteClassID, "所属站点不能为空!");
                return;
            }

            if (string.IsNullOrEmpty(this.txtTaskName.Text)) {
                errorProvider.SetError(this.txtTaskName, "任务名称不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(this.txtDemoListUrl.Text)) {
                errorProvider.SetError(this.txtDemoListUrl, "测试网站列表地址不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(this.ddlItemEncode.Text)) {
                errorProvider.SetError(this.ddlItemEncode, "网页编码不能为空!");
                return;
            }
            int ID = int.Parse(this.txtID.Text);
            //基本操作
            int TaskClassID = int.Parse(this.cmbSiteClassID.SelectedValue.ToString());
            string TaskName = this.txtTaskName.Text;
            string OldTaskName = this.txtOldTaskName.Text;
            int IsSpiderUrl = this.chkIsSpiderUrl.Checked ? 1 : 0;
            int IsSpiderContent = this.chkIsSpiderContent.Checked ? 1 : 0;
            int IsPublishContent = this.chkIsPublishContent.Checked ? 1 : 0;
            string PageEncode = ((ListItem)this.ddlItemEncode.SelectedItem).Value;
            string CollectionContent = string.Empty;
            foreach (string str in listBoxLinkUrl.Items) {
                CollectionContent += str + "$$$$";
            }
            string LinkUrlMustIncludeStr = this.txtLinkUrlMustIncludeStr.Text;
            string LinkUrlNoMustIncludeStr = this.txtLinkUrlNoMustIncludeStr.Text;
            string LinkUrlCutAreaStart = this.txtLinkUrlCutAreaStart.Text.Replace("'", "''");
            string LinkUrlCutAreaEnd = this.txtLinkUrlCutAreaEnd.Text.Replace("'", "''");
            //标签操作
            string TestViewUrl = this.txtTextViewUrl.Text;
            //发布
            int IsWebOnlinePublish1 = this.chkPublish01.Checked ? 1 : 0;
            int IsSaveLocal2 = this.chkPublish02.Checked ? 1 : 0;
            string SaveFileFormat2 = this.ddlSaveFileFormat2.Text;
            string SaveDirectory2 = this.txtSaveDirectory2.Text;
            string SaveHtmlTemplate2 = this.txtSaveHtmlTemplate2.Text;
            //int SaveIsCreateIndex2 = this.chkSaveIsCreateIndex2.Checked ? 1 : 0;
            int IsSaveDataBase3 = this.chkPublish03.Checked ? 1 : 0;
            int SaveDataType3 = 0;
            if (this.rbtnAccess.Checked) {
                SaveDataType3 = 1;
            }
            else if (this.rbtnMsSql.Checked) {
                SaveDataType3 = 2;
            }
            else if (this.rbtnSqlite.Checked) {
                SaveDataType3 = 3;
            }
            else if (this.rbtnMySql.Checked) {
                SaveDataType3 = 4;
            }
            else if (this.rbtnSqlite.Checked) {
                SaveDataType3 = 5;
            }
            string SaveDataUrl3 = this.txtSaveDataUrl3.Text;
            string SaveDataSQL3 = this.txtSaveDataSQL3.Text.Replace("'", "''");
            int IsSaveSQL4 = this.chkPublish04.Checked ? 1 : 0;
            //string SaveSQLContent4 = this.txtSaveSQLContent4.Text.Replace("'", "''");
            //string SaveSQLDirectory4 = this.txtSaveSQLDirectory4.Text;

            decimal CollectionContentThreadCount = this.nudCollectionContentThreadCount.Value;
            decimal CollectionContentStepTime = this.nudCollectionContentStepTime.Value;
            decimal PublishContentThreadCount = this.nudPublishContentThreadCount.Value;
            decimal PublishContentStepTimeMin = this.nudPublishContentStepTimeMin.Value;
            decimal PublishContentStepTimeMax = this.nudPublishContentStepTimeMax.Value;

            int IsHandGetUrl = this.chkIsHandGetUrl.Checked ? 1 : 0;
            string HandCollectionUrlRegex = this.txtHandCollectionUrlRegex.Text.Replace("'", "''");
            //
            int IsPlan = this.chkTaskSetStatus.Checked ? 1 : 0;
            string PlanFormat = this.txtHiddenPlanFormat.Text;
            //
            model.ID = ID;
            model.TaskClassID = TaskClassID;
            model.TaskName = TaskName;
            model.IsSpiderUrl = IsSpiderUrl;
            model.IsSpiderContent = IsSpiderContent;
            model.IsPublishContent = IsPublishContent;
            model.PageEncode = PageEncode;
            //model.CollectionType = CollectionType;
            model.CollectionContent = CollectionContent;
            model.LinkUrlMustIncludeStr = LinkUrlMustIncludeStr;
            model.LinkUrlNoMustIncludeStr = LinkUrlNoMustIncludeStr;
            model.LinkUrlCutAreaStart = LinkUrlCutAreaStart;
            model.LinkUrlCutAreaEnd = LinkUrlCutAreaEnd;
            model.TestViewUrl = TestViewUrl;
            model.IsWebOnlinePublish1 = IsWebOnlinePublish1;
            model.IsSaveLocal2 = IsSaveLocal2;
            model.SaveFileFormat2 = SaveFileFormat2;
            model.SaveDirectory2 = SaveDirectory2;
            model.SaveHtmlTemplate2 = SaveHtmlTemplate2;
            //model.SaveIsCreateIndex2 = SaveIsCreateIndex2;
            model.IsSaveDataBase3 = IsSaveDataBase3;
            model.SaveDataType3 = SaveDataType3;
            model.SaveDataUrl3 = SaveDataUrl3;
            model.SaveDataSQL3 = SaveDataSQL3;
            model.IsSaveSQL4 = IsSaveSQL4;
            //model.SaveSQLContent4 = SaveSQLContent4;
            //model.SaveSQLDirectory4 = SaveSQLDirectory4;
            //
            model.PluginSpiderUrl = this.cmbSpiderUrlPlugins.Text;
            model.PluginSpiderContent = this.cmbSpiderContentPlugins.Text;
            model.PluginSaveContent = this.cmbSaveConentPlugins.Text;
            model.PluginPublishContent = this.cmbPublishContentPlugins.Text;
            //2012 2-16
            model.CollectionContentThreadCount = Convert.ToInt32(CollectionContentThreadCount);
            model.CollectionContentStepTime = Convert.ToInt32(CollectionContentStepTime);
            model.PublishContentThreadCount = Convert.ToInt32(PublishContentThreadCount);
            model.PublishContentStepTimeMin = Convert.ToInt32(PublishContentStepTimeMin);
            model.PublishContentStepTimeMax = Convert.ToInt32(PublishContentStepTimeMax);

            model.IsHandGetUrl = IsHandGetUrl;
            model.HandCollectionUrlRegex = HandCollectionUrlRegex;

            model.DemoListUrl = this.txtDemoListUrl.Text;
            //
            model.IsPlan = IsPlan;
            model.PlanFormat = PlanFormat;
            if (ID == 0) {
                string guid = Guid.NewGuid().ToString();
                ID = dal.GetMaxId();
                model.ID = ID;
                model.Guid = guid;
                dal.Add(model);
                DALTaskLabel dalTaskLabel = new DALTaskLabel();
                dalTaskLabel.UpdateTaskLabelByTaskID(ID);
                if (TaskOpDelegate != null) {
                    TaskOpEv.TaskIndex = TaskIndex;
                    TaskOpDelegate(this, TaskOpEv);
                }
            }
            else if (ID > 0) {
                if (TaskName != OldTaskName) {
                    string RootPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\";
                    string OldTaskPath = RootPath + OldTaskName;
                    string NewTaskPath = RootPath + TaskName;
                    if (Directory.Exists(NewTaskPath)) {
                        MessageBox.Show("任务名称存在!请换个名称试试!");
                        return;
                    }
                    try {
                        Directory.Move(OldTaskPath, NewTaskPath);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("修改异常!" + ex.Message);
                        return;
                    }
                }
                if (!dal.CheckTaskGuid(ID)) {
                    model.Guid = Guid.NewGuid().ToString();
                }
                dal.Update(model);
                if (TaskOpDelegate != null) {
                    TaskOpEv.TaskIndex = TaskIndex;
                    TaskOpEv.OpType = 1;
                    TaskOpDelegate(this, TaskOpEv);
                }
            }
            CreateDataFile(TaskName, ID);

            #region 更新计划任务
            if (model.IsPlan == 1) {
                PlanTaskHelper.PushJobDetail(ID);
            }
            #endregion
            this.Hide();
            this.Close();
        }
        #endregion

        #region 创建文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="taskName"></param>
        private void CreateDataFile(string taskName, int taskID) {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\";
            string SQLiteName = baseDir + taskName + "\\SpiderResult.db";
            string LocalSQLiteName = "Data\\Collection\\" + taskName + "\\SpiderResult.db";
            string SQL = string.Empty;
            if (!Directory.Exists(baseDir + taskName + "\\")) {
                Directory.CreateDirectory(baseDir + taskName + "\\");
            }
            if (!File.Exists(SQLiteName)) {
                SQLiteHelper.CreateDataBase(SQLiteName);
                DALTask dal = new DALTask();
                string createSQL = string.Empty;
                DataTable dt = new DALTaskLabel().GetList(" TaskID=" + taskID).Tables[0];
                foreach (DataRow dr in dt.Rows) {
                    createSQL += @"
                     [" + dr["LabelName"] + @"] varchar,";
                }
                if (createSQL.Trim() != "") {
                    createSQL = createSQL.Remove(createSQL.Length - 1);
                }
                /* 修改表
                 BEGIN TRANSACTION;   
                    CREATE TEMPORARY TABLE temp_table(a);
                    INSERT INTO temp_table SELECT a FROM 表;   
                    DROP TABLE 表;
                    CREATE TABLE 表(a);   
                    INSERT INTO 表 SELECT a FROM temp_table;   
                    DROP TABLE temp_table;
                    COMMIT;
                 */
                //select * from sqlite_master  where tbl_name='Content'
                string ss = string.Empty;
                if (!string.IsNullOrEmpty(createSQL)) {
                    ss = ",";
                }
                SQL = @"
                create table Content(
                    ID integer primary key autoincrement,
                    [HrefSource] varchar,
                    [已采] int,
                    [已发] int" + ss + @"
                    " + createSQL + @"
                );
            ";
                SQLiteHelper.Execute(LocalSQLiteName, SQL);
            }
            else {
                //添加Sqlite列名称
                DataTable dt = new DALTaskLabel().GetList(" TaskID=" + taskID).Tables[0];
                foreach (DataRow dr in dt.Rows) {
                    try {
                        SQLiteHelper.Execute("LocalSQLiteName", " ALTER TABLE Content ADD COLUMN [" + dr["LabelName"] + "] VARCHAR; ");
                    }
                    catch {
                        continue;
                    }
                }

            }
        }
        #endregion

        #region 网站发布模块
        private void btnWebModuleAdd_Click(object sender, EventArgs e) {
            frmPublishEdit PublishEdit = new frmPublishEdit();
            PublishEdit.TaskID = this.ID;
            PublishEdit.ECEH = WebPublishModuleComplate;
            PublishEdit.Show(this);
        }

        private void btnWebModuleEdit_Click(object sender, EventArgs e) {
            frmPublishEdit PublishEdit = new frmPublishEdit();
            PublishEdit.ECEH = WebPublishModuleComplate;
            PublishEdit.TaskID = this.ID;
            PublishEdit.EditItem = this.listViewWebModule.SelectedItems;
            PublishEdit.Show(this);
        }

        private void WebPublishModuleComplate(int id, string msg) {
            Bind_WebPublishModule(" TaskID=" + this.ID + " ");
        }

        private void btnWebModuleDelete_Click(object sender, EventArgs e) {
            ListView.SelectedListViewItemCollection item = this.listViewWebModule.SelectedItems;
            DALWebPublishModule dal = new DALWebPublishModule();
            dal.Delete(int.Parse("0" + item[0].Tag));
            Bind_WebPublishModule(" TaskID=" + this.ID + " ");
        }

        private void Bind_WebPublishModule(string strWhere) {
            this.listViewWebModule.Items.Clear();
            DALWebPublishModule dal = new DALWebPublishModule();
            DataTable dt = dal.GetList(strWhere).Tables[0];
            foreach (DataRow dr in dt.Rows) {
                ListViewItem li = new ListViewItem(dr["ModuleName"].ToString());
                li.SubItems.Add(dr["ClassID"].ToString());
                li.SubItems.Add(dr["ClassName"].ToString());
                li.Tag = dr["ID"].ToString();
                this.listViewWebModule.Items.Add(li);
            }
        }
        #endregion

        private void IsHandGetUrl_CheckedChanged(object sender, EventArgs e) {
            if (this.chkIsHandGetUrl.Checked) {
                this.txtHandCollectionUrlRegex.Enabled = true;
            }
            else {
                this.txtHandCollectionUrlRegex.Enabled = false;
            }
        }

        Form WebBrowser;

        private void btnGetCookies_Click(object sender, EventArgs e) {
            WebBrowser = AppRunHelper.AppRunWebBrowserByAssembly(this);
            AppRunHelper.OutPutMessage = OutPutMessage;
        }

        void OutPutMessage(object sender, AppRunHelper.AppRunEventArgs e) {
            this.txtCollectionCookies.Text = e.Msg1;

            AppRunHelper.CloseWindow(WebBrowser, true);
        }

        void Proc_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e) {
            MessageBox.Show("写入值了" + e.Data);
        }

        private void btnTaskLabelCopy_Click(object sender, EventArgs e) {
            var sel = this.listViewTaskLabel.SelectedItems;
            if (sel != null && sel.Count > 0) {
                ListView.SelectedListViewItemCollection item = this.listViewTaskLabel.SelectedItems;
                int ID = int.Parse("0" + item[0].Tag.ToString());
                DALTaskLabel dal = new DALTaskLabel();
                dal.TaskLabelCopy(ID);
                this.Bind_TaskLabel(" TaskID=" + this.ID);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            try {


                //实例化一个进程类
                Process cmd = new Process();

                //获得系统信息，使用的是 systeminfo.exe 这个控制台程序
                cmd.StartInfo.FileName = "V5.DataWebBrowser.exe";

                //将cmd的标准输入和输出全部重定向到.NET的程序里

                cmd.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常

                cmd.StartInfo.RedirectStandardInput = true; //标准输入
                cmd.StartInfo.RedirectStandardOutput = true; //标准输出

                //不显示命令行窗口界面
                cmd.StartInfo.CreateNoWindow = true;
                // cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                cmd.Start(); //启动进程

                //获取输出
                //需要说明的：此处是指明开始获取，要获取的内容，
                //只有等进程退出后才能真正拿到
                this.txtCollectionCookies.Text = cmd.StandardOutput.ReadToEnd();
                cmd.WaitForExit();//等待控制台程序执行完成
                cmd.Close();//关闭该进程

                ////声明一个程序信息类 
                //System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
                ////设置外部程序名 
                //Info.FileName = "V5.DataWebBrowser.exe";
                ////设置外部程序的启动参数（命令行参数）为test.txt 
                //Info.Arguments = "controlname,type";
                ////设置外部程序工作目录为 C:\ 
                //Info.WorkingDirectory = "";
                //Info.UseShellExecute = false;
                //Info.RedirectStandardOutput = true;
                //Info.RedirectStandardError = true;
                //Info.CreateNoWindow = true;
                //Info.WindowStyle = ProcessWindowStyle.Hidden;
                ////声明一个程序类 
                //System.Diagnostics.Process Proc;
                //try {
                //    // 
                //    //启动外部程序 
                //    // 
                //    Proc = System.Diagnostics.Process.Start(Info);
                //}
                //catch (System.ComponentModel.Win32Exception ex) {
                //    Console.WriteLine("系统找不到指定的程序文件。\r{0}", ex);
                //    return;
                //}
                ////while (true) {
                ////    if (Proc.HasExited) {
                ////        MessageBox.Show("程序退出了");
                ////        return;
                ////    }
                ////}
                ////Proc.ha
                //Proc.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(Proc_OutputDataReceived); //+= new EventHandler(Proc_Exited);
                //Proc.BeginOutputReadLine();
                ////MessageBox.Show("调用程序!" + Proc.StandardOutput.ReadToEnd());
                //Proc.WaitForExit();

                ////if (Proc.ExitCode != 0) {
                ////    StreamReader aSr = Proc.StandardOutput;
                ////    string aConsole = aSr.ReadToEnd();
                ////    MessageBox.Show("调用程序!" + Proc.StandardOutput.ReadToEnd());
                ////}
                //Proc.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message + "==" + ex.Source + "==" + ex.InnerException);
            }
        }

        private void btnDataBaseLabelTag_Click(object sender, EventArgs e) {
            Bind_contextMenuStrip_Label(contextMenuStrip_Label);
            this.contextMenuStrip_Label.Show(btnDataBaseLabelTag, 0, 21);
        }

        private void Bind_contextMenuStrip_Label(ContextMenuStrip cms) {
            cms.Items.Clear();
            cms.Items.Add("Guid");
            cms.Items.Add("Url");
            DALTaskLabel dal = new DALTaskLabel();
            DataTable dt = dal.GetList(" TaskID=" + this.ID + " Order by OrderID Asc").Tables[0];
            foreach (DataRow dr in dt.Rows) {
                cms.Items.Add(dr["LabelName"].ToString());
            }
        }

        private void btnDataBaseLabelTag4_Click(object sender, EventArgs e) {
            Bind_contextMenuStrip_Label(contextMenuStrip1);
            //this.contextMenuStrip1.Show(btnDataBaseLabelTag4, 0, 21);
        }

        private void listViewTaskLabel_DoubleClick(object sender, EventArgs e) {
            var sel = this.listViewTaskLabel.SelectedItems;
            if (sel != null && sel.Count == 1) {
                frmTaskSpiderLabel FormTaskSpiderLabel = new frmTaskSpiderLabel();
                FormTaskSpiderLabel.EditItem = this.listViewTaskLabel.SelectedItems;
                FormTaskSpiderLabel.ViewLabel = AddViewLabel;
                FormTaskSpiderLabel.TaskID = ID;
                FormTaskSpiderLabel.ShowDialog(this);
            }
        }

        private void listViewWebModule_DoubleClick(object sender, EventArgs e) {

        }

        #region 4.自定义Web发布地址

        private void btnDiyWebUrlAdd_Click(object sender, EventArgs e) {
            frmTaskDiyWebUrl FormDiyWebUrl = new frmTaskDiyWebUrl();
            FormDiyWebUrl.OnDataOperation += (object sender1, MainEvents.DataOperationArgs e1) => {
                Bind_DiyUrlWebList("And SelfId=" + this.ID);
            };
            FormDiyWebUrl.TaskId = this.ID;
            FormDiyWebUrl.ShowDialog(this);
        }

        private void btnDiyWebUrlEdit_Click(object sender, EventArgs e) {
            if (this.listView_DiyUrlWeb.SelectedItems.Count == 0) {
                return;
            }
            frmTaskDiyWebUrl FormDiyWebUrl = new frmTaskDiyWebUrl();
            FormDiyWebUrl.OnDataOperation += (object sender1, MainEvents.DataOperationArgs e1) => {
                Bind_DiyUrlWebList("And SelfId=" + this.ID);
            };
            FormDiyWebUrl.TaskId = this.ID;
            FormDiyWebUrl.EditItem = this.listView_DiyUrlWeb.SelectedItems[0].Tag;
            FormDiyWebUrl.ShowDialog(this);
        }

        private void btnDiyWebUrlDelete_Click(object sender, EventArgs e) {
            if (this.listView_DiyUrlWeb.SelectedItems.Count == 0) {
                return;
            }
            frmTaskDiyWebUrl FormDiyWebUrl = new frmTaskDiyWebUrl();
            FormDiyWebUrl.OnDataOperation += (object sender1, MainEvents.DataOperationArgs e1) => {
                Bind_DiyUrlWebList("And SelfId=" + this.ID);
            };
            FormDiyWebUrl.EditItem = this.listView_DiyUrlWeb.SelectedItems[0].Tag;
            FormDiyWebUrl.Delete();
        }

        private void Bind_DiyUrlWebList(string p) {
            this.listView_DiyUrlWeb.Items.Clear();
            var list = DALDiyWebUrlHelper.GetList(p, "", 0);
            foreach (var l in list) {
                ListViewItem li = new ListViewItem(l.Name);
                li.SubItems.Add(l.Url);
                li.Tag = l.Id;
                this.listView_DiyUrlWeb.Items.Add(li);
            }
        }
        #endregion

        private void btnTaskSet_Click(object sender, EventArgs e) {
            frmTaskPlanSet FormPlanSet = new frmTaskPlanSet();
            FormPlanSet.OnDataOperation += (object sender1, MainEvents.DataOperationArgs e1) => {
                if (e1.Operation == MainEvents.OperationEnum.Edit) {
                    this.txtHiddenPlanFormat.Text = e1.ReturnObj as string;
                }
            };
            FormPlanSet.ShowDialog(this);
        }




    }
}
