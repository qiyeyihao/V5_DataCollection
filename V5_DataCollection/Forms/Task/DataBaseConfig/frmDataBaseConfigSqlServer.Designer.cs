namespace V5_DataCollection.Forms.Task.DataBaseConfig {
    partial class frmDataBaseConfigSqlServer {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label7 = new System.Windows.Forms.Label();
            this.ddlMsSqlDataBase = new System.Windows.Forms.ComboBox();
            this.txtMsSqlUserPwd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMsSqlUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMsSqlServerAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnDataBaseTest = new System.Windows.Forms.Button();
            this.rbtnMsSqlLoginType1 = new System.Windows.Forms.RadioButton();
            this.rbtnMsSqlLoginType2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "数据库";
            // 
            // ddlMsSqlDataBase
            // 
            this.ddlMsSqlDataBase.Enabled = false;
            this.ddlMsSqlDataBase.FormattingEnabled = true;
            this.ddlMsSqlDataBase.Location = new System.Drawing.Point(69, 100);
            this.ddlMsSqlDataBase.Name = "ddlMsSqlDataBase";
            this.ddlMsSqlDataBase.Size = new System.Drawing.Size(129, 20);
            this.ddlMsSqlDataBase.TabIndex = 15;
            // 
            // txtMsSqlUserPwd
            // 
            this.txtMsSqlUserPwd.Enabled = false;
            this.txtMsSqlUserPwd.Location = new System.Drawing.Point(240, 66);
            this.txtMsSqlUserPwd.Name = "txtMsSqlUserPwd";
            this.txtMsSqlUserPwd.Size = new System.Drawing.Size(105, 21);
            this.txtMsSqlUserPwd.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(189, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "密码";
            // 
            // txtMsSqlUserName
            // 
            this.txtMsSqlUserName.Enabled = false;
            this.txtMsSqlUserName.Location = new System.Drawing.Point(69, 66);
            this.txtMsSqlUserName.Name = "txtMsSqlUserName";
            this.txtMsSqlUserName.Size = new System.Drawing.Size(100, 21);
            this.txtMsSqlUserName.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "用户名";
            // 
            // txtMsSqlServerAddress
            // 
            this.txtMsSqlServerAddress.Location = new System.Drawing.Point(69, 12);
            this.txtMsSqlServerAddress.Name = "txtMsSqlServerAddress";
            this.txtMsSqlServerAddress.Size = new System.Drawing.Size(276, 21);
            this.txtMsSqlServerAddress.TabIndex = 8;
            this.txtMsSqlServerAddress.Text = ".";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "服务器";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(270, 137);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(179, 137);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 17;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnDataBaseTest
            // 
            this.btnDataBaseTest.Location = new System.Drawing.Point(240, 98);
            this.btnDataBaseTest.Name = "btnDataBaseTest";
            this.btnDataBaseTest.Size = new System.Drawing.Size(75, 23);
            this.btnDataBaseTest.TabIndex = 19;
            this.btnDataBaseTest.Text = "测试";
            this.btnDataBaseTest.UseVisualStyleBackColor = true;
            this.btnDataBaseTest.Click += new System.EventHandler(this.btnDataBaseTest_Click);
            // 
            // rbtnMsSqlLoginType1
            // 
            this.rbtnMsSqlLoginType1.AutoSize = true;
            this.rbtnMsSqlLoginType1.Checked = true;
            this.rbtnMsSqlLoginType1.Location = new System.Drawing.Point(69, 42);
            this.rbtnMsSqlLoginType1.Name = "rbtnMsSqlLoginType1";
            this.rbtnMsSqlLoginType1.Size = new System.Drawing.Size(113, 16);
            this.rbtnMsSqlLoginType1.TabIndex = 10;
            this.rbtnMsSqlLoginType1.TabStop = true;
            this.rbtnMsSqlLoginType1.Text = "Windows身份验证";
            this.rbtnMsSqlLoginType1.UseVisualStyleBackColor = true;
            this.rbtnMsSqlLoginType1.CheckedChanged += new System.EventHandler(this.rbtnMsSqlLoginType1_CheckedChanged);
            // 
            // rbtnMsSqlLoginType2
            // 
            this.rbtnMsSqlLoginType2.AutoSize = true;
            this.rbtnMsSqlLoginType2.Location = new System.Drawing.Point(193, 43);
            this.rbtnMsSqlLoginType2.Name = "rbtnMsSqlLoginType2";
            this.rbtnMsSqlLoginType2.Size = new System.Drawing.Size(113, 16);
            this.rbtnMsSqlLoginType2.TabIndex = 9;
            this.rbtnMsSqlLoginType2.Text = "MsSqlServer验证";
            this.rbtnMsSqlLoginType2.UseVisualStyleBackColor = true;
            this.rbtnMsSqlLoginType2.CheckedChanged += new System.EventHandler(this.rbtnMsSqlLoginType2_CheckedChanged);
            // 
            // frmDataBaseConfigSqlServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 172);
            this.Controls.Add(this.btnDataBaseTest);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ddlMsSqlDataBase);
            this.Controls.Add(this.txtMsSqlUserPwd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMsSqlUserName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rbtnMsSqlLoginType2);
            this.Controls.Add(this.rbtnMsSqlLoginType1);
            this.Controls.Add(this.txtMsSqlServerAddress);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataBaseConfigSqlServer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SqlServer配置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ddlMsSqlDataBase;
        private System.Windows.Forms.TextBox txtMsSqlUserPwd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMsSqlUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMsSqlServerAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnDataBaseTest;
        private System.Windows.Forms.RadioButton rbtnMsSqlLoginType1;
        private System.Windows.Forms.RadioButton rbtnMsSqlLoginType2;
    }
}