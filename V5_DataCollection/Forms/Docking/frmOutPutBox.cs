using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace V5_DataCollection.Forms.Docking {
    public partial class frmOutPutBox : DockContent {

        public frmOutPutBox() {
            InitializeComponent();

            this.txtOutWindowString.Dock = DockStyle.Fill;
        }
        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OutPutWindow(object sender, MainEvents.OutPutWindowEventArgs e) {
            if (this.txtOutWindowString.InvokeRequired) {
                this.txtOutWindowString.BeginInvoke(new MethodInvoker(delegate() {
                    this.txtOutWindowString.AppendText("【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】 " + e.Message);
                    this.txtOutWindowString.AppendText("\r\n");
                }));
            }
            else {
                this.txtOutWindowString.AppendText("【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】 " + e.Message);
                this.txtOutWindowString.AppendText("\r\n");
            }
        }

        private void toolStripButton_ContentClear_Click(object sender, EventArgs e) {
            this.txtOutWindowString.Clear();
        }
    }
}
