using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

namespace V5_IDE._Class.DataBase.Db {
    /// <summary>
    /// Sqlite
    /// Data Source=D:\V5.DataCollection.db;
    /// </summary>
    public class SqliteHelper : IDataBase {

        private string dbStr = string.Empty;

        public string DbType {
            get {
                return "sqlite";
            }
        }

        public string DbStr {
            get {
                return dbStr;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dbStr"></param>
        public SqliteHelper(string _dbStr) {
            dbStr = string.Format("Data Source=" + _dbStr, AppDomain.CurrentDomain.BaseDirectory);
        }

        public void AlterProcedure() {

        }

        public void AlterTable() {

        }

        public void AlterView() {

        }

        public void CreateDataBase() {

        }

        public void CreateProcedure() {

        }

        public void CreateTable() {

        }

        public void CreateView() {

        }

        public void DropDataBase() {

        }

        public void DropProcedure() {

        }

        public void DropTable(string dbName, string tbName) {

        }

        public void DropView() {

        }

        public List<string> GetProcedures(string dbString) {
            return null;
        }

        public void reTableName(string dbName, string oldTableName, string newTableName) {

        }

        #region db
        public List<string> GetDataBases() {
            List<string> ss = new List<string>();
            var db = dbStr.Substring(dbStr.LastIndexOf("\\") + 1);
            ss.Add(db);
            return ss;
        }
        #endregion

        #region table
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbString"></param>
        /// <returns></returns>
        public List<string> GetDataTables(string dbString) {
            List<string> dblist = new List<string>();
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select name from sqlite_master where type='table' order by name");
            var list = Query(sqlStr.ToString());
            foreach (var row in list) {
                if (row.name.ToString() != "sqlite_sequence") {
                    dblist.Add(row.name);
                }
            }
            return dblist;
        }
        #endregion

        #region view
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbString"></param>
        /// <returns></returns>
        public List<string> GetViews(string dbString) {
            List<string> dblist = new List<string>();
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select name from sqlite_master where type='view' and name NOT LIKE 'sqlite_%' order by name");
            var list = this.Query(sqlStr.ToString());
            foreach (var row in list) {
                dblist.Add(row.name);
            }
            return dblist;
        }
        #endregion

        #region Column
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public List<ColumnInfo> GetTableStruct(string dbName, string tbName) {
            List<ColumnInfo> dblist = new List<ColumnInfo>();
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(" pragma table_info(" + tbName + "); ");
            var list = this.Query(sqlStr.ToString());
            foreach (var row in list) {
                var col = new ColumnInfo();
                col.Colorder = Convert.ToString(row.cid);
                col.ColumnName = Convert.ToString(row.name);
                col.TypeName = Convert.ToString(row.type);
                col.Length = "0";// Convert.ToString(row.Length);
                col.Preci = "0";//Convert.ToString(row.Preci);
                col.Scale = "0";//Convert.ToString(row.Scale);
                col.IsIdentity = (Convert.ToString(row.notnull) == "0" && Convert.ToString(row.pk) == "1") ? true : false;
                col.IsPK = (Convert.ToString(row.pk) == "1") ? true : false;
                col.cisNull = (Convert.ToString(row.notnull) == "0") ? true : false;
                col.DefaultVal = Convert.ToString("" + row.dflt_value);
                col.DeText = "";
                dblist.Add(col);
            }
            return dblist;
        }
        #endregion

        #region OP
        public string GetVersion() {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(string sql) {
            int result = 0;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.Execute(sql);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql) {
            object result = null;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.ExecuteScalar(sql);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string sql) {
            IEnumerable<dynamic> result = null;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.Query(sql);
            }
            return result;
        }

        private bool IsAddMark(string columnType) {
            bool isadd = false;
            if (columnType.IndexOf("varchar") > -1
                || columnType.IndexOf("nvarchar") > -1
                || columnType.IndexOf("datetime") > -1
                || columnType.IndexOf("text") > -1
                ) {
                isadd = true;
            }
            return isadd;
        }

        public string CreateDataBaseScript(string dbName, string tbName) {

            var tbStruct = this.GetTableStruct(dbName, tbName);

            string sql = "select sql from sqlite_master where type='table' and name='" + tbName + "'";
            var result = this.ExecuteScalar(sql);
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine(result.ToString());

            //获取数据
            var list = this.Query("select * from " + tbName + " limit 1000");
            foreach (var item in list) {
                var row = (IDictionary<string, object>)item;

                StringBuilder strfild = new StringBuilder();
                StringBuilder strdata = new StringBuilder();
                foreach (var key in row.Keys) {
                    string colname = key;
                    string colValue = (row[key] ?? DBNull.Value).ToString();
                    string colType = tbStruct.FirstOrDefault(p => p.ColumnName == key).TypeName;
                    bool isPK = tbStruct.FirstOrDefault(p => p.ColumnName == key).IsPK;
                    switch (colType.ToLower()) {
                        case "true":
                            colValue = "1";
                            break;
                    }
                    if (!isPK) {
                        //判断是否为字符串
                        if (IsAddMark(colType)) {
                            colValue = colValue.Replace("'", "''");
                            strdata.Append("'" + colValue + "',");
                        }
                        else {
                            if (colValue == "")
                                colValue = "0";
                            strdata.Append(colValue + ",");
                        }
                        strfild.Append(@" """ + colname + @""",");
                    }
                }
                strfild = strfild.Remove(strfild.Length - 1, 1);
                strdata = strdata.Remove(strdata.Length - 1, 1);
                //导出数据INSERT语句
                strSql.Append(string.Format(@"INSERT Into ""{0}"" (", tbName));
                strSql.Append(strfild.ToString());
                strSql.Append(") VALUES ( ");
                strSql.Append(strdata.ToString());//数据值
                strSql.AppendLine(")");
            }
            return strSql.ToString();
        }

        public string SelectProcedure(string dbName, string objName) {
            return string.Empty;
        }

        public string CreateProcedureScript(string dbName) {
            return string.Empty;
        }
        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        public string GetDbName() {
            return dbStr.Substring(dbStr.LastIndexOf("\\") + 1);
        }

        /// <summary>
        /// 数据库是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsDbLink() {
            try {
                using (var conn = new SQLiteConnection(dbStr)) {
                    conn.Open();
                    return conn.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex) {
                return false;
            }
        }
        #endregion
    }
}
