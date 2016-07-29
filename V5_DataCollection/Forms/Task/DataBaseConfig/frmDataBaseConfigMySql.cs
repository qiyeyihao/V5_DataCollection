using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V5_DataCollection.Forms.Task.DataBaseConfig {
    public partial class frmDataBaseConfigMySql : Form {


        public delegate void OutDataBaseConfig(string strDataBase);

        public OutDataBaseConfig OutDBConfig;


        public frmDataBaseConfigMySql() {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e) {

            string server = this.comboBoxServer.Text.Trim();
            string user = this.txtUser.Text.Trim();
            string pass = this.txtPass.Text.Trim();
            string port = this.textBox1.Text.Trim();

            string dbname = string.Empty, constr = string.Empty;
            if ((user == "") || (server == "")) {
                MessageBox.Show(this, "服务器或用户名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.cmbDBlist.SelectedIndex > 0) {
                dbname = cmbDBlist.Text;
            }
            else {
                dbname = "mysql";
            }
            constr = String.Format("server={0};user id={1}; Port={2};password={3}; database={4}; pooling=false", server, user, port, pass, dbname);

            MySqlConnection myCn = new MySqlConnection(constr);
            try {
                this.Text = "正在连接服务器，请稍候...";
                myCn.Open();
            }
            catch (System.Exception ex) {
                this.Text = "连接服务器失败！";
                MessageBox.Show(this, "连接服务器失败！请检查服务器地址或用户名密码是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally {
                myCn.Close();
            }
            this.Text = "连接服务器成功！";

            if (OutDBConfig != null) {
                OutDBConfig(constr);
            }

            this.Hide();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Hide();
            this.Close();
        }

        private void btnDataBaseTest_Click(object sender, EventArgs e) {
            try {
                string server = this.comboBoxServer.Text.Trim();
                string user = this.txtUser.Text.Trim();
                string pass = this.txtPass.Text.Trim();
                string port = this.textBox1.Text.Trim();
                if ((user == "") || (server == "")) {
                    MessageBox.Show(this, "服务器或用户名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string constr = String.Format("server={0};uid={1}; Port={2};pwd={3}; database=mysql; pooling=false", server, user, port, pass);
                try {
                    this.Text = "正在连接服务器，请稍候...";

                    List<string> dblist = GetDBList(constr);
                    this.cmbDBlist.Enabled = true;
                    this.cmbDBlist.Items.Clear();
                    this.cmbDBlist.Items.Add("全部库");
                    if (dblist != null) {
                        if (dblist.Count > 0) {
                            foreach (string dbname in dblist) {
                                this.cmbDBlist.Items.Add(dbname);
                            }
                        }
                    }
                    this.cmbDBlist.SelectedIndex = 0;
                    this.Text = "连接服务器成功！";

                }
                catch (System.Exception ex) {
                    this.Text = "连接服务器或获取数据信息失败！";
                    MessageBox.Show(this, "连接服务器或获取数据信息失败！\r\n请检查服务器地址或用户名密码是否正确！查看帮助文件以帮助您解决问题？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    return;
                }

            }
            catch (System.Exception ex) {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public List<string> GetDBList(string connStr) {
            List<string> dblist = new List<string>();
            string strSql = "SHOW DATABASES";

            using (MySqlConnection ccc = new MySqlConnection(connStr)) {
                if (ccc.State == System.Data.ConnectionState.Closed) {
                    ccc.Open();
                }

                MySqlDataReader reader = ExecuteReader(ccc, connStr, "mysql", strSql);
                while (reader.Read()) {
                    dblist.Add(reader.GetString(0));
                }
                reader.Close();

            }
            //dblist.Sort(CompareStrByOrder);

            return dblist;
        }

        public MySqlDataReader ExecuteReader(MySqlConnection ccc, string connect, string DbName, string strSQL) {
            try {
                OpenDB(DbName, connect);

                MySqlCommand cmd = new MySqlCommand(strSQL, ccc);
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="DbName">要打开的数据库</param>
        /// <returns></returns>
        private MySqlCommand OpenDB(string DbName, string _dbconnectStr) {
            MySqlConnection connect = new MySqlConnection();
            try {
                if (connect.ConnectionString == "") {
                    connect.ConnectionString = _dbconnectStr;
                }
                if (connect.ConnectionString != _dbconnectStr) {
                    connect.Close();
                    connect.ConnectionString = _dbconnectStr;
                }
                MySqlCommand dbCommand = new MySqlCommand();
                dbCommand.Connection = connect;
                if (connect.State == System.Data.ConnectionState.Closed) {
                    connect.Open();
                }
                dbCommand.CommandText = "use " + DbName + "";
                dbCommand.ExecuteNonQuery();
                return dbCommand;

            }
            catch (System.Exception ex) {
                string str = ex.Message;
                return null;
            }

        }
    }
}
