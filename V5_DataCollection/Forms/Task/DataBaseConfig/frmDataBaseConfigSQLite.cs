using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_WinLibs.DBHelper;

namespace V5_DataCollection.Forms.Task.DataBaseConfig {
    public partial class frmDataBaseConfigSQLite : Form {

        public delegate void OutDataBaseConfig(string strDataBase);

        public OutDataBaseConfig OutDBConfig;

        public frmDataBaseConfigSQLite() {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e) {
            try {
                string server = this.txtServer.Text.Trim();
                string pass = this.txtPass.Text.Trim();

                string constr = string.Empty;


                if (string.IsNullOrEmpty(server)) {
                    MessageBox.Show(this, "数据库不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this.chkAccessIsValied.Checked) {
                    constr = this.GetConstr(server, pass);
                }
                else {
                    constr = this.GetConstr(server, "");
                }

                if(SQLiteHelper.IsOpen(constr)) {
                    this.Text = "正在连接数据库，请稍候...";
                } else{
                    this.Text = "连接数据库失败！";
                    MessageBox.Show(this, "连接数据库失败！请检查数据库地址或密码是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Text = "连接数据库成功！";

                if (OutDBConfig != null) {
                    OutDBConfig(constr);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            catch (System.Exception ex) {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            this.Hide();
            this.Close();
        }

        private void chkAccessIsValied_CheckedChanged(object sender, EventArgs e) {
            if (chkAccessIsValied.Checked) {
                this.txtPass.Enabled = true;
            }
            else {
                this.txtPass.Enabled = false;
            }
        }

        private void btnAccessBrowsePath_Click(object sender, EventArgs e) {
            OpenFileDialog openfile = new OpenFileDialog();
            if (openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                this.txtServer.Text = openfile.FileName;
            }
        }


        private string GetConstr(string fileName, string password) {
            string reFileName = string.Empty;
            FileInfo file = new FileInfo(txtServer.Text);
            string ext = file.Extension;
            switch (ext.ToLower().Trim()) {
                case ".sdb":
                    reFileName = @"Data Source=" + fileName;
                    if (password.Trim() != "") {
                        reFileName += ";Password=" + password.Trim();
                    }

                    break;
                case ".s3db":
                    reFileName = @"Data Source=" + txtServer.Text;
                    if (password.Trim() != "") {
                        reFileName += ";Password=" + password.Trim();
                    }
                    break;
                default:
                    reFileName = @"Data Source=" + txtServer.Text;
                    if (password.Trim() != "") {
                        reFileName += ";Password=" + password.Trim();
                    }
                    break;
            }
            return reFileName;
        }

        private void btnDataBaseTest_Click(object sender, EventArgs e) {
            string server = this.txtServer.Text.Trim();
            string pass = this.txtPass.Text.Trim();

            string constr = string.Empty;


            if (string.IsNullOrEmpty(server)) {
                MessageBox.Show(this, "数据库不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.chkAccessIsValied.Checked) {
                constr = this.GetConstr(server, pass);
            }
            else {
                constr = this.GetConstr(server, "");
            }

            if (SQLiteHelper.IsOpen(constr)) {
                this.Text = "正在连接数据库，请稍候...";
            }
            else {
                this.Text = "连接数据库失败！";
                MessageBox.Show(this, "连接数据库失败！请检查数据库地址或密码是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Text = "连接数据库成功！";
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Hide();
            this.Close();
        }
    }
}
