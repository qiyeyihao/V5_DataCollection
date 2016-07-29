using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace V5_WinLibs.Core {
    public class AppRunHelper {
        /*
         例子
         public delegate void OutPutTextHandler(object sender, AppRunHelper.AppRunEventArgs e);
         public event OutPutTextHandler OutPutText;
         
        */
        public delegate void OutPutTextHandler(object sender, AppRunEventArgs e);
        public static event OutPutTextHandler OutPutText;
        public static OutPutTextHandler OutPutMessage;
        AppRunHelper() {
            OutPutText += new OutPutTextHandler(AppRunHelper_OutPutText);
        }

        void AppRunHelper_OutPutText(object sender, AppRunHelper.AppRunEventArgs e) {
            if (OutPutMessage != null) {
                OutPutMessage(sender, e);
            }
        }
        /// <summary>
        /// 运行小型浏览器
        /// </summary>
        public static Form AppRunWebBrowserByAssembly(Form mainForm) {
            try {
                string path = AppDomain.CurrentDomain.BaseDirectory + "V5.DataWebBrowser.exe";
                Assembly assem = Assembly.LoadFile(path);//Assembly.GetExecutingAssembly();
                Type tExForm = assem.GetType("V5.DataWebBrowser.frmMainBrowser");
                Object exFormAsObj = Activator.CreateInstance(tExForm);
                #region 字段
                FieldInfo fi = tExForm.GetField("IsInvoke", BindingFlags.NonPublic | BindingFlags.Instance);
                fi.SetValue(exFormAsObj, true);
                #endregion
                #region 事件
                // 获取表示该事件的 EventInfo 对象，并使用 EventHandlerType 属性来获取用于处理事件的委托类型。
                EventInfo evClick = tExForm.GetEvent("OutPutText");
                Type tDelegate = evClick.EventHandlerType;
                // 获取表示处理事件的方法的 MethodInfo 对象。
                MethodInfo miHandler =
                    typeof(AppRunHelper).GetMethod("AppRunHelper_OutPutText",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                // 使用 CreateDelegate 方法创建委托的实例。
                Delegate d = Delegate.CreateDelegate(tDelegate, null, miHandler);
                // 下面的代码获取 Click 事件的 add 访问器并以后期绑定方式调用它，并在委托实例中传递。参数必须作为数组传
                // 递。
                MethodInfo addHandler = evClick.GetAddMethod();
                Object[] addHandlerArgs = { d };
                addHandler.Invoke(exFormAsObj, addHandlerArgs);
                #endregion
                Form f = (Form)exFormAsObj;
                f.Show(mainForm);
                return f;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message + "==" + ex.Source + "==" + ex.InnerException);
            }
            return null;
        }
        /// <summary>
        /// 返回事件
        /// </summary>
        public class AppRunEventArgs : EventArgs {
            public string Msg1 { get; set; }
            public string Msg2 { get; set; }
            public string Msg3 { get; set; }
            public string Msg4 { get; set; }
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="isInvoke"></param>
        public static void CloseWindow(Form fr, bool isInvoke) {
            GC.Collect();
            if (isInvoke) {
                fr.Close();
            }
            else {

                System.Environment.Exit(0);
            }
        }


        /// <summary>
        /// 打开应用程序
        /// </summary>
        /// <param name="cmdPath"></param>
        public static void RunAppCmd(string cmdPath, string cmdArguments) {
            try {
                /*
                            //实例化一个进程类
                            Process cmd = new Process();
                            //获得系统信息，使用的是 systeminfo.exe 这个控制台程序
                            cmd.StartInfo.FileName = cmdPath;
                            //将cmd的标准输入和输出全部重定向到.NET的程序里
                            cmd.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常
                            cmd.StartInfo.RedirectStandardInput = true; //标准输入
                            cmd.StartInfo.RedirectStandardOutput = true; //标准输出
                            //不显示命令行窗口界面
                            cmd.StartInfo.CreateNoWindow = true;
                            // cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            cmd.Start(); //启动进程
                            cmd.WaitForExit();//等待控制台程序执行完成
                            cmd.Close();//关闭该进程
                            */
                //实例化一个进程类
                Process cmd = new Process();
                //获得系统信息，使用的是 systeminfo.exe 这个控制台程序
                cmd.StartInfo.FileName = cmdPath;
                cmd.StartInfo.Arguments = cmdArguments;
                //不显示命令行窗口界面
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                cmd.Start(); //启动进程
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message + "==" + ex.Source + "==" + ex.InnerException);
            }

        }
    }
}
