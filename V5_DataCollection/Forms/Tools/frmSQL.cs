using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_DataCollection._Class.Common;
using V5_WinLibs.DBHelper;

namespace V5_DataCollection.Forms.Tools {
    public partial class frmSQL : Form {
        public frmSQL() {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e) {
            DataSet ds = SQLiteHelper.Query1(CommonHelper.SQLiteConnectionString,this.txtSQL.Text);
            this.dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }
    }
}
