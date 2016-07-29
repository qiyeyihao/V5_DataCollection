using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace V5_DataCollection.Forms.Task {
    public class TaskIndexRegex {
        public static void CreateRegexXml(ModelTaskIndexRegex model, string fileNameNotExt) {
            try {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\IndexFiles\\" + fileNameNotExt + ".Config";
                if (!File.Exists(fileName)) {
                    File.Create(fileName);
                }
                XmlSerializer serializer = new XmlSerializer(typeof(ModelTaskIndexRegex));
                FileStream fs = new FileStream(fileName, FileMode.Create);
                serializer.Serialize(fs, model);
                fs.Close();
            }
            catch (Exception) {

            }
        }
    }
}
