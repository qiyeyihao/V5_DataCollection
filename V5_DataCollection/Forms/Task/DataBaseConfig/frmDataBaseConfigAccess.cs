using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V5_DataCollection.Forms.Task.DataBaseConfig {
    public partial class frmDataBaseConfigAccess : Form {

        public delegate void OutDataBaseConfig(string strDataBase);

        public OutDataBaseConfig OutDBConfig;

        public frmDataBaseConfigAccess() {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Hide();
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e) {

            string server = this.txtServer.Text.Trim();
            string user = this.txtUser.Text.Trim();
            string pass = this.txtPass.Text.Trim();

            if (string.IsNullOrEmpty(server)) {
                MessageBox.Show(this, "数据库文件不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connStr = GetConstr(server);

            if (OutDBConfig != null) {
                OutDBConfig(connStr);
            }

            this.Hide();
            this.Close();
        }

        private void chkAccessIsValied_CheckedChanged(object sender, EventArgs e) {
            if (rchkAccess.Checked) {
                this.txtUser.Enabled = true;
                this.txtPass.Enabled = true;
            }
            else {
                this.txtUser.Enabled = false;
                this.txtPass.Enabled = false;
            }
        }

        private void btnDataBaseTest_Click(object sender, EventArgs e) {
            string server = this.txtServer.Text.Trim();
            string user = this.txtUser.Text.Trim();
            string pass = this.txtPass.Text.Trim();

            if (string.IsNullOrEmpty(server)) {
                MessageBox.Show(this, "数据库文件不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connStr = GetConstr(server);
            OleDbConnection myCn = new OleDbConnection(connStr);
            try {
                this.Text = "正在连接数据库，请稍候...";
                myCn.Open();
            }
            catch {
                this.Text = "连接数据库失败！";
                MessageBox.Show(this, "连接数据库失败！请检查数据库地址或用户名密码是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            myCn.Close();
            this.Text = "连接数据库成功！";

        }

        private void btnAccessBrowsePath_Click(object sender, EventArgs e) {
            OpenFileDialog openfile = new OpenFileDialog();
            if (openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                this.txtServer.Text = openfile.FileName;
            }
        }


        private string GetConstr(string fileName) {
            string connStr = string.Empty;
            FileInfo file = new FileInfo(fileName);
            string ext = file.Extension;
            switch (ext.ToLower().Trim()) {
                case ".mdb":
                    connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Persist Security Info=False";
                    break;
                case ".accdb":
                    connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Persist Security Info=False";
                    break;
                default:
                    connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Persist Security Info=False";
                    break;
            }
            return connStr;
        }
    }
}
