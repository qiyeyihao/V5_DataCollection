using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_DataCollection.Forms.Docking;
using V5_DataCollection.Forms.Publish;
using V5_DataCollection.Forms.Task;
using V5_DataCollection.Forms.Tools;
using System.Diagnostics;

using V5_DataCollection._Class.Common;
using V5_DataCollection._Class.DAL;
using WeifenLuo.WinFormsUI.Docking;
using V5_WinLibs.Core;

namespace V5_DataCollection {
    public partial class frmMain : Form {
        private DeserializeDockContent m_deserializeDockContent;

        #region 主界面浮动窗体
        private frmTreeBox m_frmTreeBox = new frmTreeBox();
        private FrmTaskMain m_frmTaskMain = new FrmTaskMain();
        private frmOutPutBox m_frmOutPutBox = new frmOutPutBox();
        private frmOutPutFileDown m_frmOutPutFileDown = new frmOutPutFileDown();
        #endregion

        public frmMain() {
            InitializeComponent();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            CommonHelper.FormMain = this;
        }
        /// <summary>
        /// 任务树操作结果委托方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_frmTreeBox_OpOver(object sender, MainEvents.TreeViewEventArgs e) {
            switch (e.Result) {
                case "selectednode":
                    this.m_frmTaskMain.Show();
                    int ClassId = int.Parse("0" + e.ReturnObj);
                    m_frmTaskMain.ClassID = ClassId;
                    m_frmTaskMain.Bind_DataList();
                    break;
                case "selectednodetask":
                    this.m_frmTaskMain.Show();
                    m_frmTaskMain.Bind_DataList(" And IsPlan=1 ");
                    break;
                default:
                    MessageBox.Show("操作:" + e.Result + ".结果:" + e.Message);
                    break;
            }
        }

        /// <summary>
        /// 配置委托函数
        /// </summary>
        private IDockContent GetContentFromPersistString(string persistString) {
            //if (dockPanel.DocumentStyle == DocumentStyle.DockingMdi) {
            //    foreach (Form form in MdiChildren)
            //        if (form.Text == persistString)
            //            return form as IDockContent;

            //    return null;
            //}
            //else {
            //    foreach (IDockContent content in dockPanel.Documents)
            //        if (content.DockHandler.TabText == persistString)
            //            return content;

            //    return null;
            //}
            if (persistString == typeof(frmTreeBox).ToString()) {
                return m_frmTreeBox;
            }
            else if (persistString == typeof(FrmTaskMain).ToString()) {
                return m_frmTaskMain;
            }
            else if (persistString == typeof(frmOutPutBox).ToString()) {
                return m_frmOutPutBox;
            }
            else if (persistString == typeof(frmOutPutFileDown).ToString()) {
                return m_frmOutPutFileDown;
            }
            else {
                return null;
            }
            //Type t = Type.GetType(persistString);
            //object o = System.Activator.CreateInstance(t);
            //return (DockContent)o;
        }
        /// <summary>
        /// 主窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e) {
            //加载浮动设置
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Config/DockPanel.config");
            if (File.Exists(configFile)) {
                try {
                    this.dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
                }
                catch (Exception) {
                }
            }

            m_frmTaskMain.OutPutWindowDelegate = OutPutWindowBox;
            m_frmTreeBox.OpOver += m_frmTreeBox_OpOver;
        }
        /// <summary>
        /// 程序退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Operate_Exit_Click(object sender, EventArgs e) {
            Exit();
        }

        #region  浮动控制
        private void ToolStripMenuItem_View_TaskTree_Click(object sender, EventArgs e) {
            m_frmTreeBox.Show(dockPanel, DockState.DockLeft);
        }

        private void ToolStripMenuItem_View_TaskView_Click(object sender, EventArgs e) {
            m_frmTaskMain.Show(dockPanel, DockState.Document);
        }


        private void 资源下载列表ToolStripMenuItem_Click(object sender, EventArgs e) {
            m_frmOutPutFileDown.Show(dockPanel, DockState.DockBottom);
        }
        #endregion

        private void ToolStripMenuItem_View_OutWindow_Click(object sender, EventArgs e) {
            m_frmOutPutBox.Show(dockPanel, DockState.DockBottom);
        }

        //保存浮动设置
        private void SaveDockXml() {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Config/DockPanel.config");
            if (!File.Exists(configFile)) {
                FileInfo ff = new FileInfo(configFile);
                var dd = ff.DirectoryName;
                if (!Directory.Exists(dd)) {
                    Directory.CreateDirectory(dd);
                }
                using (StreamWriter sw = new StreamWriter(configFile, false, Encoding.UTF8)) {
                    sw.Write("");
                    sw.Flush();
                    sw.Close();
                }
            }
            dockPanel.SaveAsXml(configFile);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            Exit();
        }

        private void Exit() {
            SaveDockXml();
            this.Dispose();
            System.Environment.Exit(0);
        }

        #region OutPutWindow 输出窗口
        public void OutPutWindowBox(object sender, MainEvents.OutPutWindowEventArgs e) {
            m_frmOutPutBox.OutPutWindow(sender, e);
        }
        #endregion

        private void ToolStripMenuItem_Tool_Config_Click(object sender, EventArgs e) {
            frmOption option = new frmOption();
            option.Show();
        }

        #region 工具条
        private void toolStripButton_TaskNew_Click(object sender, EventArgs e) {
            FrmTaskEdit FormTaskEdit = new FrmTaskEdit();
            FormTaskEdit.TaskOpDelegate = m_frmTaskMain.OutTaskOpDelegate;
            FormTaskEdit.ShowDialog(this);
        }

        private void toolStripButton_TaskEdit_Click(object sender, EventArgs e) {
            int taskId = m_frmTaskMain.Get_DataViewID();
            if (taskId > 0) {
                FrmTaskEdit FormTaskEdit = new FrmTaskEdit();
                FormTaskEdit.TaskOpDelegate = m_frmTaskMain.OutTaskOpDelegate;
                FormTaskEdit.ID = taskId;
                FormTaskEdit.ShowDialog(this);
            }
        }

        private void toolStripButton_TaskDelete_Click(object sender, EventArgs e) {
            int taskId = m_frmTaskMain.Get_DataViewID();
            if (taskId > 0) {
                if (MessageBox.Show("你确定要删除吗?删除不可恢复!", "警告!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                    DALTask dal = new DALTask();
                    dal.Delete(taskId);
                    m_frmTaskMain.Bind_DataList();
                }
            }
        }


        private void toolStripButton_SoftOption_Click(object sender, EventArgs e) {
            frmOption option = new frmOption();
            option.ShowDialog(this);
        }

        private void toolStripButton_SoftAbout_Click(object sender, EventArgs e) {
            new frmAboutBox().ShowDialog(this);
        }

        #endregion

        private void ToolStripMenuItem_ImportWebCollection_Click(object sender, EventArgs e) {
            frmImportWebCollectionModule formImportWebCollectionModule = new frmImportWebCollectionModule();
            formImportWebCollectionModule.ShowDialog();
        }

        private void ToolStripMenuItem_ImportWebPublish_Click(object sender, EventArgs e) {
            frmImportWebPublishModule formImportWebPublishModule = new frmImportWebPublishModule();
            formImportWebPublishModule.ShowDialog();
        }

        private void ToolStripMenuItem_AboutUs_Click(object sender, EventArgs e) {
            new frmAboutBox().ShowDialog();
        }

        private void ToolStripMenuItem_SoftUpdate_Click(object sender, EventArgs e) {
            AppRunHelper.RunAppCmd("V5.AutoUpdate.exe", "-c V5.DataCollection");
        }

        private void ToolStripMenuItem_ManageSite_Click(object sender, EventArgs e) {

        }

        private void 重新启动ToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Restart();
        }

        private void sQLToolStripMenuItem_Click(object sender, EventArgs e) {
            frmSQL formSQL = new frmSQL();
            formSQL.ShowDialog(this);
        }

        private void v5ToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("http://www.v5soft.com");
        }

        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e) {
            var ff = new frmTest();
            ff.Show(this);
        }
    }
}
