using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace V5.DataWebBrowser {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            //if (args.Length > 0) {
            //    string ss = string.Empty;
            //    foreach (string str in args) {
            //        ss += str + ",";
            //    }
            //    MessageBox.Show(ss);
            //}
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMainBrowser());
        }
    }
}
