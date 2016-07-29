using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_Model;
using System.Data;
using V5_DataCollection._Class.Common;
using V5_WinLibs.DBHelper;

namespace V5_DataCollection._Class.DAL {
    public class DALTask {

        string dbStr = CommonHelper.SQLiteConnectionString;
        public DALTask() {
        }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId() {
            object obj = SQLiteHelper.ExecuteScalar(dbStr, "select max(id)+1 from S_Task");
            if (obj == null) {
                return 1;
            }
            else {
                return int.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ModelTask model) {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.ID != null) {
                strSql1.Append("ID,");
                strSql2.Append("" + model.ID + ",");
            }
            if (model.Status != null) {
                strSql1.Append("Status,");
                strSql2.Append("" + model.Status + ",");
            }
            if (model.TaskClassID != null) {
                strSql1.Append("TaskClassID,");
                strSql2.Append("" + model.TaskClassID + ",");
            }
            if (model.TaskName != null) {
                strSql1.Append("TaskName,");
                strSql2.Append("'" + model.TaskName + "',");
            }
            if (model.IsSpiderUrl != null) {
                strSql1.Append("IsSpiderUrl,");
                strSql2.Append("" + model.IsSpiderUrl + ",");
            }
            if (model.IsSpiderContent != null) {
                strSql1.Append("IsSpiderContent,");
                strSql2.Append("" + model.IsSpiderContent + ",");
            }
            if (model.IsPublishContent != null) {
                strSql1.Append("IsPublishContent,");
                strSql2.Append("" + model.IsPublishContent + ",");
            }
            if (model.PageEncode != null) {
                strSql1.Append("PageEncode,");
                strSql2.Append("'" + model.PageEncode + "',");
            }
            if (model.CollectionType != null) {
                strSql1.Append("CollectionType,");
                strSql2.Append("" + model.CollectionType + ",");
            }
            if (model.CollectionContent != null) {
                strSql1.Append("CollectionContent,");
                strSql2.Append("'" + model.CollectionContent + "',");
            }
            if (model.LinkUrlMustIncludeStr != null) {
                strSql1.Append("LinkUrlMustIncludeStr,");
                strSql2.Append("'" + model.LinkUrlMustIncludeStr + "',");
            }
            if (model.LinkUrlNoMustIncludeStr != null) {
                strSql1.Append("LinkUrlNoMustIncludeStr,");
                strSql2.Append("'" + model.LinkUrlNoMustIncludeStr + "',");
            }
            if (model.LinkUrlCutAreaStart != null) {
                strSql1.Append("LinkUrlCutAreaStart,");
                strSql2.Append("'" + model.LinkUrlCutAreaStart + "',");
            }
            if (model.LinkUrlCutAreaEnd != null) {
                strSql1.Append("LinkUrlCutAreaEnd,");
                strSql2.Append("'" + model.LinkUrlCutAreaEnd + "',");
            }
            if (model.TestViewUrl != null) {
                strSql1.Append("TestViewUrl,");
                strSql2.Append("'" + model.TestViewUrl + "',");
            }
            if (model.IsWebOnlinePublish1 != null) {
                strSql1.Append("IsWebOnlinePublish1,");
                strSql2.Append("" + model.IsWebOnlinePublish1 + ",");
            }
            if (model.IsSaveLocal2 != null) {
                strSql1.Append("IsSaveLocal2,");
                strSql2.Append("" + model.IsSaveLocal2 + ",");
            }
            if (model.SaveFileFormat2 != null) {
                strSql1.Append("SaveFileFormat2,");
                strSql2.Append("'" + model.SaveFileFormat2 + "',");
            }
            if (model.SaveDirectory2 != null) {
                strSql1.Append("SaveDirectory2,");
                strSql2.Append("'" + model.SaveDirectory2 + "',");
            }
            if (model.SaveHtmlTemplate2 != null) {
                strSql1.Append("SaveHtmlTemplate2,");
                strSql2.Append("'" + model.SaveHtmlTemplate2 + "',");
            }
            if (model.SaveIsCreateIndex2 != null) {
                strSql1.Append("SaveIsCreateIndex2,");
                strSql2.Append("" + model.SaveIsCreateIndex2 + ",");
            }
            if (model.IsSaveDataBase3 != null) {
                strSql1.Append("IsSaveDataBase3,");
                strSql2.Append("" + model.IsSaveDataBase3 + ",");
            }
            if (model.SaveDataType3 != null) {
                strSql1.Append("SaveDataType3,");
                strSql2.Append("" + model.SaveDataType3 + ",");
            }
            if (model.SaveDataUrl3 != null) {
                strSql1.Append("SaveDataUrl3,");
                strSql2.Append("'" + model.SaveDataUrl3 + "',");
            }
            if (model.SaveDataSQL3 != null) {
                strSql1.Append("SaveDataSQL3,");
                strSql2.Append("'" + model.SaveDataSQL3 + "',");
            }
            if (model.IsSaveSQL4 != null) {
                strSql1.Append("IsSaveSQL4,");
                strSql2.Append("" + model.IsSaveSQL4 + ",");
            }
            if (model.SaveSQLContent4 != null) {
                strSql1.Append("SaveSQLContent4,");
                strSql2.Append("'" + model.SaveSQLContent4 + "',");
            }
            if (model.SaveSQLDirectory4 != null) {
                strSql1.Append("SaveSQLDirectory4,");
                strSql2.Append("'" + model.SaveSQLDirectory4 + "',");
            }
            if (model.SaveSQLDirectory4 != null) {
                strSql1.Append("Guid,");
                strSql2.Append("'" + model.Guid + "',");
            }
            //
            if (model.PluginSpiderUrl != null) {
                strSql1.Append("PluginSpiderUrl,");
                strSql2.Append("'" + model.PluginSpiderUrl + "',");
            }
            if (model.PluginSpiderContent != null) {
                strSql1.Append("PluginSpiderContent,");
                strSql2.Append("'" + model.PluginSpiderContent + "',");
            }
            if (model.PluginSaveContent != null) {
                strSql1.Append("PluginSaveContent,");
                strSql2.Append("'" + model.PluginSaveContent + "',");
            }
            if (model.PluginPublishContent != null) {
                strSql1.Append("PluginPublishContent,");
                strSql2.Append("'" + model.PluginPublishContent + "',");
            }
            //======================================2012 2-6
            if (model.CollectionContentThreadCount != null) {
                strSql1.Append("CollectionContentThreadCount,");
                strSql2.Append("" + model.CollectionContentThreadCount + ",");
            }
            if (model.CollectionContentStepTime != null) {
                strSql1.Append("CollectionContentStepTime,");
                strSql2.Append("" + model.CollectionContentStepTime + ",");
            }
            if (model.PublishContentThreadCount != null) {
                strSql1.Append("PublishContentThreadCount,");
                strSql2.Append("" + model.PublishContentThreadCount + ",");
            }
            if (model.PublishContentStepTimeMin != null) {
                strSql1.Append("PublishContentStepTimeMin,");
                strSql2.Append("" + model.PublishContentStepTimeMin + ",");
            }
            if (model.PublishContentStepTimeMax != null) {
                strSql1.Append("PublishContentStepTimeMax,");
                strSql2.Append("" + model.PublishContentStepTimeMax + ",");
            }
            if (model.IsHandGetUrl != null) {
                strSql1.Append("IsHandGetUrl,");
                strSql2.Append("" + model.IsHandGetUrl + ",");
            }
            if (model.HandCollectionUrlRegex != null) {
                strSql1.Append("HandCollectionUrlRegex,");
                strSql2.Append("'" + model.HandCollectionUrlRegex + "',");
            }
            if (model.DemoListUrl != null) {
                strSql1.Append("DemoListUrl,");
                strSql2.Append("'" + model.DemoListUrl + "',");
            }
            //
            if (model.IsPlan != null) {
                strSql1.Append("IsPlan,");
                strSql2.Append("" + model.IsPlan + ",");
            }
            if (model.PlanFormat != null) {
                strSql1.Append("PlanFormat,");
                strSql2.Append("'" + model.PlanFormat + "',");
            }
            strSql.Append("insert into S_Task(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            SQLiteHelper.Execute(dbStr, strSql.ToString());
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ModelTask model) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_Task set ");
            if (model.Status != null) {
                strSql.Append("Status=" + model.Status + ",");
            }
            if (model.TaskClassID != null) {
                strSql.Append("TaskClassID=" + model.TaskClassID + ",");
            }
            if (model.TaskName != null) {
                strSql.Append("TaskName='" + model.TaskName + "',");
            }
            if (model.IsSpiderUrl != null) {
                strSql.Append("IsSpiderUrl=" + model.IsSpiderUrl + ",");
            }
            if (model.IsSpiderContent != null) {
                strSql.Append("IsSpiderContent=" + model.IsSpiderContent + ",");
            }
            if (model.IsPublishContent != null) {
                strSql.Append("IsPublishContent=" + model.IsPublishContent + ",");
            }
            if (model.PageEncode != null) {
                strSql.Append("PageEncode='" + model.PageEncode + "',");
            }
            if (model.CollectionType != null) {
                strSql.Append("CollectionType=" + model.CollectionType + ",");
            }
            if (model.CollectionContent != null) {
                strSql.Append("CollectionContent='" + model.CollectionContent + "',");
            }
            if (model.LinkUrlMustIncludeStr != null) {
                strSql.Append("LinkUrlMustIncludeStr='" + model.LinkUrlMustIncludeStr + "',");
            }
            if (model.LinkUrlNoMustIncludeStr != null) {
                strSql.Append("LinkUrlNoMustIncludeStr='" + model.LinkUrlNoMustIncludeStr + "',");
            }
            if (model.LinkUrlCutAreaStart != null) {
                strSql.Append("LinkUrlCutAreaStart='" + model.LinkUrlCutAreaStart + "',");
            }
            if (model.LinkUrlCutAreaEnd != null) {
                strSql.Append("LinkUrlCutAreaEnd='" + model.LinkUrlCutAreaEnd + "',");
            }
            if (model.TestViewUrl != null) {
                strSql.Append("TestViewUrl='" + model.TestViewUrl + "',");
            }
            if (model.IsWebOnlinePublish1 != null) {
                strSql.Append("IsWebOnlinePublish1=" + model.IsWebOnlinePublish1 + ",");
            }
            if (model.IsSaveLocal2 != null) {
                strSql.Append("IsSaveLocal2=" + model.IsSaveLocal2 + ",");
            }
            if (model.SaveFileFormat2 != null) {
                strSql.Append("SaveFileFormat2='" + model.SaveFileFormat2 + "',");
            }
            if (model.SaveDirectory2 != null) {
                strSql.Append("SaveDirectory2='" + model.SaveDirectory2 + "',");
            }
            if (model.SaveHtmlTemplate2 != null) {
                strSql.Append("SaveHtmlTemplate2='" + model.SaveHtmlTemplate2 + "',");
            }
            if (model.SaveIsCreateIndex2 != null) {
                strSql.Append("SaveIsCreateIndex2=" + model.SaveIsCreateIndex2 + ",");
            }
            if (model.IsSaveDataBase3 != null) {
                strSql.Append("IsSaveDataBase3=" + model.IsSaveDataBase3 + ",");
            }
            if (model.SaveDataType3 != null) {
                strSql.Append("SaveDataType3=" + model.SaveDataType3 + ",");
            }
            if (model.SaveDataUrl3 != null) {
                strSql.Append("SaveDataUrl3='" + model.SaveDataUrl3 + "',");
            }
            if (model.SaveDataSQL3 != null) {
                strSql.Append("SaveDataSQL3='" + model.SaveDataSQL3 + "',");
            }
            if (model.IsSaveSQL4 != null) {
                strSql.Append("IsSaveSQL4=" + model.IsSaveSQL4 + ",");
            }
            if (model.SaveSQLContent4 != null) {
                strSql.Append("SaveSQLContent4='" + model.SaveSQLContent4 + "',");
            }
            if (model.SaveSQLDirectory4 != null) {
                strSql.Append("SaveSQLDirectory4='" + model.SaveSQLDirectory4 + "',");
            }
            if (model.Guid != null) {
                strSql.Append("Guid='" + model.Guid + "',");
            }
            //
            if (model.PluginSpiderUrl != null) {
                strSql.Append("PluginSpiderUrl='" + model.PluginSpiderUrl + "',");
            }
            if (model.PluginSpiderContent != null) {
                strSql.Append("PluginSpiderContent='" + model.PluginSpiderContent + "',");
            }
            if (model.PluginSaveContent != null) {
                strSql.Append("PluginSaveContent='" + model.PluginSaveContent + "',");
            }
            if (model.PluginPublishContent != null) {
                strSql.Append("PluginPublishContent='" + model.PluginPublishContent + "',");
            }
            //============================================2012 2-6
            if (model.CollectionContentThreadCount != null) {
                strSql.Append("CollectionContentThreadCount=" + model.CollectionContentThreadCount + ",");
            }
            if (model.CollectionContentStepTime != null) {
                strSql.Append("CollectionContentStepTime=" + model.CollectionContentStepTime + ",");
            }
            if (model.PublishContentThreadCount != null) {
                strSql.Append("PublishContentThreadCount=" + model.PublishContentThreadCount + ",");
            }
            if (model.PublishContentStepTimeMin != null) {
                strSql.Append("PublishContentStepTimeMin=" + model.PublishContentStepTimeMin + ",");
            }
            if (model.PublishContentStepTimeMax != null) {
                strSql.Append("PublishContentStepTimeMax=" + model.PublishContentStepTimeMax + ",");
            }
            if (model.IsHandGetUrl != null) {
                strSql.Append("IsHandGetUrl=" + model.IsHandGetUrl + ",");
            }
            if (model.HandCollectionUrlRegex != null) {
                strSql.Append("HandCollectionUrlRegex='" + model.HandCollectionUrlRegex + "',");
            }
            if (model.DemoListUrl != null) {
                strSql.Append("DemoListUrl='" + model.DemoListUrl + "',");
            }
            //
            if (model.IsPlan != null) {
                strSql.Append("IsPlan=" + model.IsPlan + ",");
            }
            if (model.PlanFormat != null) {
                strSql.Append("PlanFormat='" + model.PlanFormat + "',");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" where ID=" + model.ID + " ");
            int rowsAffected = SQLiteHelper.Execute(dbStr, strSql.ToString());
            if (rowsAffected > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from S_Task ");
            strSql.Append(" where ID=" + ID + " ");
            int rowsAffected = SQLiteHelper.Execute(dbStr, strSql.ToString());
            if (rowsAffected > 0) {
                return true;
            }
            else {
                return false;
            }
        }       /// <summary>
                /// 删除一条数据
                /// </summary>
        public bool DeleteList(string IDlist) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from S_Task ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = SQLiteHelper.Execute(dbStr, strSql.ToString());
            if (rows > 0) {
                return true;
            }
            else {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ModelTask GetModel(int ID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            strSql.Append(" * ");
            strSql.Append(" from S_Task ");
            strSql.Append(" where ID=" + ID + " ");
            ModelTask model = new ModelTask();
            DataSet ds = SQLiteHelper.Query1(dbStr, strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0) {
                if (ds.Tables[0].Rows[0]["ID"] != null && ds.Tables[0].Rows[0]["ID"].ToString() != "") {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TaskClassID"] != null && ds.Tables[0].Rows[0]["TaskClassID"].ToString() != "") {
                    model.TaskClassID = int.Parse(ds.Tables[0].Rows[0]["TaskClassID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TaskName"] != null && ds.Tables[0].Rows[0]["TaskName"].ToString() != "") {
                    model.TaskName = ds.Tables[0].Rows[0]["TaskName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["IsSpiderUrl"] != null && ds.Tables[0].Rows[0]["IsSpiderUrl"].ToString() != "") {
                    model.IsSpiderUrl = int.Parse(ds.Tables[0].Rows[0]["IsSpiderUrl"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsSpiderContent"] != null && ds.Tables[0].Rows[0]["IsSpiderContent"].ToString() != "") {
                    model.IsSpiderContent = int.Parse(ds.Tables[0].Rows[0]["IsSpiderContent"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsPublishContent"] != null && ds.Tables[0].Rows[0]["IsPublishContent"].ToString() != "") {
                    model.IsPublishContent = int.Parse(ds.Tables[0].Rows[0]["IsPublishContent"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PageEncode"] != null && ds.Tables[0].Rows[0]["PageEncode"].ToString() != "") {
                    model.PageEncode = ds.Tables[0].Rows[0]["PageEncode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CollectionType"] != null && ds.Tables[0].Rows[0]["CollectionType"].ToString() != "") {
                    model.CollectionType = int.Parse(ds.Tables[0].Rows[0]["CollectionType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CollectionContent"] != null && ds.Tables[0].Rows[0]["CollectionContent"].ToString() != "") {
                    model.CollectionContent = ds.Tables[0].Rows[0]["CollectionContent"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LinkUrlMustIncludeStr"] != null && ds.Tables[0].Rows[0]["LinkUrlMustIncludeStr"].ToString() != "") {
                    model.LinkUrlMustIncludeStr = ds.Tables[0].Rows[0]["LinkUrlMustIncludeStr"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LinkUrlNoMustIncludeStr"] != null && ds.Tables[0].Rows[0]["LinkUrlNoMustIncludeStr"].ToString() != "") {
                    model.LinkUrlNoMustIncludeStr = ds.Tables[0].Rows[0]["LinkUrlNoMustIncludeStr"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LinkUrlCutAreaStart"] != null && ds.Tables[0].Rows[0]["LinkUrlCutAreaStart"].ToString() != "") {
                    model.LinkUrlCutAreaStart = ds.Tables[0].Rows[0]["LinkUrlCutAreaStart"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LinkUrlCutAreaEnd"] != null && ds.Tables[0].Rows[0]["LinkUrlCutAreaEnd"].ToString() != "") {
                    model.LinkUrlCutAreaEnd = ds.Tables[0].Rows[0]["LinkUrlCutAreaEnd"].ToString();
                }
                if (ds.Tables[0].Rows[0]["TestViewUrl"] != null && ds.Tables[0].Rows[0]["TestViewUrl"].ToString() != "") {
                    model.TestViewUrl = ds.Tables[0].Rows[0]["TestViewUrl"].ToString();
                }
                if (ds.Tables[0].Rows[0]["IsWebOnlinePublish1"] != null && ds.Tables[0].Rows[0]["IsWebOnlinePublish1"].ToString() != "") {
                    model.IsWebOnlinePublish1 = int.Parse(ds.Tables[0].Rows[0]["IsWebOnlinePublish1"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsSaveLocal2"] != null && ds.Tables[0].Rows[0]["IsSaveLocal2"].ToString() != "") {
                    model.IsSaveLocal2 = int.Parse(ds.Tables[0].Rows[0]["IsSaveLocal2"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SaveFileFormat2"] != null && ds.Tables[0].Rows[0]["SaveFileFormat2"].ToString() != "") {
                    model.SaveFileFormat2 = ds.Tables[0].Rows[0]["SaveFileFormat2"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SaveDirectory2"] != null && ds.Tables[0].Rows[0]["SaveDirectory2"].ToString() != "") {
                    model.SaveDirectory2 = ds.Tables[0].Rows[0]["SaveDirectory2"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SaveHtmlTemplate2"] != null && ds.Tables[0].Rows[0]["SaveHtmlTemplate2"].ToString() != "") {
                    model.SaveHtmlTemplate2 = ds.Tables[0].Rows[0]["SaveHtmlTemplate2"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SaveIsCreateIndex2"] != null && ds.Tables[0].Rows[0]["SaveIsCreateIndex2"].ToString() != "") {
                    model.SaveIsCreateIndex2 = int.Parse(ds.Tables[0].Rows[0]["SaveIsCreateIndex2"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsSaveDataBase3"] != null && ds.Tables[0].Rows[0]["IsSaveDataBase3"].ToString() != "") {
                    model.IsSaveDataBase3 = int.Parse(ds.Tables[0].Rows[0]["IsSaveDataBase3"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SaveDataType3"] != null && ds.Tables[0].Rows[0]["SaveDataType3"].ToString() != "") {
                    model.SaveDataType3 = int.Parse(ds.Tables[0].Rows[0]["SaveDataType3"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SaveDataUrl3"] != null && ds.Tables[0].Rows[0]["SaveDataUrl3"].ToString() != "") {
                    model.SaveDataUrl3 = ds.Tables[0].Rows[0]["SaveDataUrl3"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SaveDataSQL3"] != null && ds.Tables[0].Rows[0]["SaveDataSQL3"].ToString() != "") {
                    model.SaveDataSQL3 = ds.Tables[0].Rows[0]["SaveDataSQL3"].ToString();
                }
                if (ds.Tables[0].Rows[0]["IsSaveSQL4"] != null && ds.Tables[0].Rows[0]["IsSaveSQL4"].ToString() != "") {
                    model.IsSaveSQL4 = int.Parse(ds.Tables[0].Rows[0]["IsSaveSQL4"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SaveSQLContent4"] != null && ds.Tables[0].Rows[0]["SaveSQLContent4"].ToString() != "") {
                    model.SaveSQLContent4 = ds.Tables[0].Rows[0]["SaveSQLContent4"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SaveSQLDirectory4"] != null && ds.Tables[0].Rows[0]["SaveSQLDirectory4"].ToString() != "") {
                    model.SaveSQLDirectory4 = ds.Tables[0].Rows[0]["SaveSQLDirectory4"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Guid"] != null && ds.Tables[0].Rows[0]["Guid"].ToString() != "") {
                    model.Guid = ds.Tables[0].Rows[0]["Guid"].ToString();
                }
                //
                if (ds.Tables[0].Rows[0]["PluginSpiderUrl"] != null && ds.Tables[0].Rows[0]["PluginSpiderUrl"].ToString() != "") {
                    model.PluginSpiderUrl = ds.Tables[0].Rows[0]["PluginSpiderUrl"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PluginSpiderContent"] != null && ds.Tables[0].Rows[0]["PluginSpiderContent"].ToString() != "") {
                    model.PluginSpiderContent = ds.Tables[0].Rows[0]["PluginSpiderContent"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PluginSaveContent"] != null && ds.Tables[0].Rows[0]["PluginSaveContent"].ToString() != "") {
                    model.PluginSaveContent = ds.Tables[0].Rows[0]["PluginSaveContent"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PluginPublishContent"] != null && ds.Tables[0].Rows[0]["PluginPublishContent"].ToString() != "") {
                    model.PluginPublishContent = ds.Tables[0].Rows[0]["PluginPublishContent"].ToString();
                }




                //===============================2012 2-16
                if (ds.Tables[0].Rows[0]["CollectionContentThreadCount"] != null && ds.Tables[0].Rows[0]["CollectionContentThreadCount"].ToString() != "") {
                    model.CollectionContentThreadCount = int.Parse(ds.Tables[0].Rows[0]["CollectionContentThreadCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CollectionContentStepTime"] != null && ds.Tables[0].Rows[0]["CollectionContentStepTime"].ToString() != "") {
                    model.CollectionContentStepTime = int.Parse(ds.Tables[0].Rows[0]["CollectionContentStepTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PublishContentThreadCount"] != null && ds.Tables[0].Rows[0]["PublishContentThreadCount"].ToString() != "") {
                    model.PublishContentThreadCount = int.Parse(ds.Tables[0].Rows[0]["PublishContentThreadCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PublishContentStepTimeMin"] != null && ds.Tables[0].Rows[0]["PublishContentStepTimeMin"].ToString() != "") {
                    model.PublishContentStepTimeMin = int.Parse(ds.Tables[0].Rows[0]["PublishContentStepTimeMin"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PublishContentStepTimeMax"] != null && ds.Tables[0].Rows[0]["PublishContentStepTimeMax"].ToString() != "") {
                    model.PublishContentStepTimeMax = int.Parse(ds.Tables[0].Rows[0]["PublishContentStepTimeMax"].ToString());
                }

                if (ds.Tables[0].Rows[0]["IsHandGetUrl"] != null && ds.Tables[0].Rows[0]["IsHandGetUrl"].ToString() != "") {
                    model.IsHandGetUrl = int.Parse(ds.Tables[0].Rows[0]["IsHandGetUrl"].ToString());
                }
                if (ds.Tables[0].Rows[0]["HandCollectionUrlRegex"] != null) {
                    model.HandCollectionUrlRegex = ds.Tables[0].Rows[0]["HandCollectionUrlRegex"].ToString();
                }

                if (ds.Tables[0].Rows[0]["DemoListUrl"] != null) {
                    model.DemoListUrl = ds.Tables[0].Rows[0]["DemoListUrl"].ToString();
                }
                //
                if (ds.Tables[0].Rows[0]["IsPlan"] != null && ds.Tables[0].Rows[0]["IsPlan"].ToString() != "") {
                    model.IsPlan = int.Parse(ds.Tables[0].Rows[0]["IsPlan"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PlanFormat"] != null) {
                    model.PlanFormat = ds.Tables[0].Rows[0]["PlanFormat"].ToString();
                }
                return model;
            }
            else {
                return null;
            }
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM (Select A.*,B.TreeClassName as ClassName From S_Task A Left Join S_TreeClass B On A.TaskClassId=B.ClassId) AA ");
            if (strWhere.Trim() != "") {
                strSql.Append(" where 1=1 " + strWhere);
            }
            return SQLiteHelper.Query1(dbStr, strSql.ToString());
        }

        /*
        */

        #endregion  Method

        /// <summary>
        /// 获取单个任务模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ModelTask GetModelSingleTask(int id) {
            ModelTask model = this.GetModel(id);

            model.ListTaskLabel = new DALTaskLabel().GetModelByTaskID(id);

            model.ListModelWebPublishModule = new DALWebPublishModule().GetListModel(id);
            return model;
        }


        public bool CheckTaskGuid(int taskID) {
            DataSet ds = SQLiteHelper.Query1(dbStr, "Select Guid From S_Task Where ID=" + taskID);
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0) {
                if (!string.IsNullOrEmpty(dt.Rows[0]["guid"].ToString())) {
                    return true;
                }
                return false;
            }
            return false;
        }


        public List<ModelTaskLabel> GetListTaskLabel(int id, ref string guid) {

            return new DALTaskLabel().GetModelByTaskID(id);
        }
    }
}
