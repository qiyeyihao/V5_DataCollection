using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_Model;
using System.IO;
using V5_DataCollection._Class.DAL;

namespace V5_DataCollection.Forms.Task {
    public partial class frmTaskSpiderLabel : Form {
        #region 委托
        public TaskEventHandler.AddViewLabel ViewLabel;
        private TaskEvents.AddViewLabelEvents ev = new TaskEvents.AddViewLabelEvents();
        #endregion

        #region 访问器
        private object _EditItem = null;

        public object EditItem {
            get { return _EditItem; }
            set { _EditItem = value; }
        }

        private int _TaskID = 0;
        /// <summary>
        /// 任务ID
        /// </summary>
        public int TaskID {
            get { return _TaskID; }
            set { _TaskID = value; }
        }
        #endregion

        DALTaskLabel dal = new DALTaskLabel();

        public frmTaskSpiderLabel() {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e) {
            int ID = int.Parse("0" + this.txtID.Text);
            ModelTaskLabel model = new ModelTaskLabel();
            model.TaskID = TaskID;
            model.LabelName = this.txtLabelName.Text;
            model.LabelNameCutRegex = this.txtLabelNameCutRegex.Text.Replace("'", "''");
            model.LabelRemove = string.Empty;
            model.LabelReplace = string.Empty;

            #region Html过滤
            model.LabelHtmlRemove = string.Empty;
            if (this.chkAllHtml.Checked)
                model.LabelHtmlRemove += "all||||";
            if (this.chkTable.Checked)
                model.LabelHtmlRemove += "table||||";
            if (this.chkFont.Checked)
                model.LabelHtmlRemove += "font<span>||||";
            if (this.chkhref.Checked)
                model.LabelHtmlRemove += "a||||";
            if (this.chkScript.Checked)
                model.LabelHtmlRemove += "script||||";
            if (model.LabelHtmlRemove.Trim() != "")
                model.LabelHtmlRemove = model.LabelHtmlRemove.Remove(model.LabelHtmlRemove.Length - 4);
            #endregion
            #region 内容排除
            foreach (ListViewItem item in this.listViewContentRemove.Items) {
                model.LabelRemove += item.SubItems[0].Text + "$$$$";
            }
            if (model.LabelRemove.Trim() != "") {
                model.LabelRemove = model.LabelRemove.Remove(model.LabelRemove.Length - 4);
            }
            #endregion
            #region 内容替换
            foreach (ListViewItem item in this.listViewContentReplace.Items) {
                model.LabelReplace += item.SubItems[0].Text + "||" + item.SubItems[1].Text + "$$$$";
            }
            if (model.LabelReplace.Trim() != "") {
                model.LabelReplace = model.LabelReplace.Remove(model.LabelReplace.Length - 4);
            }
            #endregion

            model.IsLoop = this.chkLabelIsLoop.Checked ? 1 : 0;
            model.IsNoNull = this.chkLabelNoNull.Checked ? 1 : 0;
            model.IsLinkUrl = this.chkLabelIsLinkUrl.Checked ? 1 : 0;
            model.IsPager = this.chkLabelIsPager.Checked ? 1 : 0;
            model.LabelValueLinkUrlRegex = this.txtLabelValueIsLinkUrlRegex.Text.Replace("'", "''");
            model.LabelValuePagerRegex = this.txtLabelValueIsPagerRegex.Text.Replace("'", "''");
            //
            model.SpiderLabelPlugin = this.cmbSpiderPlugin.Text;
            //下载文件
            model.IsDownResource = this.chkDownResource.Checked ? 1 : 0;
            model.DownResourceExts = this.txtDownResourceExt.Text;
            if (ID > 0) {
                model.ID = ID;
                dal.Update(model);
            }
            else if (ID == 0) {
                //获取最大的排序ID
                int OrderID = dal.GetMaxOrderID(TaskID) + 1;
                model.OrderID = OrderID;
                ID = dal.Add(model);
            }
            if (ViewLabel != null) {
                ViewLabel(this, ev);
            }
            this.Hide();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Hide();
            this.Close();
        }

        private void frmTaskSpiderLabel_Load(object sender, EventArgs e) {

            Bind_SpiderContentPlugin();



            if (EditItem != null) {
                Bind_DataEdit();
            }
        }
        /// <summary>
        /// 编辑标签
        /// </summary>
        private void Bind_DataEdit() {
            DALTaskLabel dal = new DALTaskLabel();
            ListView.SelectedListViewItemCollection item = (ListView.SelectedListViewItemCollection)EditItem;
            int ID = int.Parse("0" + item[0].Tag);
            ModelTaskLabel model = dal.GetModel(ID);
            this.txtID.Text = ID.ToString();
            this.txtLabelName.Text = model.LabelName;
            this.txtLabelNameCutRegex.Text = model.LabelNameCutRegex;
            if (!string.IsNullOrEmpty(model.LabelHtmlRemove)) {
                string[] arr = model.LabelHtmlRemove.Split(new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr) {
                    if (str == "all") {
                        this.chkAllHtml.Checked = true;
                    }
                    else if (str == "table") {
                        this.chkTable.Checked = true;
                    }
                    else if (str == "font<span>") {
                        this.chkFont.Checked = true;
                    }
                    else if (str == "a") {
                        this.chkhref.Checked = true;
                    }
                    else if (str == "script") {
                        this.chkScript.Checked = true;
                    }
                }
            }
            #region 内容排除
            if (!string.IsNullOrEmpty(model.LabelRemove)) {
                foreach (string str in model.LabelRemove.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                    ListViewItem lvi = new ListViewItem(str);
                    this.listViewContentRemove.Items.Add(lvi);
                }
            }
            #endregion

            #region 内容替换
            if (!string.IsNullOrEmpty(model.LabelReplace)) {
                foreach (string str in model.LabelReplace.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                    string[] aa = str.Split(new string[] { "||" }, StringSplitOptions.None);
                    ListViewItem lvi = new ListViewItem(aa[0]);
                    lvi.SubItems.Add(aa[1]);
                    this.listViewContentReplace.Items.Add(lvi);
                }
            }
            #endregion


            this.chkLabelIsLoop.Checked = model.IsLoop == 1 ? true : false;
            this.chkLabelNoNull.Checked = model.IsNoNull == 1 ? true : false;
            this.chkLabelIsLinkUrl.Checked = model.IsLinkUrl == 1 ? true : false;
            this.chkLabelIsPager.Checked = model.IsPager == 1 ? true : false;

            this.txtLabelValueIsLinkUrlRegex.Text = model.LabelValueLinkUrlRegex;
            this.txtLabelValueIsPagerRegex.Text = model.LabelValuePagerRegex;


            this.cmbSpiderPlugin.Text = model.SpiderLabelPlugin == string.Empty ? "不使用插件" : model.SpiderLabelPlugin;

            //下载资源
            this.chkDownResource.Checked = model.IsDownResource == 1 ? true : false;
            this.txtDownResourceExt.Text = model.DownResourceExts;
        }


        #region 内容排除
        /// <summary>
        /// 内容排除添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContentRemoveAdd_Click(object sender, EventArgs e) {
            frmTaskLabelRemove FormTaskLabelRemove = new frmTaskLabelRemove();
            FormTaskLabelRemove.TLR = OutTaskLabelRemove;
            FormTaskLabelRemove.ShowDialog();
        }
        /// <summary>
        /// 内容排除编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContentRemoveEdit_Click(object sender, EventArgs e) {
            int selectIndex = -1;
            string Name = string.Empty;
            ListView.SelectedIndexCollection indexes = this.listViewContentRemove.SelectedIndices;
            foreach (int index in indexes) {
                Name = this.listViewContentRemove.Items[index].SubItems[0].Text;
                selectIndex = index;
                if (selectIndex != -1)
                    break;
            }
            if (selectIndex == -1)
                return;
            frmTaskLabelRemove FormTaskLabelRemove = new frmTaskLabelRemove();
            FormTaskLabelRemove.TLR = OutTaskLabelRemove;
            FormTaskLabelRemove.OldName = Name;
            FormTaskLabelRemove.ItemIndex = selectIndex;
            FormTaskLabelRemove.ShowDialog();
        }
        /// <summary>
        /// 内容排除删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContentRemoveDel_Click(object sender, EventArgs e) {
            int selectIndex = -1;
            ListView.SelectedIndexCollection indexes = this.listViewContentRemove.SelectedIndices;
            foreach (int index in indexes) {
                selectIndex = index;
                if (selectIndex != -1)
                    break;
            }
            if (selectIndex == -1)
                return;
            this.listViewContentRemove.Items.RemoveAt(selectIndex);
        }
        /// <summary>
        /// 内容排除委托显示
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="RemoveStr"></param>
        /// <param name="DbType"></param>
        private void OutTaskLabelRemove(int itemIndex, string RemoveStr, string DbType) {
            if (DbType == "add") {
                ListViewItem lvi = new ListViewItem(RemoveStr);
                this.listViewContentRemove.Items.Add(lvi);
            }
            else if (DbType == "edit") {
                this.listViewContentRemove.Items[itemIndex].SubItems[0].Text = RemoveStr;
            }
        }
        #endregion

        #region 内容过滤
        /// <summary>
        /// 内容过滤 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContentReplaceAdd_Click(object sender, EventArgs e) {
            frmTaskLabelReplace FormTaskLabelReplace = new frmTaskLabelReplace();
            FormTaskLabelReplace.TLR = OutTaskLabelReplace;
            FormTaskLabelReplace.ShowDialog(this);
        }
        /// <summary>
        /// 内容过滤编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContentReplaceEdit_Click(object sender, EventArgs e) {
            string oldStr = string.Empty, newStr = string.Empty;
            int selectIndex = -1;
            ListView.SelectedIndexCollection indexes = this.listViewContentReplace.SelectedIndices;
            foreach (int index in indexes) {
                selectIndex = index;
                oldStr = this.listViewContentReplace.Items[index].SubItems[0].Text;
                newStr = this.listViewContentReplace.Items[index].SubItems[1].Text;
                if (selectIndex != -1)
                    break;
            }
            if (selectIndex == -1)
                return;
            frmTaskLabelReplace FormTaskLabelReplace = new frmTaskLabelReplace();
            FormTaskLabelReplace.TLR = OutTaskLabelReplace;
            FormTaskLabelReplace.ItemIndex = selectIndex;
            FormTaskLabelReplace.OldStr = oldStr;
            FormTaskLabelReplace.NewStr = newStr;
            FormTaskLabelReplace.ShowDialog(this);
        }
        /// <summary>
        /// 内容过滤删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContentReplaceDel_Click(object sender, EventArgs e) {
            int selectIndex = -1;
            ListView.SelectedIndexCollection indexes = this.listViewContentReplace.SelectedIndices;
            foreach (int index in indexes) {
                selectIndex = index;
                if (selectIndex != -1)
                    break;
            }
            if (selectIndex == -1)
                return;
            this.listViewContentReplace.Items.RemoveAt(selectIndex);
        }
        /// <summary>
        /// 内容过滤委托显示
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        /// <param name="DbType"></param>
        private void OutTaskLabelReplace(int itemIndex, string oldStr, string newStr, string DbType) {
            if (DbType == "add") {
                ListViewItem lvi = new ListViewItem(oldStr);
                lvi.SubItems.Add(newStr);
                this.listViewContentReplace.Items.Add(lvi);
            }
            else if (DbType == "edit") {
                this.listViewContentReplace.Items[itemIndex].SubItems[0].Text = oldStr;
                this.listViewContentReplace.Items[itemIndex].SubItems[1].Text = newStr;
            }
        }
        #endregion

        private void chkLabelValueIsPager_CheckedChanged(object sender, EventArgs e) {
            if (this.chkLabelIsPager.Checked) {
                this.txtLabelValueIsPagerRegex.Enabled = true;
            }
            else {
                this.txtLabelValueIsPagerRegex.Enabled = false;
            }
        }

        private void chkLabelValueIsLinkUrl_CheckedChanged(object sender, EventArgs e) {
            if (this.chkLabelIsLinkUrl.Checked) {
                this.txtLabelValueIsLinkUrlRegex.Enabled = true;
            }
            else {
                this.txtLabelValueIsLinkUrlRegex.Enabled = false;
            }
        }

        private static string SpiderUrlPluginPath = AppDomain.CurrentDomain.BaseDirectory + "\\System\\SpiderLabel\\";

        private void Bind_SpiderContentPlugin() {
            if (!Directory.Exists(SpiderUrlPluginPath)) {
                Directory.CreateDirectory(SpiderUrlPluginPath);
            }
            var publishFiles = Directory.GetFiles(SpiderUrlPluginPath, "*.py");

            foreach (string str2 in publishFiles) {

                FileInfo fi = new FileInfo(str2);
                string fullName = "System\\SpiderLabel\\" + fi.Name;
                this.cmbSpiderPlugin.Items.Add(fullName);
            }

            this.cmbSpiderPlugin.Items.Insert(0, "不使用插件");
            this.cmbSpiderPlugin.SelectedIndex = 0;
        }

        private void lblPluginEdit_Click(object sender, EventArgs e) {

        }


    }
}
