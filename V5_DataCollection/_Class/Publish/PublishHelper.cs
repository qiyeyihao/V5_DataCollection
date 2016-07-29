using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_Model;
using System.IO;
using V5_WinLibs;
using System.Data;
using V5_DataCollection._Class.Common;
using System.Windows.Forms;
using V5_DataCollection._Class.Gather;
using V5_DataCollection.Forms.Publish;
using System.Xml.Serialization;
using V5_DataCollection._Class.DAL;
using V5_WinLibs.DBHelper;
using V5_Utility.Utility;
using V5_WinLibs.Utility;
using V5_WinLibs.DBUtility;
using V5_WinLibs.Core;

namespace V5_DataCollection._Class.Publish {
    /// <summary>
    /// 发布管理
    /// </summary>
    public class PublishHelper {

        #region 属性访问器
        private ModelTask model = new ModelTask();
        /// <summary>
        /// 任务模型
        /// </summary>
        public ModelTask ModelTask {
            get { return model; }
            set { model = value; }
        }
        #endregion

        string modulePath = AppDomain.CurrentDomain.BaseDirectory + "/Modules/";
        public GatherEventHandler.GatherWorkHandler PublishCompalteDelegate;
        private ThreadMultiHelper thPublishData;
        private GatherEvents.GatherLinkEvents gatherEv = new GatherEvents.GatherLinkEvents();

        public PublishHelper() {

        }

        ~PublishHelper() {
            ModelTask = null;
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="args"></param>
        public void Start(object args) {
            if (StringHelper.Instance.SetNumber(ModelTask.IsWebOnlinePublish1) == 1) {
                gatherEv.Message = "开始发布Web数据!";
                PublishCompalteDelegate(this, gatherEv);
                thPublishData = new ThreadMultiHelper(1);
                thPublishData.WorkMethod += PublishDataToWeb;
                thPublishData.CompleteEvent += PublishData;
                thPublishData.Start();
            }

            if (StringHelper.Instance.SetNumber(ModelTask.IsSaveLocal2) == 1) {
                gatherEv.Message = "开始发布本地数据!";
                PublishCompalteDelegate(this, gatherEv);
                thPublishData = new ThreadMultiHelper(1);
                thPublishData.WorkMethod += PublishDataToLocal;
                thPublishData.CompleteEvent += PublishData;
                thPublishData.Start();
            }

            if (StringHelper.Instance.SetNumber(ModelTask.IsSaveDataBase3) == 1) {
                gatherEv.Message = "开始保存到数据库!";
                PublishCompalteDelegate(this, gatherEv);
                thPublishData = new ThreadMultiHelper(1);
                thPublishData.WorkMethod += PublishDataToDB;
                thPublishData.CompleteEvent += PublishData;
                thPublishData.Start();
            }

            if (StringHelper.Instance.SetNumber(ModelTask.IsSaveSQL4) == 1) {
                gatherEv.Message = "开始发布自定义网站!";
                if (PublishCompalteDelegate != null)
                    PublishCompalteDelegate(this, gatherEv);
                thPublishData = new ThreadMultiHelper(1);
                thPublishData.WorkMethod += PublishDataToSQL;
                thPublishData.CompleteEvent += PublishData;
                thPublishData.Start();
            }
        }

        private string LoginedCookies = string.Empty, LoginPostData = string.Empty;
        cPublishModuleItem mPublishModuleItem = new cPublishModuleItem();

        private void PublishData() {
            if (PublishCompalteDelegate != null) {
                gatherEv.Message = "数据发布成功!";
                PublishCompalteDelegate(this, gatherEv);
            }
        }

        #region 1.Web发布
        private void PublishDataToWeb(int taskindex, int threadindex) {
            foreach (ModelWebPublishModule mWebPublishModule in ModelTask.ListModelWebPublishModule) {
                mPublishModuleItem = GetModelXml(mWebPublishModule.ModuleNameFile);
                DataToWeb(mWebPublishModule);
            }
        }
        private void DataToWeb(ModelWebPublishModule m) {
            if (m.IsCookiesLogin == 1) {
                LoginedCookies = m.CookiesValue;
                ViewCMS(m);
            }
        }
        private void ViewCMS(ModelWebPublishModule m) {
            LoginPostData = mPublishModuleItem.ViewPostData;
            string result = SimulationHelper.PostPage(m.SiteUrl + mPublishModuleItem.ViewUrl,
                "",
                m.SiteUrl + mPublishModuleItem.ViewRefUrl,
                mPublishModuleItem.PageEncode,
                ref LoginedCookies);
            //
        }
        public cPublishModuleItem GetModelXml(string pathName) {
            cPublishModuleItem model = new cPublishModuleItem();
            XmlSerializer serializer = new XmlSerializer(typeof(cPublishModuleItem));
            try {
                string fileName = modulePath + pathName;
                using (FileStream fs = new FileStream(fileName, FileMode.Open)) {
                    model = (cPublishModuleItem)serializer.Deserialize(fs);
                    fs.Close();
                }
            }
            catch {

            }
            return model;
        }
        #endregion

        #region 2.保存本地
        private void PublishDataToLocal(int taskindex, int threadindex) {
            try {
                string str = string.Empty;
                string LocalSQLiteName = "Data\\Collection\\" + ModelTask.TaskName + "\\SpiderResult.db";
                DataTable dtData = SQLiteHelper.Query1(LocalSQLiteName, "Select * From Content").Tables[0];
                if (!Directory.Exists(ModelTask.SaveDirectory2))
                    Directory.CreateDirectory(ModelTask.SaveDirectory2);
                if (ModelTask.SaveFileFormat2.ToLower() == ".html") {
                    foreach (DataRow dr in dtData.Rows) {
                        string fileName = dr["标题"].ToString();
                        str = dr["内容"].ToString();
                        try {
                            fileName = fileName.Replace(".", "");
                            fileName = fileName.Replace(",", "");
                            fileName = fileName.Replace("、", "");
                            fileName = fileName.Replace(" ", "");
                            fileName = fileName.Replace("*", "_");
                            fileName = fileName.Replace("?", "_");
                            fileName = fileName.Replace("/", "_");
                            fileName = fileName.Replace("\\", "_");
                            fileName = fileName.Replace(":", "_");
                            fileName = fileName.Replace("|", "_");
                            fileName += ".html";
                            using (StreamWriter sw = new StreamWriter(ModelTask.SaveDirectory2 + "\\" + fileName, false, Encoding.UTF8)) {
                                sw.Write(str);
                                sw.Flush();
                                sw.Close();
                            }
                        }
                        catch {
                            continue;
                        }
                    }
                }
                else if (ModelTask.SaveFileFormat2.ToLower() == ".txt") {
                    foreach (DataRow dr in dtData.Rows) {
                        foreach (ModelTaskLabel mTaskLabel in ModelTask.ListTaskLabel) {
                            str += mTaskLabel.LabelName + ":" + dr[mTaskLabel.LabelName] + "\r\n";
                        }
                        str += "\r\n\r\n";
                    }
                    using (StreamWriter sw = new StreamWriter(ModelTask.SaveDirectory2 + "\\采集结果文本保存.txt", false, Encoding.UTF8)) {
                        sw.Write(str);
                        sw.Flush();
                        sw.Close();
                    }
                }
                else if (ModelTask.SaveFileFormat2.ToLower() == ".sql") {
                    try {
                        string strTemplateContent = File.ReadAllText(ModelTask.SaveHtmlTemplate2, Encoding.UTF8);
                        StringBuilder sbContent = new StringBuilder();
                        foreach (DataRow dr in dtData.Rows) {
                            string sql = strTemplateContent;
                            foreach (ModelTaskLabel mTaskLabel in ModelTask.ListTaskLabel) {
                                string content = dr[mTaskLabel.LabelName].ToString().Replace("'", "''");
                                sql = sql.Replace("[" + mTaskLabel.LabelName + "]", content);
                            }
                            sbContent.AppendLine(sql);
                        }
                        using (StreamWriter sw = new StreamWriter(ModelTask.SaveDirectory2 + "\\sql.sql", false, Encoding.UTF8)) {
                            sw.Write(sbContent.ToString());
                            sw.Flush();
                            sw.Close();
                        }
                    }
                    catch (Exception ex) {
                        gatherEv.Message = "错误!" + ex.Message;
                        PublishCompalteDelegate(this, gatherEv);
                    }
                }
            }
            catch (Exception ex) {
                Log4Helper.Write(V5_Utility.Utility.LogLevel.Error, ex);
            }
        }
        #endregion

        #region 3.保存到数据库
        private void PublishDataToDB(int taskindex, int threadindex) {
            string LocalSQLiteName = "Data\\Collection\\" + ModelTask.TaskName + "\\SpiderResult.db";
            DataTable dtData = SQLiteHelper.Query1(LocalSQLiteName, "Select * From Content").Tables[0];

            int saveDateType = ModelTask.SaveDataType3.Value;
            string connectionString = ModelTask.SaveDataUrl3;
            string exeSQL = ModelTask.SaveDataSQL3;
            string sql = string.Empty;
            switch (saveDateType) {
                case 1:
                    break;
                case 2:
                    foreach (DataRow dr in dtData.Rows) {
                        try {
                            sql = exeSQL;
                            foreach (ModelTaskLabel mTaskLabel in ModelTask.ListTaskLabel) {
                                sql = sql.Replace("[" + mTaskLabel.LabelName + "]", dr[mTaskLabel.LabelName].ToString().Replace("'", "''"));
                            }
                            sql = sql.Replace("[Guid]", Guid.NewGuid().ToString());
                            sql = sql.Replace("[Url]", dr["HrefSource"].ToString());
                            DbHelperSQL.connectionString = ModelTask.SaveDataUrl3;
                            DbHelperSQL.ExecuteSql(sql);
                            gatherEv.Message = dr["HrefSource"].ToString() + ":保存数据库成功!";
                            PublishCompalteDelegate(this, gatherEv);
                        }
                        catch (Exception ex) {
                            Log4Helper.Write(V5_Utility.Utility.LogLevel.Error, dr["HrefSource"].ToString() + ":保存数据库失败!", ex);
                            continue;
                        }
                    }
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
        #endregion

        #region 4.发布到自定义Web网站
        private void PublishDataToSQL(int taskindex, int threadindex) {
            string LocalSQLiteName = "Data\\Collection\\" + ModelTask.TaskName + "\\SpiderResult.db";
            DataTable dtData = SQLiteHelper.Query1(LocalSQLiteName, "Select * From Content").Tables[0];

            var listDiyUrl = DALDiyWebUrlHelper.GetList(" And SelfId=" + ModelTask.ID, "", 0);
            HttpHelper4 http = new HttpHelper4();
            int taskId = ModelTask.ID;
            foreach (DataRow dr in dtData.Rows) {
                int resultId = int.Parse(dr["Id"].ToString());
                foreach (var m in listDiyUrl) {
                    try {

                        string getUrl = m.Url;
                        string postParams = m.UrlParams;
                        StringBuilder sbContent = new StringBuilder();
                        foreach (ModelTaskLabel mTaskLabel in ModelTask.ListTaskLabel) {
                            string pageEncodeContent = dr[mTaskLabel.LabelName].ToString().Replace("'", "''");
                            //可能需要编码实际测试才知道
                            getUrl = getUrl.Replace("[" + mTaskLabel.LabelName + "]", pageEncodeContent);
                            postParams = postParams.Replace("[" + mTaskLabel.LabelName + "]", pageEncodeContent);
                            sbContent.Append(pageEncodeContent);
                        }
                        string md5key = StringHelper.Instance.MD5(taskId.ToString() + resultId.ToString() + sbContent.ToString(), 32).ToLower();
                        //判断该条记录这个weburl是否发过
                        if (!DALDataPublishLogHelper.ChkRecord(
                            ModelTask.ID, resultId, md5key)) {
                            //记录日志
                            DALDataPublishLogHelper.Insert(new ModelDataPublishLog() {
                                TaskId = taskId,
                                ResultId = resultId,
                                DesKey = md5key,
                                CreateTime = DateTime.Now.ToString()
                            });
                        }
                        else {
                            continue;
                        }

                        //开始发布网站
                        var result = http.GetHtml(new HttpItem() {
                            URL = getUrl,
                            Postdata = postParams,
                            ContentType = "application/x-www-form-urlencoded"
                        });
                        var html = result.Html;
                    }
                    catch (Exception ex) {
                        continue;
                    }
                }
            }
            if (PublishCompalteDelegate != null) {
                gatherEv.Message = "发布到自定义Web网站完成!";
                PublishCompalteDelegate(this, gatherEv);
            }
        }
        #endregion

    }
}
