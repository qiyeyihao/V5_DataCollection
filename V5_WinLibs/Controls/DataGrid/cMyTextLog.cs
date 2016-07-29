using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms ;

///功能：自定义文本控件，主要用于采集任务日志的显示
namespace V5.WinControls.DataGrid
{
    class cMyTextLog : RichTextBox 
    {
        public cMyTextLog()
        {
            this.Text = "";
        }

        //public void Clear() 
        //{
        //    this.Text = "";
        //}

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                string strT = value;

                if (strT.Length > 0)
                {
                    try
                    {
                        int infoType = int.Parse(strT.Substring(0, 4));
                        strT = strT.Substring(4, strT.Length - 4);
                        //switch (infoType)
                        //{
                        //    case (int)cGlobalParas.LogType.Error:
                        //        base.SelectionFont = new System.Drawing.Font(DefaultFont, System.Drawing.FontStyle.Bold);
                        //        base.SelectionColor = System.Drawing.Color.Red;
                        //        break;
                        //    case (int)cGlobalParas.LogType.Info:
                        //        base.SelectionFont = new System.Drawing.Font(DefaultFont,System.Drawing.FontStyle.Regular );
                        //        base.SelectionColor = System.Drawing.Color.Black;
                        //        break;
                        //    case (int)cGlobalParas.LogType.Warning:
                        //        base.SelectionFont = new System.Drawing.Font(DefaultFont, System.Drawing.FontStyle.Bold);
                        //        base.SelectionColor = System.Drawing.Color.Orange;
                        //        break;
                        //    default:
                        //        base.SelectionFont = new System.Drawing.Font(DefaultFont, System.Drawing.FontStyle.Regular);
                        //        base.SelectionColor = System.Drawing.Color.Black;
                        //        break;
                        //}

                        base.AppendText(strT );
                        base.SelectionStart = int.MaxValue; 
                        base.ScrollToCaret();

                        
                    }
                    catch (System.Exception)
                    {
                        base.Text = value + base.Text;
                    }
                }
                
            }
        }

       
    }
}
