using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using V5_DataCollection._Class.Common;
using V5_WinLibs.Utility;
using WeifenLuo.WinFormsUI.Docking;

namespace V5_DataCollection.Forms.Docking {
    public partial class frmOutPutFileDown : DockContent {

        public frmOutPutFileDown() {
            InitializeComponent();
        }

        private void frmOutPutFileDown_Load(object sender, EventArgs e) {
            var th = new ThreadMultiHelper(1, 1);
            th.WorkMethod += th_WorkMethod;
            th.CompleteEvent += th_CompleteEvent;
            th.Start();
        }

        void th_CompleteEvent() {

        }

        void th_WorkMethod(int taskindex, int threadindex) {
            while (true) {
                Dictionary<string, string> d = null;
                lock (QueueHelper.lockObj) {
                    if (QueueHelper.Q_DownImgResource.Count > 0) {
                        d = QueueHelper.Q_DownImgResource.Dequeue();
                    }
                }
                if (d != null) {
                    OutDownload(d.ToString());
                }
                Thread.Sleep(100);
            }
        }


        private void OutDownload(string msg) {
            this.Invoke(new MethodInvoker(() => {
                this.txtLogView.AppendText(msg);
                this.txtLogView.AppendText("\r\n");
            }));
        }
    }
}
