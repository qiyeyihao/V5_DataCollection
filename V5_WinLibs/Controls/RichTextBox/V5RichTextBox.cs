using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text.RegularExpressions;
using V5.WinControls._Class.V5RichTextBox;

namespace V5.WinControls
{
    /// <summary>
    /// RichTextBoxEx is derived from RichTextBox and supports XP Visual Styles.
    /// </summary>
    public class V5RichTextBox : RichTextBox
    {
        /// <summary>
        /// Contains the size of the visual style borders
        /// </summary>
        NativeMethods.RECT borderRect;

        ContextMenu cm1 = new ContextMenu();
        public V5RichTextBox()
            : base()
        {
            MenuItem miUndo = new MenuItem();
            miUndo.Name = "Undo";
            miUndo.Click += new EventHandler(Undo_Click);
            miUndo.Text = "撤销";
            cm1.MenuItems.Add(miUndo);

            MenuItem miLine = new MenuItem();
            miLine.Name = "Line";
            miLine.Text = "-";
            cm1.MenuItems.Add(miLine);

            MenuItem miCopy = new MenuItem();
            miCopy.Name = "Copy";
            miCopy.Click += new EventHandler(miCopy_Click);
            miCopy.Text = "复制";
            cm1.MenuItems.Add(miCopy);

            MenuItem miPaste = new MenuItem();
            miPaste.Name = "Paste";
            miPaste.Click += new EventHandler(Paste_Click);
            miPaste.Text = "粘贴";
            cm1.MenuItems.Add(miPaste);

            MenuItem miCut = new MenuItem();
            miCut.Name = "Cut";
            miCut.Click += new EventHandler(Cut_Click);
            miCut.Text = "剪切";
            cm1.MenuItems.Add(miCut);

            MenuItem miDel = new MenuItem();
            miDel.Name = "Del";
            miDel.Click += new EventHandler(Del_Click);
            miDel.Text = "删除";
            cm1.MenuItems.Add(miDel);

            miLine = new MenuItem();
            miLine.Name = "Line";
            miLine.Text = "-";
            cm1.MenuItems.Add(miLine);

            MenuItem miSelectAll = new MenuItem();
            miSelectAll.Name = "SelectAll";
            miSelectAll.Click += new EventHandler(SelectAll_Click);
            miSelectAll.Text = "全选";
            cm1.MenuItems.Add(miSelectAll);


            this.ContextMenu = cm1;

            //

            LoadColor();
        }

        #region
        void miCopy_Click(object sender, EventArgs e)
        {
            this.ContextMenu.SourceControl.Select();//先获取焦点，防止点两下才运行
            V5RichTextBox rtb = (V5RichTextBox)this.ContextMenu.SourceControl;
            rtb.Copy();
        }
        void Paste_Click(object sender, EventArgs e)
        {
            this.ContextMenu.SourceControl.Select();
            V5RichTextBox rtb = (V5RichTextBox)this.ContextMenu.SourceControl;
            rtb.Paste();
        }
        void Cut_Click(object sender, EventArgs e)
        {
            this.ContextMenu.SourceControl.Select();
            V5RichTextBox rtb = (V5RichTextBox)this.ContextMenu.SourceControl;
            rtb.Cut();
        }
        void Del_Click(object sender, EventArgs e)
        {
            this.ContextMenu.SourceControl.Select();
            V5RichTextBox rtb = (V5RichTextBox)this.ContextMenu.SourceControl;
            rtb.Text = "";
        }
        void SelectAll_Click(object sender, EventArgs e)
        {
            this.ContextMenu.SourceControl.Select();
            V5RichTextBox rtb = (V5RichTextBox)this.ContextMenu.SourceControl;
            rtb.SelectAll();
        }
        void Undo_Click(object sender, EventArgs e)
        {
            this.ContextMenu.SourceControl.Select();
            V5RichTextBox rtb = (V5RichTextBox)this.ContextMenu.SourceControl;
            rtb.Undo();
        }
        #endregion
        private void LoadColor()
        {
            ShangSe(@"(【(.*?)】：|[(*)])", this, Color.Green);
            ShangSe(@"(【(.*?)】)", this, Color.Green);
            ShangSe(@"(\[(.*?)\])", this, Color.Green);
            //ShangSe(@"(insert|into|values|where)", this, Color.Blue);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            LoadColor();
            base.OnTextChanged(e);
            //int index = this.SelectionStart;　　//记录修改的位置
            //this.SelectAll();
            //this.SelectionColor = Color.Black;
            //string[] keystr ={ "select ", "from ", "where ", " and ", " or ", " order ", " by ", " desc ", " when ", " case ",
            //" then ", " end ", " on ", " in ", " is ", " else ", " left ", " join ", " not ", " null ","(*)" };
            //for (int i = 0; i < keystr.Length; i++)
            //    this.getbunch(keystr[i], this.Text);
            //this.Select(index, 0);　　 //返回修改的位置
            //this.SelectionColor = Color.Black;
        }

        private void ShangSe(string tokens, RichTextBox rt,Color cl)
        {
            //string tokens = "(【(.*?)】：|[(*)])";// "(auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|const|float|short|unsigned|continue|for|signed|void|default|goto|sizeof|volatile|do|if|static|while)";
            Regex rex = new Regex(tokens);
            MatchCollection mc = rex.Matches(this.Text);
            int StartCursorPosition = rt.SelectionStart;
            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;
                rt.Select(startIndex, StopIndex);
                rt.SelectionColor = cl;
                rt.SelectionFont = new Font("宋体", 10);
                rt.SelectionStart = StartCursorPosition;
                rt.SelectionColor = Color.Black;
            }
            rt.Select(StartCursorPosition, 0);
        }

        public int getbunch(string p, string s) //给关键字上色
        {
            int cnt = 0; int M = p.Length; int N = s.Length;
            char[] ss = s.ToCharArray(), pp = p.ToCharArray();
            if (M > N) return 0;
            for (int i = 0; i < N - M + 1; i++)
            {
                int j;
                for (j = 0; j < M; j++)
                {
                    if (ss[i + j] != pp[j]) break;
                }
                if (j == p.Length)
                {
                    this.Select(i, p.Length);
                    this.SelectionColor = Color.Green;
                    Font f = this.Font;
                    this.SelectionFont = new Font("宋体", 11, FontStyle.Bold);
                    cnt++;
                }
            }
            return cnt;
        }
        /// <summary>
        /// Filter some message we need to draw the border.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_NCPAINT: // the border painting is done here.
                    WmNcpaint(ref m);
                    break;
                case NativeMethods.WM_NCCALCSIZE: // the size of the client area is calcuated here.
                    WmNccalcsize(ref m);
                    break;
                case NativeMethods.WM_THEMECHANGED: // Updates styles when the theme is changing.
                    UpdateStyles();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// Calculates the size of the window frame and client area of the RichTextBox
        /// </summary>
        void WmNccalcsize(ref Message m)
        {
            // let the richtextbox control draw the scrollbar if necessary.
            base.WndProc(ref m);

            // we visual styles are not enabled and BorderStyle is not Fixed3D then we have nothing more to do.
            if (!this.RenderWithVisualStyles())
                return;

            // contains detailed information about WM_NCCALCSIZE message
            NativeMethods.NCCALCSIZE_PARAMS par = new NativeMethods.NCCALCSIZE_PARAMS();

            // contains the window frame RECT
            NativeMethods.RECT windowRect;

            if (m.WParam == IntPtr.Zero) // LParam points to a RECT struct
            {
                windowRect = (NativeMethods.RECT)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.RECT));
            }
            else // LParam points to a NCCALCSIZE_PARAMS struct
            {
                par = (NativeMethods.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.NCCALCSIZE_PARAMS));
                windowRect = par.rgrc0;
            }

            // contains the client area of the control
            NativeMethods.RECT contentRect;

            // get the DC
            IntPtr hDC = NativeMethods.GetWindowDC(this.Handle);

            // open theme data
            IntPtr hTheme = NativeMethods.OpenThemeData(this.Handle, "EDIT");

            // find out how much space the borders needs
            if (NativeMethods.GetThemeBackgroundContentRect(hTheme, hDC, NativeMethods.EP_EDITTEXT, NativeMethods.ETS_NORMAL
                , ref windowRect
                , out contentRect) == NativeMethods.S_OK)
            {
                // shrink the client area the make more space for containing text.
                contentRect.Inflate(-1, -1);

                // remember the space of the borders
                this.borderRect = new NativeMethods.RECT(contentRect.Left - windowRect.Left
                    , contentRect.Top - windowRect.Top
                    , windowRect.Right - contentRect.Right
                    , windowRect.Bottom - contentRect.Bottom);

                // update LParam of the message with the new client area
                if (m.WParam == IntPtr.Zero)
                {
                    Marshal.StructureToPtr(contentRect, m.LParam, false);
                }
                else
                {
                    par.rgrc0 = contentRect;
                    Marshal.StructureToPtr(par, m.LParam, false);
                }

                // force the control to redraw it磗 client area
                m.Result = new IntPtr(NativeMethods.WVR_REDRAW);
            }

            // release theme data handle
            NativeMethods.CloseThemeData(hTheme);

            // release DC
            NativeMethods.ReleaseDC(this.Handle, hDC);
        }

        /// <summary>
        /// The border painting is done here.
        /// </summary>
        void WmNcpaint(ref Message m)
        {
            base.WndProc(ref m);

            if (!this.RenderWithVisualStyles())
            {
                return;
            }

            /////////////////////////////////////////////////////////////////////////////
            // Get the DC of the window frame and paint the border using uxTheme API磗
            /////////////////////////////////////////////////////////////////////////////

            // set the part id to TextBox
            int partId = NativeMethods.EP_EDITTEXT;

            // set the state id of the current TextBox
            int stateId;
            if (this.Enabled)
                if (this.ReadOnly)
                    stateId = NativeMethods.ETS_READONLY;
                else
                    stateId = NativeMethods.ETS_NORMAL;
            else
                stateId = NativeMethods.ETS_DISABLED;

            // define the windows frame rectangle of the TextBox
            NativeMethods.RECT windowRect;
            NativeMethods.GetWindowRect(this.Handle, out windowRect);
            windowRect.Right -= windowRect.Left; windowRect.Bottom -= windowRect.Top;
            windowRect.Top = windowRect.Left = 0;

            // get the device context of the window frame
            IntPtr hDC = NativeMethods.GetWindowDC(this.Handle);

            // define a rectangle inside the borders and exclude it from the DC
            NativeMethods.RECT clientRect = windowRect;
            clientRect.Left += this.borderRect.Left;
            clientRect.Top += this.borderRect.Top;
            clientRect.Right -= this.borderRect.Right;
            clientRect.Bottom -= this.borderRect.Bottom;
            NativeMethods.ExcludeClipRect(hDC, clientRect.Left, clientRect.Top, clientRect.Right, clientRect.Bottom);

            // open theme data
            IntPtr hTheme = NativeMethods.OpenThemeData(this.Handle, "EDIT");

            // make sure the background is updated when transparent background is used.
            if (NativeMethods.IsThemeBackgroundPartiallyTransparent(hTheme
                , NativeMethods.EP_EDITTEXT, NativeMethods.ETS_NORMAL) != 0)
            {
                NativeMethods.DrawThemeParentBackground(this.Handle, hDC, ref windowRect);
            }

            // draw background
            NativeMethods.DrawThemeBackground(hTheme, hDC, partId, stateId, ref windowRect, IntPtr.Zero);

            // close theme data
            NativeMethods.CloseThemeData(hTheme);

            // release dc
            NativeMethods.ReleaseDC(this.Handle, hDC);

            // we have processed the message so set the result to zero
            m.Result = IntPtr.Zero;
        }

        /// <summary>
        /// Returns true, when visual styles are enabled in this application.
        /// </summary>
        bool VisualStylesEnabled()
        {
            // Check if RenderWithVisualStyles property is available in the Application class (New feature in NET 2.0)
            Type t = typeof(Application);
            System.Reflection.PropertyInfo pi = t.GetProperty("RenderWithVisualStyles");

            if (pi == null)
            {
                // NET 1.1
                OperatingSystem os = System.Environment.OSVersion;
                if (os.Platform == PlatformID.Win32NT && (((os.Version.Major == 5) && (os.Version.Minor >= 1)) || (os.Version.Major > 5)))
                {
                    NativeMethods.DLLVersionInfo version = new NativeMethods.DLLVersionInfo();
                    version.cbSize = Marshal.SizeOf(typeof(NativeMethods.DLLVersionInfo));
                    if (NativeMethods.DllGetVersion(ref version) == 0)
                    {
                        return (version.dwMajorVersion > 5) && NativeMethods.IsThemeActive() && NativeMethods.IsAppThemed();
                    }
                }

                return false;
            }
            else
            {
                // NET 2.0
                bool result = (bool)pi.GetValue(null, null);
                return result;
            }
        }

        /// <summary>
        /// Return true, when this control should render with visual styles.
        /// </summary>
        /// <returns></returns>
        bool RenderWithVisualStyles()
        {
            return (this.BorderStyle == BorderStyle.Fixed3D && this.VisualStylesEnabled());
        }

        /// <summary>
        /// Update the control parameters.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;

                // remove the Fixed3D border style
                if (this.RenderWithVisualStyles() && (p.ExStyle & NativeMethods.WS_EX_CLIENTEDGE) == NativeMethods.WS_EX_CLIENTEDGE)
                    p.ExStyle ^= NativeMethods.WS_EX_CLIENTEDGE;

                return p;
            }
        }

    }
}
