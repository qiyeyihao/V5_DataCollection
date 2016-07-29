using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace V5_WinLibs.Core {

    public class ObjFileStoreHelper {
        const string keyStr = "v5soft123";  //加密的KEY字符，用MD5码生成128位KEY密钥

        const string ivStr = "v5soft123";  //加密的IV字符，用MD5码生成128位IV密钥
        public ObjFileStoreHelper() { }

        /// <summary>
        /// 将对象保存为文件
        /// </summary>
        /// <param name="uFilename">文件名及地址</param>
        /// <param name="uTarget">保存对象，需[Serializable]</param>
        static public void SaveObj(string uFilename, object uTarget) {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream();
            formatter.Serialize(mStream, uTarget);
            Rijndael rijn = Rijndael.Create();
            MD5 md5 = MD5.Create();
            byte[] Key = md5.ComputeHash(Encoding.ASCII.GetBytes(keyStr));
            byte[] IV = md5.ComputeHash(Encoding.ASCII.GetBytes(ivStr));
            FileStream fStream = new FileStream(uFilename, FileMode.Create, FileAccess.Write,
                FileShare.None);
            CryptoStream cStream = new CryptoStream(fStream, rijn.CreateEncryptor(Key, IV),
                CryptoStreamMode.Write);
            StreamWriter sWriter = new StreamWriter(cStream, Encoding.ASCII);
            char[] data = Encoding.ASCII.GetChars(mStream.ToArray());
            sWriter.Write(data);
            sWriter.Flush();
            sWriter.Close();
            cStream.Close();
            fStream.Close();
            mStream.Close();
        }

        /// <summary>
        /// 从文件读取对象
        /// </summary>
        /// <param name="uFilename">文件名及地址</param>
        /// <returns>反序列化后的对象，类型为object</returns>
        static public object LoadObj(string uFilename) {
            IFormatter formatter = new BinaryFormatter();
            Rijndael rijn = Rijndael.Create();
            MD5 md5 = MD5.Create();
            byte[] Key = md5.ComputeHash(Encoding.ASCII.GetBytes(keyStr));
            byte[] IV = md5.ComputeHash(Encoding.ASCII.GetBytes(ivStr));
            FileStream fStream = new FileStream(uFilename, FileMode.Open, FileAccess.Read,
                FileShare.Read);
            CryptoStream cStream = new CryptoStream(fStream, rijn.CreateDecryptor(Key, IV),
                CryptoStreamMode.Read);
            StreamReader sReader = new StreamReader(cStream);
            string ftxt = sReader.ReadToEnd();
            byte[] bytes = Encoding.ASCII.GetBytes(ftxt);
            MemoryStream mStream = new MemoryStream(bytes);
            object result = formatter.Deserialize(mStream);
            sReader.Close();
            cStream.Close();
            fStream.Close();
            mStream.Close();
            return result;
        }



        //获取或设置对称算法的机密密钥。机密密钥既用于加密，也用于解密。为了保证对称算法的安全，必须只有发送方和接收方知道该机密密钥。
        //有效密钥大小是由特定对称算法实现指定的，密钥大小在 LegalKeySizes 中列出。
        private static byte[] DESKey = new byte[] { 11, 23, 93, 102, 72, 41, 18, 12 };
        //获取或设置对称算法的初始化向量
        private static byte[] DESIV = new byte[] { 75, 158, 46, 97, 78, 57, 17, 36 };

        public static void Serialize(object data, string filePath) {
            try {
                DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();//des加密
                FileStream fout = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                CryptoStream objcry = new CryptoStream(fout, objDes.CreateEncryptor(DESKey, DESIV), CryptoStreamMode.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objcry, data);
                objcry.Close();
                fout.Close();
            }
            catch (Exception ex) {
                //MessageBox.Show(ex.Message, "序列化", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public static object Deserialize(string filePath) {
            object data = new object();
            try {
                DESCryptoServiceProvider objdes = new DESCryptoServiceProvider();
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                CryptoStream objcry = new CryptoStream(fs, objdes.CreateDecryptor(DESKey, DESIV), CryptoStreamMode.Read);
                BinaryFormatter bf = new BinaryFormatter();
                data = (object)bf.Deserialize(objcry);
                fs.Close();
            }
            catch (Exception ex) {
                //MessageBox.Show(ex.Message, "反序列化", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return data;
        }
    }
}
