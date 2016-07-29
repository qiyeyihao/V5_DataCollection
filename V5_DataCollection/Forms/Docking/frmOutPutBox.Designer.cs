namespace V5_DataCollection.Forms.Docking
{
    partial class frmOutPutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOutPutBox));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton_ContentClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtOutWindowString = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackgroundImage = global::V5_DataCollection.Properties.Resources.背景;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.toolStripButton_ContentClear,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(675, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripButton_ContentClear
            // 
            this.toolStripButton_ContentClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_ContentClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ContentClear.Image")));
            this.toolStripButton_ContentClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ContentClear.Name = "toolStripButton_ContentClear";
            this.toolStripButton_ContentClear.Size = new System.Drawing.Size(84, 22);
            this.toolStripButton_ContentClear.Text = "清除文本内容";
            this.toolStripButton_ContentClear.Click += new System.EventHandler(this.toolStripButton_ContentClear_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtOutWindowString
            // 
            this.txtOutWindowString.Location = new System.Drawing.Point(56, 68);
            this.txtOutWindowString.Multiline = true;
            this.txtOutWindowString.Name = "txtOutWindowString";
            this.txtOutWindowString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutWindowString.Size = new System.Drawing.Size(475, 133);
            this.txtOutWindowString.TabIndex = 1;
            // 
            // frmOutPutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 284);
            this.Controls.Add(this.txtOutWindowString);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "frmOutPutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "输出窗口";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TextBox txtOutWindowString;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton_ContentClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}