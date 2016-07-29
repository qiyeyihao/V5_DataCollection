using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace V5_WinLibs.Security {
    class RsaHelper {
        private void btnCreate_Click(object sender, EventArgs e) {
            /*
           this.saveFileDialog.Filter = "验证文件|*.lrc|所有文件|*.*";
            if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                string PubKey = string.Empty, PiraKey = string.Empty;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                    // 公钥 
                    PubKey = rsa.ToXmlString(false);
                    // 私钥 
                    PiraKey = rsa.ToXmlString(true);

                    string inString = this.txtMachineCode.Text + "||";
                    inString += this.txtUserEmail.Text + "||";
                    inString += this.txtDeadline.Text + "||";
                    inString += this.dtpStartDateTime.Text;
                    rsa.FromXmlString(PiraKey);
                    // 加密对象 
                    RSAPKCS1SignatureFormatter f = new RSAPKCS1SignatureFormatter(rsa);
                    f.SetHashAlgorithm("SHA1");
                    byte[] source = System.Text.ASCIIEncoding.ASCII.GetBytes(inString);
                    SHA1Managed sha = new SHA1Managed();
                    byte[] result = sha.ComputeHash(source);
                    byte[] b = f.CreateSignature(result);
                    string Result = Convert.ToBase64String(b);

                    ModelLicenseItem model = new ModelLicenseItem();
                    model.MachineCode = this.txtMachineCode.Text;
                    model.UserEmail = this.txtUserEmail.Text;
                    model.Deadline = this.txtDeadline.Text;
                    model.StartDateTime = this.dtpStartDateTime.Text;
                    model.PublicKey = "<![CDATA[" + PubKey + "]]>";// PubKey;
                    model.PublicResult = "<![CDATA[" + Result + "]]>";// Result;
                    string fName = saveFileDialog.FileName;
                    ObjFileStoreHelper.SaveObj(fName, model);
                    MessageBox.Show("生成完成!");
                    this.Close();
                }
            }*/
        }

        private void toolStripButton_TestLoad_Click(object sender, EventArgs e) {
            /*
            this.openFileDialog.Filter = "验证文件|*.lrc|所有文件|*.*";
            if (this.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                ModelLicenseItem model = (ModelLicenseItem)ObjFileStoreHelper.LoadObj(this.openFileDialog.FileName);
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                    string pubkey = System.Web.HttpUtility.HtmlDecode(model.PublicKey);
                    string key123 = System.Web.HttpUtility.HtmlDecode(model.PublicResult);
                    rsa.FromXmlString(pubkey);
                    RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsa);
                    f.SetHashAlgorithm("SHA1");
                    byte[] key = Convert.FromBase64String(key123);
                    string inString = model.MachineCode + "||";
                    inString += model.UserEmail + "||";
                    inString += model.Deadline + "||";
                    inString += model.StartDateTime;
                    SHA1Managed sha = new SHA1Managed();
                    byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(inString));
                    if (f.VerifySignature(name, key))
                        MessageBox.Show("验证成功");
                    else
                        MessageBox.Show("不成功");
                }
            }*/
        }


    }
}
