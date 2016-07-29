using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace V5.AutoUpdate {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            //String temp = "";
            //string song, singer, path;
            //int head, tail;
            //// 合并命令行参数
            //for (int i = 0; i < args.Length; i++) {
            //    MessageBox.Show(args[i]);
            //    temp += args[i];
            //    temp += " ";
            //}
            //MessageBox.Show(temp);
            //try {
            //    // 解析合并后的命令行
            //    head = temp.IndexOf('|', 0);
            //    song = temp.Substring(0, head);
            //    MessageBox.Show(song);
            //    head += 1;
            //    tail = temp.IndexOf('|', head);
            //    singer = temp.Substring(head, tail - head);
            //    MessageBox.Show(singer);
            //    path = temp.Substring(tail + 1);
            //    MessageBox.Show(path);
            //}
            //catch { }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmUpdate());
        }
    }
}
