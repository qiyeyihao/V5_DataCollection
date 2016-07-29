using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_DataCollection._Class.Common;
using System.Data;
using V5_Model;
using V5_WinLibs.DBHelper;

namespace V5_DataCollection._Class.DAL {
    public class DALTaskClass {
        string dbStr = CommonHelper.SQLiteConnectionString;
        public DALTaskClass() {
        }

        public void Insert(ModelTaskClass model) {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            //if (this.ClassID != null) {
            //    strSql1.Append("ClassID,");
            //    strSql2.Append("" + this.ClassID + ",");
            //}
            if (model.TreeClassName != null) {
                strSql1.Append("TreeClassName,");
                strSql2.Append("'" + model.TreeClassName + "',");
            }
            if (model.TreeClassReadMe != null) {
                strSql1.Append("TreeClassReadMe,");
                strSql2.Append("'" + model.TreeClassReadMe + "',");
            }
            strSql.Append("insert into S_TreeClass(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            SQLiteHelper.Execute(dbStr, strSql.ToString());
        }

        public void Update(ModelTaskClass model) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_TreeClass set ");
            if (model.TreeClassName != null) {
                strSql.Append("TreeClassName='" + model.TreeClassName + "',");
            }
            if (model.TreeClassReadMe != null) {
                strSql.Append("TreeClassReadMe='" + model.TreeClassReadMe + "',");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" where ClassID=" + model.ClassID + " ");
            int rowsAffected = SQLiteHelper.Execute(dbStr, strSql.ToString());
        }

        public void Delete(int ID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from S_TreeClass ");
            strSql.Append(" where ClassID=" + ID + " ");
            SQLiteHelper.Execute(dbStr, strSql.ToString());
        }

        public DataSet GetList(string strWhere) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM S_TreeClass ");
            if (strWhere.Trim() != "") {
                strSql.Append(" where " + strWhere);
            }
            return SQLiteHelper.Query1(dbStr, strSql.ToString());
        }

    }
}
