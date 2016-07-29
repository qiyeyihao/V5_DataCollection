using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V5_DataCollection.Forms.Task.DataBaseConfig {
    public partial class frmDataBaseConfigOracle : Form {

        public delegate void OutDataBaseConfig(string strDataBase);

        public OutDataBaseConfig OutDBConfig;

        public frmDataBaseConfigOracle() {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e) {


            try {
                string user = this.txtUser.Text.Trim();
                string pass = this.txtPass.Text.Trim();
                string server = this.txtServer.Text.Trim();

                if ((user.Trim() == "") || (server.Trim() == "")) {
                    MessageBox.Show(this, "用户名或密码不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string constr = "Data Source=" + server + "; user id=" + user + ";password=" + pass;

                OracleConnection myCn = new OracleConnection(constr);
                try {
                    this.Text = "正在连接服务器，请稍候...";
                    myCn.Open();
                }
                catch {
                    this.Text = "连接服务器失败！";
                    myCn.Close();
                    MessageBox.Show(this, "连接服务器失败！请检查服务器地址或用户名密码是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                myCn.Close();
                this.Text = "连接服务器成功！";

                if (OutDBConfig != null) {
                    OutDBConfig(constr);
                }

                this.Hide();
                this.Close();
            }
            catch (System.Exception ex) {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Hide();
            this.Close();
        }
    }
}
