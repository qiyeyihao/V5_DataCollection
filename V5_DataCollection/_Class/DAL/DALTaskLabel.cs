using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using V5_Model;
using V5_DataCollection._Class.Common;
using V5_WinLibs.DBHelper;

namespace V5_DataCollection._Class.DAL {
    public class DALTaskLabel {

        string dbStr = CommonHelper.SQLiteConnectionString;
        public DALTaskLabel() {
        }

        #region  Method
        public int GetMaxID() {
            object obj = SQLiteHelper.ExecuteScalar(dbStr, "select max(id)+1 from S_TaskLabel");
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
        public int Add(ModelTaskLabel model) {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.LabelName != null) {
                strSql1.Append("LabelName,");
                strSql2.Append("'" + model.LabelName + "',");
            }
            if (model.LabelNameCutRegex != null) {
                strSql1.Append("LabelNameCutRegex,");
                strSql2.Append("'" + model.LabelNameCutRegex + "',");
            }
            if (model.LabelHtmlRemove != null) {
                strSql1.Append("LabelHtmlRemove,");
                strSql2.Append("'" + model.LabelHtmlRemove + "',");
            }
            if (model.LabelRemove != null) {
                strSql1.Append("LabelRemove,");
                strSql2.Append("'" + model.LabelRemove + "',");
            }
            if (model.LabelReplace != null) {
                strSql1.Append("LabelReplace,");
                strSql2.Append("'" + model.LabelReplace + "',");
            }
            if (model.TaskID != null) {
                strSql1.Append("TaskID,");
                strSql2.Append("" + model.TaskID + ",");
            }
            if (model.GuidNum != null) {
                strSql1.Append("GuidNum,");
                strSql2.Append("'" + model.GuidNum + "',");
            }
            if (model.OrderID != null) {
                strSql1.Append("OrderID,");
                strSql2.Append("" + model.OrderID + ",");
            }
            if (model.CreateTime != null) {
                strSql1.Append("CreateTime,");
                strSql2.Append("'" + model.CreateTime + "',");
            }
            //=============================================2012 2-6
            if (model.IsLoop != null) {
                strSql1.Append("IsLoop,");
                strSql2.Append("" + model.IsLoop + ",");
            }
            if (model.IsNoNull != null) {
                strSql1.Append("IsNoNull,");
                strSql2.Append("" + model.IsNoNull + ",");
            }
            if (model.IsLinkUrl != null) {
                strSql1.Append("IsLinkUrl,");
                strSql2.Append("" + model.IsLinkUrl + ",");
            }
            if (model.IsPager != null) {
                strSql1.Append("IsPager,");
                strSql2.Append("" + model.IsPager + ",");
            }
            if (model.LabelValueLinkUrlRegex != null) {
                strSql1.Append("LabelValueLinkUrlRegex,");
                strSql2.Append("'" + model.LabelValueLinkUrlRegex + "',");
            }
            if (model.LabelValuePagerRegex != null) {
                strSql1.Append("LabelValuePagerRegex,");
                strSql2.Append("'" + model.LabelValuePagerRegex + "',");
            }
            //
            if (model.SpiderLabelPlugin != null) {
                strSql1.Append("SpiderLabelPlugin,");
                strSql2.Append("'" + model.SpiderLabelPlugin + "',");
            }
            //
            if (model.IsDownResource != null) {
                strSql1.Append("IsDownResource,");
                strSql2.Append("" + model.IsDownResource + ",");
            }
            if (model.DownResourceExts != null) {
                strSql1.Append("DownResourceExts,");
                strSql2.Append("'" + model.DownResourceExts + "',");
            }
            strSql.Append("insert into S_TaskLabel(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            strSql.Append(";select LAST_INSERT_ROWID()");
            object obj = SQLiteHelper.ExecuteScalar(dbStr, strSql.ToString());
            if (obj == null) {
                return 0;
            }
            else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ModelTaskLabel model) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_TaskLabel set ");
            if (model.LabelName != null) {
                strSql.Append("LabelName='" + model.LabelName + "',");
            }
            if (model.LabelNameCutRegex != null) {
                strSql.Append("LabelNameCutRegex='" + model.LabelNameCutRegex + "',");
            }
            if (model.LabelHtmlRemove != null) {
                strSql.Append("LabelHtmlRemove='" + model.LabelHtmlRemove + "',");
            }
            if (model.LabelRemove != null) {
                strSql.Append("LabelRemove='" + model.LabelRemove + "',");
            }
            if (model.LabelReplace != null) {
                strSql.Append("LabelReplace='" + model.LabelReplace + "',");
            }
            if (model.TaskID != null) {
                strSql.Append("TaskID=" + model.TaskID + ",");
            }
            if (model.GuidNum != null) {
                strSql.Append("GuidNum='" + model.GuidNum + "',");
            }
            //=========================================2012 2-6
            if (model.IsLoop != null) {
                strSql.Append("IsLoop=" + model.IsLoop + ",");
            }
            if (model.IsNoNull != null) {
                strSql.Append("IsNoNull=" + model.IsNoNull + ",");
            }
            if (model.IsLinkUrl != null) {
                strSql.Append("IsLinkUrl=" + model.IsLinkUrl + ",");
            }
            if (model.IsPager != null) {
                strSql.Append("IsPager=" + model.IsPager + ",");
            }
            if (model.LabelValueLinkUrlRegex != null) {
                strSql.Append("LabelValueLinkUrlRegex='" + model.LabelValueLinkUrlRegex + "',");
            }
            if (model.LabelValuePagerRegex != null) {
                strSql.Append("LabelValuePagerRegex='" + model.LabelValuePagerRegex + "',");
            }
            //
            if (model.SpiderLabelPlugin != null) {
                strSql.Append("SpiderLabelPlugin='" + model.SpiderLabelPlugin + "',");
            }
            //
            if (model.IsDownResource != null) {
                strSql.Append("IsDownResource=" + model.IsDownResource + ",");
            }
            if (model.DownResourceExts != null) {
                strSql.Append("DownResourceExts='" + model.DownResourceExts + "',");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" where ID=" + model.ID + "");
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
            strSql.Append("delete from S_TaskLabel ");
            strSql.Append(" where ID=" + ID + "");
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
            strSql.Append("delete from S_TaskLabel ");
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
        public ModelTaskLabel GetModel(int ID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            strSql.Append(" * ");
            strSql.Append(" from S_TaskLabel ");
            strSql.Append(" where ID=" + ID + "");
            ModelTaskLabel model = new ModelTaskLabel();
            DataSet ds = SQLiteHelper.Query1(dbStr, strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0) {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "") {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LabelName"] != null) {
                    model.LabelName = ds.Tables[0].Rows[0]["LabelName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LabelNameCutRegex"] != null) {
                    model.LabelNameCutRegex = ds.Tables[0].Rows[0]["LabelNameCutRegex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LabelHtmlRemove"] != null) {
                    model.LabelHtmlRemove = ds.Tables[0].Rows[0]["LabelHtmlRemove"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LabelRemove"] != null) {
                    model.LabelRemove = ds.Tables[0].Rows[0]["LabelRemove"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LabelReplace"] != null) {
                    model.LabelReplace = ds.Tables[0].Rows[0]["LabelReplace"].ToString();
                }
                if (ds.Tables[0].Rows[0]["TaskID"].ToString() != "") {
                    model.TaskID = int.Parse(ds.Tables[0].Rows[0]["TaskID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["GuidNum"] != null) {
                    model.GuidNum = ds.Tables[0].Rows[0]["GuidNum"].ToString();
                }
                if (ds.Tables[0].Rows[0]["OrderID"].ToString() != "") {
                    model.OrderID = int.Parse(ds.Tables[0].Rows[0]["OrderID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CreateTime"] != null) {
                    model.CreateTime = ds.Tables[0].Rows[0]["CreateTime"].ToString();
                }
                //=================================2012 2-6
                if (ds.Tables[0].Rows[0]["IsLoop"] != null && ds.Tables[0].Rows[0]["IsLoop"].ToString() != "") {
                    model.IsLoop = int.Parse(ds.Tables[0].Rows[0]["IsLoop"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsNoNull"] != null && ds.Tables[0].Rows[0]["IsNoNull"].ToString() != "") {
                    model.IsNoNull = int.Parse(ds.Tables[0].Rows[0]["IsNoNull"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsLinkUrl"] != null && ds.Tables[0].Rows[0]["IsLinkUrl"].ToString() != "") {
                    model.IsLinkUrl = int.Parse(ds.Tables[0].Rows[0]["IsLinkUrl"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsPager"] != null && ds.Tables[0].Rows[0]["IsPager"].ToString() != "") {
                    model.IsPager = int.Parse(ds.Tables[0].Rows[0]["IsPager"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LabelValueLinkUrlRegex"] != null) {
                    model.LabelValueLinkUrlRegex = ds.Tables[0].Rows[0]["LabelValueLinkUrlRegex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LabelValuePagerRegex"] != null) {
                    model.LabelValuePagerRegex = ds.Tables[0].Rows[0]["LabelValuePagerRegex"].ToString();
                }
                //
                if (ds.Tables[0].Rows[0]["SpiderLabelPlugin"] != null) {
                    model.SpiderLabelPlugin = ds.Tables[0].Rows[0]["SpiderLabelPlugin"].ToString();
                }
                //
                if (ds.Tables[0].Rows[0]["IsDownResource"] != null && ds.Tables[0].Rows[0]["IsDownResource"].ToString() != "") {
                    model.IsDownResource = int.Parse(ds.Tables[0].Rows[0]["IsDownResource"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DownResourceExts"] != null) {
                    model.DownResourceExts = ds.Tables[0].Rows[0]["DownResourceExts"].ToString();
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
            strSql.Append(" FROM S_TaskLabel ");
            if (strWhere.Trim() != "") {
                strSql.Append(" where " + strWhere);
            }
            return SQLiteHelper.Query1(dbStr, strSql.ToString());
        }

        /*
        */

        #endregion  Method

        public void UpdateTaskLabelByTaskID(int TaskID) {
            SQLiteHelper.Execute(dbStr, "Update S_TaskLabel Set TaskID=" + TaskID + " Where TaskID=0 ");
        }

        public ModelTaskLabel GetModel(string LabelName, int TaskID) {
            DataTable dt = this.GetList(" TaskID=" + TaskID + " And LabelName='" + LabelName + "' ").Tables[0];
            int EditID = int.Parse("0" + dt.Rows[0]["ID"].ToString());
            return this.GetModel(EditID);
        }

        public List<ModelTaskLabel> GetModelByTaskID(int TaskID) {
            List<ModelTaskLabel> list = new List<ModelTaskLabel>();
            DataTable dt = this.GetList(" TaskID=" + TaskID + " Order by OrderID asc").Tables[0];
            foreach (DataRow dr in dt.Rows) {
                ModelTaskLabel model = this.GetModel(int.Parse("0" + dr["ID"]));
                list.Add(model);
            }
            return list;
        }

        public bool Delete(string LabelName, int TaskID) {
            DataTable dt = this.GetList(" TaskID=" + TaskID + " And LabelName='" + LabelName + "' ").Tables[0];
            int EditID = int.Parse("0" + dt.Rows[0]["ID"].ToString());
            return this.Delete(EditID);
        }
        /// <summary>
        /// 获取最大的排序ID
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public int GetMaxOrderID(int taskID) {
            int orderID = int.Parse("0" + SQLiteHelper.ExecuteScalar(dbStr, "Select max(OrderID) From S_TaskLabel Where TaskID=" + taskID));
            return orderID;
        }
        /// <summary>
        /// 数据排序
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="orderType">1为向上 -1向下</param>
        /// <returns></returns>
        public bool UpdateOrder(int TaskID, int ID, int orderType) {
            //step1 找到比它小的且最大的ID的那条记录
            int OrderID = 0, tempID = 0;
            string sql = string.Empty;
            if (orderType == -1) {
                sql = string.Format(@"Select Max(OrderID),ID From S_TaskLabel Where orderid <(select orderid from S_TaskLabel where id={0}) And TaskID={1} Group By ID ", ID, TaskID);
                DataTable dt = SQLiteHelper.Query1(dbStr, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0) {
                    OrderID = int.Parse("0" + dt.Rows[0][0]);
                    tempID = int.Parse("0" + dt.Rows[0][1]);
                    if (tempID != 0) {
                        //step2 更新当前一条记录，让ID+1 
                        sql = "Update S_TaskLabel Set OrderID=" + OrderID + " Where ID=" + ID;
                        SQLiteHelper.Execute(dbStr, sql);
                        //3 更新当前记录，让ID=前一条记录
                        sql = "Update S_TaskLabel Set OrderID=OrderID+1 Where ID=" + tempID;
                        SQLiteHelper.Execute(dbStr, sql);
                    }
                }
            }
            else {
                sql = string.Format("Select Min(OrderID),ID From S_TaskLabel Where orderid >(select orderid from S_TaskLabel where id={0}) And TaskID={1} Group By ID ", ID, TaskID);
                DataTable dt = SQLiteHelper.Query1(dbStr, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0) {
                    OrderID = int.Parse("0" + dt.Rows[0][0]);
                    tempID = int.Parse("0" + dt.Rows[0][1]);
                    if (tempID != 0) {
                        //step2 更新当前一条记录，让ID+1 
                        sql = "Update S_TaskLabel Set OrderID=" + OrderID + " Where ID=" + ID;
                        SQLiteHelper.Execute(dbStr, sql);
                        //3 更新当前记录，让ID=前一条记录
                        sql = "Update S_TaskLabel Set OrderID=OrderID-1 Where ID=" + tempID;
                        SQLiteHelper.Execute(dbStr, sql);
                    }
                }
            }
            return true;
        }


        public void TaskLabelCopy(int ID) {
            string sql = string.Empty;
            int maxID = this.GetMaxID();
            sql = string.Format(@"
                    INSERT INTO s_tasklabel 
                    SELECT 
                      {0} , 
                      labelname,  
                      labelnamecutregex,  
                      labelhtmlremove,  
                      labelremove,  
                      labelreplace,  
                      taskid,  
                      guidnum,  
                      orderid,  
                      createtime,  
                      isloop,  
                      isnonull,  
                      islinkurl,  
                      ispager,  
                      labelvaluelinkurlregex,  
                      labelvaluepagerregex
                    FROM 
                      s_tasklabel where id={1};
            ", maxID, ID);
            SQLiteHelper.Execute(dbStr, sql);
        }
    }
}
