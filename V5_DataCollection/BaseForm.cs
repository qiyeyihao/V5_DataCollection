using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V5_DataCollection {
    public partial class BaseForm : Form {
        public frmLoadingDialog loadingDialog = null;
        public BaseForm() {
            InitializeComponent();
        }
    }
}
