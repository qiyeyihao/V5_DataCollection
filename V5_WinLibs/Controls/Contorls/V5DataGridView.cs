using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace V5_WinControls {
    public partial class V5DataGridView : DataGridView {
        public V5DataGridView() {
            InitializeComponent();
        }

        public V5DataGridView(IContainer container) {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            //int NewRowHeight = this.RowTemplate.Height;
            ////新建的行高
            //int AllHeight = this.ColumnHeadersHeight + NewRowHeight * this.RowCount;
            ////总高度 包括行高
            //int TableAllWidth = this.Width - 2;
            ////表格总宽
            //Rectangle rFrame = new Rectangle(0, 0, TableAllWidth, NewRowHeight);
            //// 存储四组数字位置及高度
            //Rectangle rFill = new Rectangle(1, 1, TableAllWidth - 2, NewRowHeight);
            //// 存储四组数字位置及高度
            //Rectangle rowHeader = new Rectangle(2, 2, this.RowHeadersWidth - 3, NewRowHeight);
            //// 存储四组数字位置及高度
            //Pen line = new Pen(this.GridColor, 1);
            ////线
            //Bitmap photo = new Bitmap(TableAllWidth, NewRowHeight);
            //Graphics fengPhoto = Graphics.FromImage(photo);
            //fengPhoto.DrawRectangle(line, rFrame);
            ////绘制框
            //fengPhoto.FillRectangle(new SolidBrush(this.DefaultCellStyle.BackColor), rFill);
            ////填充颜色
            //// 封装图.FillRectangle(New SolidBrush(Me.RowHeadersDefaultCellStyle.BackColor), rowHeader
            //int TitleColumnWidth = this.RowHeadersWidth - this.RowHeadersWidth;
            //int j = 0;
            //for (j = 0; j <= this.ColumnCount - 1; j += j + 1) {
            //    fengPhoto.DrawLine(line, new Point(TitleColumnWidth, -0), new Point(TitleColumnWidth, NewRowHeight));
            //    TitleColumnWidth += this.Columns[j].Width;
            //}
            //int dd = (this.Height - AllHeight) / NewRowHeight;
            //int CC = 0;
            //for (CC = 0; CC <= dd + 1 - 1; CC += CC + 1) {
            //    e.Graphics.DrawImage(photo, 1, AllHeight + CC * NewRowHeight);
            //}
        }
    }
}
