using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V5_DataCollection.Forms.Task.DataBaseConfig {
    public partial class frmDataBaseConfigSqlServer : Form {

        public delegate void OutDataBaseConfig(string strDataBase);

        public OutDataBaseConfig OutDBConfig;

        public frmDataBaseConfigSqlServer() {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e) {
            string dbname = string.Empty;
            string constr = string.Empty;
            string server = this.txtMsSqlServerAddress.Text.Trim();
            string user = this.txtMsSqlUserName.Text.Trim();
            string pass = this.txtMsSqlUserPwd.Text.Trim();

            if (!string.IsNullOrEmpty(this.ddlMsSqlDataBase.Text)) {
                dbname = ddlMsSqlDataBase.Text;
            }
            else {
                dbname = "master";
            }

            if (GetSelVerified() == "Windows") {
                constr = "Integrated Security=SSPI;Data Source=" + server + ";Initial Catalog=" + dbname;
            }
            else {
                if ((user == "") || (server == "")) {
                    MessageBox.Show(this, "服务器或用户名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                constr = "Server=" + server + ";Uid=" + user + ";Pwd=" + pass + ";DataBase=" + dbname + ";";
            }
            if (OutDBConfig != null) {
                OutDBConfig(constr);
            }
            this.Hide();
            this.Close();
        }

        public string GetSelVerified() {
            if (rbtnMsSqlLoginType1.Checked) {
                return "Windows";
            }
            else {
                return "SQL";
            }
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            this.Hide();
            this.Close();
        }

        private void btnDataBaseTest_Click(object sender, EventArgs e) {
            string constr = string.Empty;
            string server = this.txtMsSqlServerAddress.Text.Trim();
            string user = this.txtMsSqlUserName.Text.Trim();
            string pass = this.txtMsSqlUserPwd.Text.Trim();

            if (GetSelVerified() == "Windows") {
                constr = "Integrated Security=SSPI;Data Source=" + server + ";Initial Catalog=master";
            }
            else {
                if ((user == "") || (server == "")) {
                    MessageBox.Show(this, "服务器或用户名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (pass == "") {
                    constr = "user id=" + user + ";initial catalog=master;data source=" + server;
                }
                else {
                    constr = "user id=" + user + ";password=" + pass + ";initial catalog=master;data source=" + server;
                }
            }
            try {
                _Class.DbObjects.SQL.DbObject db = new _Class.DbObjects.SQL.DbObject();
                db.DbConnectStr = constr;
                List<string> dblist = db.GetDBList();

                this.ddlMsSqlDataBase.Enabled = true;
                this.ddlMsSqlDataBase.Items.Clear();
                if (dblist != null) {
                    if (dblist.Count > 0) {
                        foreach (string dbname in dblist) {
                            this.ddlMsSqlDataBase.Items.Add(dbname);
                        }
                    }
                }
                this.ddlMsSqlDataBase.SelectedIndex = 0;
            }
            catch (Exception ex) {
                MessageBox.Show("如果Windows身份验证失败!请用MsSqlserver登陆!" + ex.Message);
            }
        }

        private void btnTestDBLink_Click(object sender, EventArgs e) {

        }

        private void rbtnMsSqlLoginType1_CheckedChanged(object sender, EventArgs e) {
            if (this.rbtnMsSqlLoginType1.Checked) {
                this.txtMsSqlUserName.Enabled = false;
                this.txtMsSqlUserPwd.Enabled = false;
            }
        }

        private void rbtnMsSqlLoginType2_CheckedChanged(object sender, EventArgs e) {
            if (this.rbtnMsSqlLoginType2.Checked) {
                this.txtMsSqlUserName.Enabled = true;
                this.txtMsSqlUserPwd.Enabled = true;
            }
        }
    }
}
