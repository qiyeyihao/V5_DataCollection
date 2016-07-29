using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Dynamic;

namespace V5_IDE._Class.DataBase.Db {
    /// <summary>
    /// SqlServer数据库操作
    /// server=.\sqlexpress;uid=sa;pwd=sa;database=V5_CMS;
    /// </summary>
    public class SqlServerHelper : IDataBase {

        private string dbStr = string.Empty;

        public string DbType {
            get {
                return "sqlserver";
            }
        }

        public string DbStr {
            get {
                return dbStr;
            }
        }

        public SqlServerHelper(string server, string username, string userpwd, int type = 0) {
            if (type == 0) {
                dbStr = "Integrated Security=SSPI;Data Source=" + server + ";Initial Catalog=master";
            }
            else {
                dbStr = "user id=" + username + ";password=" + userpwd + ";initial catalog=master;data source=" + server;
            }
        }

        #region database
        /// <summary>
        /// 获取数据库的版本号
        /// </summary>
        /// <returns></returns>
        public string GetVersion() {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("use [master];");
            sbSql.Append("execute master..sp_msgetversion");
            return this.ExecuteScalar(sbSql.ToString()).ToString();
        }
        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public List<string> GetDataBases() {
            List<string> dblist = new List<string>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("use [master];");
            sbSql.Append("select [name] from sysdatabases order by [name];");
            var list = this.Query(sbSql.ToString());
            foreach (var row in list) {
                dblist.Add(row.name);
            }
            return dblist;
        }
        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        public List<string> GetDataTables(string dbString) {
            List<string> dblist = new List<string>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("use [{0}];", dbString);
            sbSql.Append("select [name] from sysobjects where xtype='U'and [name]<>'dtproperties' order by [name];");
            var list = this.Query(sbSql.ToString());
            foreach (var row in list) {
                dblist.Add(row.name);
            }
            return dblist;
        }
        /// <summary>
        /// 获取所有视图
        /// </summary>
        /// <returns></returns>
        public List<string> GetViews(string dbString) {
            List<string> dblist = new List<string>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("use [{0}];", dbString);
            sbSql.Append("select [name] from sysobjects where xtype='V' and [name]<>'syssegments' and [name]<>'sysconstraints' order by [name]");
            var list = this.Query(sbSql.ToString());
            foreach (var row in list) {
                dblist.Add(row.name);
            }
            return dblist;
        }
        /// <summary>
        /// 获取所有存储过程
        /// </summary>
        /// <param name="dbString"></param>
        /// <returns></returns>
        public List<string> GetProcedures(string dbString) {
            List<string> dblist = new List<string>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("use [{0}];", dbString);
            sbSql.Append("select [name] from sysobjects where xtype='P'and [name]<>'dtproperties' order by [name]");
            var list = this.Query(sbSql.ToString());
            foreach (var row in list) {
                dblist.Add(row.name);
            }
            return dblist;
        }
        #endregion

        #region table
        /// <summary>
        /// 获取指定数据库表的所有结构信息
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public List<ColumnInfo> GetTableStruct(string dbName, string tbName) {
            List<ColumnInfo> dblist = new List<ColumnInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("use [{0}];", dbName);
            strSql.Append("SELECT ");
            strSql.Append("colorder=C.column_id,");
            strSql.Append("ColumnName=C.name,");
            strSql.Append("TypeName=T.name, ");
            //strSql.Append("Length=C.max_length, ");
            strSql.Append("Length=CASE WHEN T.name='nchar' THEN C.max_length/2 WHEN T.name='nvarchar' THEN C.max_length/2 ELSE C.max_length END,");
            strSql.Append("Preci=C.precision, ");
            strSql.Append("Scale=C.scale, ");
            strSql.Append("IsIdentity=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,");
            strSql.Append("isPK=ISNULL(IDX.PrimaryKey,N''),");

            strSql.Append("Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END, ");
            strSql.Append("IndexName=ISNULL(IDX.IndexName,N''), ");
            strSql.Append("IndexSort=ISNULL(IDX.Sort,N''), ");
            strSql.Append("Create_Date=O.Create_Date, ");
            strSql.Append("Modify_Date=O.Modify_date, ");

            strSql.Append("cisNull=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END, ");
            strSql.Append("defaultVal=ISNULL(D.definition,N''), ");
            strSql.Append("deText=ISNULL(PFD.[value],N'') ");

            strSql.Append("FROM sys.columns C ");
            strSql.Append("INNER JOIN sys.objects O ");
            strSql.Append("ON C.[object_id]=O.[object_id] ");
            strSql.Append("AND (O.type='U' or O.type='V') ");
            strSql.Append("AND O.is_ms_shipped=0 ");
            strSql.Append("INNER JOIN sys.types T ");
            strSql.Append("ON C.user_type_id=T.user_type_id ");
            strSql.Append("LEFT JOIN sys.default_constraints D ");
            strSql.Append("ON C.[object_id]=D.parent_object_id ");
            strSql.Append("AND C.column_id=D.parent_column_id ");
            strSql.Append("AND C.default_object_id=D.[object_id] ");
            strSql.Append("LEFT JOIN sys.extended_properties PFD ");
            strSql.Append("ON PFD.class=1  ");
            strSql.Append("AND C.[object_id]=PFD.major_id  ");
            strSql.Append("AND C.column_id=PFD.minor_id ");
            //			strSql.Append("--AND PFD.name='Caption'  -- 字段说明对应的描述名称(一个字段可以添加多个不同name的描述) ");
            strSql.Append("LEFT JOIN sys.extended_properties PTB ");
            strSql.Append("ON PTB.class=1 ");
            strSql.Append("AND PTB.minor_id=0  ");
            strSql.Append("AND C.[object_id]=PTB.major_id ");
            //			strSql.Append("-- AND PFD.name='Caption'  -- 表说明对应的描述名称(一个表可以添加多个不同name的描述)   ");
            strSql.Append("LEFT JOIN ");// -- 索引及主键信息
            strSql.Append("( ");
            strSql.Append("SELECT  ");
            strSql.Append("IDXC.[object_id], ");
            strSql.Append("IDXC.column_id, ");
            strSql.Append("Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') ");
            strSql.Append("WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, ");
            strSql.Append("PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END, ");
            strSql.Append("IndexName=IDX.Name ");
            strSql.Append("FROM sys.indexes IDX ");
            strSql.Append("INNER JOIN sys.index_columns IDXC ");
            strSql.Append("ON IDX.[object_id]=IDXC.[object_id] ");
            strSql.Append("AND IDX.index_id=IDXC.index_id ");
            strSql.Append("LEFT JOIN sys.key_constraints KC ");
            strSql.Append("ON IDX.[object_id]=KC.[parent_object_id] ");
            strSql.Append("AND IDX.index_id=KC.unique_index_id ");
            strSql.Append("INNER JOIN  ");// 对于一个列包含多个索引的情况,只显示第1个索引信息
            strSql.Append("( ");
            strSql.Append("SELECT [object_id], Column_id, index_id=MIN(index_id) ");
            strSql.Append("FROM sys.index_columns ");
            strSql.Append("GROUP BY [object_id], Column_id ");
            strSql.Append(") IDXCUQ ");
            strSql.Append("ON IDXC.[object_id]=IDXCUQ.[object_id] ");
            strSql.Append("AND IDXC.Column_id=IDXCUQ.Column_id ");
            strSql.Append("AND IDXC.index_id=IDXCUQ.index_id ");
            strSql.Append(") IDX ");
            strSql.Append("ON C.[object_id]=IDX.[object_id] ");
            strSql.Append("AND C.column_id=IDX.column_id  ");

            strSql.Append("WHERE O.name=N'" + tbName + "' ");
            strSql.Append("ORDER BY O.name,C.column_id  ");

            var list = this.Query(strSql.ToString());
            foreach (var row in list) {
                var col = new ColumnInfo();
                col.Colorder = Convert.ToString(row.colorder);
                col.ColumnName = Convert.ToString(row.ColumnName);
                col.TypeName = Convert.ToString(row.TypeName);
                col.Length = Convert.ToString(row.Length);
                col.Preci = Convert.ToString(row.Preci);
                col.Scale = Convert.ToString(row.Scale);
                col.IsIdentity = (Convert.ToString(row.IsIdentity) == "√") ? true : false;
                col.IsPK = (Convert.ToString(row.isPK) == "√") ? true : false;
                col.cisNull = (Convert.ToString(row.cisNull) == "√") ? true : false;
                col.DefaultVal = Convert.ToString(row.defaultVal);
                col.DeText = Convert.ToString(row.deText);
                dblist.Add(col);
            }
            return dblist;
        }
        #endregion


        #region Table
        /// <summary>
        /// 重命名表名称
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="oldTableName"></param>
        /// <param name="newTableName"></param>
        public void reTableName(string dbName, string oldTableName, string newTableName) {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("use [{0}];", dbName);
            sqlStr.Append("EXEC sp_rename '" + oldTableName + "', '" + newTableName + "'");

            this.Execute(sqlStr.ToString());
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tbName"></param>
        public void DropTable(string dbName, string tbName) {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("use [{0}];", dbName);
            sqlStr.Append("DROP TABLE [" + tbName + "]");

            this.Execute(sqlStr.ToString());
        }
        #endregion

        #region proc

        public void CreateProcedure() {

        }

        public void DropProcedure() {

        }

        public void AlterProcedure() {

        }

        public string SelectProcedure(string dbName, string objName) {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("use [" + dbName + "];");
            strSql.Append("select b.text ");
            strSql.Append("from sysobjects a, syscomments b  ");
            //strSql.Append("where a.xtype='p' and a.id = b.id  ");
            strSql.Append("where a.id = b.id  ");
            strSql.Append(" and a.name= '" + objName + "'");
            return this.ExecuteScalar(strSql.ToString()).ToString();
        }

        #endregion

        #region execute
        public int Execute(string sql) {
            int result = 0;
            using (var db = new SqlConnection(dbStr)) {
                db.Open();
                result = db.Execute(sql);
            }
            return result;
        }

        public object ExecuteScalar(string sql) {
            object objResult = 0;
            using (var db = new SqlConnection(dbStr)) {
                db.Open();
                objResult = db.ExecuteScalar(sql);
            }
            return objResult;
        }

        public IEnumerable<dynamic> Query(string sql) {
            IEnumerable<dynamic> dy;
            using (var db = new SqlConnection(dbStr)) {
                db.Open();
                dy = db.Query(sql);
            }
            return dy;
        }
        #endregion

        #region
        public void CreateDataBase() {

        }

        public void DropDataBase() {

        }

        public void CreateTable() {

        }

        public void AlterTable() {

        }

        public void CreateView() {

        }

        public void DropView() {

        }

        public void AlterView() {

        }


        /// <summary>
        /// 生成数据库表创建脚本
        /// </summary>
        public string CreateDataBaseScript(string dbName, string tbName) {

            var tbStruct = this.GetTableStruct(dbName, tbName);

            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("if exists (select * from sysobjects where id = OBJECT_ID('[" + tbName + "]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) ");
            strSql.AppendLine("DROP TABLE [" + tbName + "]");

            strSql.AppendLine("");
            strSql.AppendLine("CREATE TABLE [dbo].[" + tbName + "] (");

            string PKfild = "";//主键字段
            bool IsIden = false;//是否自增长列
            foreach (var row in tbStruct) {
                string columnName = row.ColumnName;
                string columnType = row.TypeName;
                bool IsIdentity = row.IsIdentity;
                string Length = row.Length;
                string Preci = row.Preci;
                string Scale = row.Scale;
                bool ispk = row.IsPK;
                bool isnull = row.cisNull;
                string defaultVal = row.DefaultVal;

                strSql.Append("[" + columnName + "] [" + columnType + "] ");
                if (IsIdentity) {
                    IsIden = true;
                    strSql.Append(" IDENTITY (1, 1) ");
                }
                switch (columnType.Trim()) {
                    case "varchar":
                    case "char":
                    case "nchar":
                    case "binary":
                    case "nvarchar":
                    case "varbinary": {
                            string len = Length;
                            strSql.Append(" (" + len + ")");
                        }
                        break;
                    case "decimal":
                    case "numeric":
                        strSql.Append(" (" + Preci + "," + Scale + ")");
                        break;
                }

                if (isnull) {
                    strSql.Append(" NULL");
                }
                else {
                    strSql.Append(" NOT NULL");
                }

                if (defaultVal != "") {
                    strSql.Append(" DEFAULT " + defaultVal);
                }
                strSql.AppendLine(",");

                if ((ispk) && (PKfild == "")) {
                    PKfild = columnName;//得到主键
                }
            }

            strSql = strSql.Remove(strSql.Length - 1, 1);
            strSql.AppendLine(")");
            strSql.AppendLine("");

            if (PKfild != "") {
                strSql.AppendLine("ALTER TABLE [" + tbName + "] WITH NOCHECK ADD  CONSTRAINT [PK_" + tbName + "] PRIMARY KEY  NONCLUSTERED ( [" + PKfild + "] )");
            }

            if (IsIden) {
                strSql.AppendLine("SET IDENTITY_INSERT [" + tbName + "] ON");
                strSql.AppendLine("");
            }
            //获取数据
            var list = this.Query("use [" + dbName + "];select top 1000 * from " + tbName);

            foreach (var item in list) {
                var row = (IDictionary<string, object>)item;

                StringBuilder strfild = new StringBuilder();
                StringBuilder strdata = new StringBuilder();
                foreach (var key in row.Keys) {
                    string colname = key;
                    string colValue = (row[key] ?? DBNull.Value).ToString();
                    string colType = tbStruct.FirstOrDefault(p => p.ColumnName == key).TypeName;

                    string strval = "";
                    switch (colType) {
                        case "binary": {
                                byte[] bys = (byte[])row[colname];
                                //如果改列为字节的话
                                strval = ToHexString(bys);
                            }
                            break;
                        case "bit": {
                                strval = (row[colname].ToString().ToLower() == "true") ? "1" : "0";
                            }
                            break;
                        default:
                            strval = colValue;
                            break;
                    }
                    if (strval != "") {
                        //判断是否为字符串
                        if (IsAddMark(colType)) {
                            strval = strval.Replace("'", "''");
                            strdata.Append("'" + strval + "',");
                        }
                        else {
                            strdata.Append(strval + ",");
                        }
                        strfild.Append("[" + colname + "],");
                    }
                }

                strfild = strfild.Remove(strfild.Length - 1, 1);
                strdata = strdata.Remove(strdata.Length - 1, 1);


                //导出数据INSERT语句
                strSql.Append("INSERT [" + tbName + "] (");
                strSql.Append(strfild.ToString());
                strSql.Append(") VALUES ( ");
                strSql.Append(strdata.ToString());//数据值
                strSql.AppendLine(")");
            }


            if (IsIden) {
                strSql.AppendLine("");
                strSql.AppendLine("SET IDENTITY_INSERT [" + tbName + "] OFF");
            }

            return strSql.ToString();
        }
        #endregion

        #region byte型数据转16进制 

        static char[] hexDigits = {'0', '1', '2', '3', '4', '5', '6', '7',
                                      '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
        public string ToHexString(byte[] bytes) {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++) {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            string str = new string(chars);
            return "0x" + str.Substring(0, bytes.Length);
        }

        #endregion

        #region 该数据类型是否加单引号

        /// <summary>
        /// 该数据类型是否加单引号
        /// </summary>
        /// <param name="columnType">数据库类型</param>
        /// <returns></returns>
        public static bool IsAddMark(string columnType) {
            bool isadd = false;
            switch (columnType) {
                case "varchar":
                case "nvarchar":
                case "datetime":
                    isadd = true;
                    break;
            }
            return isadd;
        }
        #endregion

        /// <summary>
        /// 转换IEnumerable<T>到DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l_oItems"></param>
        /// <returns></returns>
        public DataTable EnumToDataTable<T>(IEnumerable<T> l_oItems) {
            var firstItem = l_oItems.FirstOrDefault();
            if (firstItem == null)
                return new DataTable();

            DataTable oReturn = new DataTable(TypeDescriptor.GetClassName(firstItem));
            object[] a_oValues;
            int i;

            var properties = TypeDescriptor.GetProperties(firstItem);

            foreach (PropertyDescriptor property in properties) {
                oReturn.Columns.Add(property.Name, BaseType(property.PropertyType));
            }

            //#### Traverse the l_oItems
            foreach (T oItem in l_oItems) {
                //#### Collect the a_oValues for this loop
                a_oValues = new object[properties.Count];

                //#### Traverse the a_oProperties, populating each a_oValues as we go
                for (i = 0; i < properties.Count; i++)
                    a_oValues[i] = properties[i].GetValue(oItem);

                //#### .Add the .Row that represents the current a_oValues into our oReturn value
                oReturn.Rows.Add(a_oValues);
            }

            //#### Return the above determined oReturn value to the caller
            return oReturn;
        }

        public Type BaseType(Type oType) {
            //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ) {
                return Nullable.GetUnderlyingType(oType);
            }
            //#### Else the passed oType was null or was not logicially nullable, so simply return the passed oType
            else {
                return oType;
            }
        }

        /// <summary>
        /// 创建存储过程脚本
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string CreateProcedureScript(string dbName) {
            StringBuilder strSql = new StringBuilder();

            //获取存储过程
            var pros = this.GetProcedures(dbName);
            if (pros.Count > 0) {
                //存储过程
                strSql.AppendLine("GO");
                strSql.AppendLine("--=====================================================");
                strSql.AppendLine("--执行创建存储过程请先执行语句");
                strSql.AppendLine("declare @tname varchar(8000)");
                strSql.AppendLine("set @tname=''");
                strSql.AppendLine("select @tname=@tname + Name + ',' from sysobjects where xtype='P'");
                strSql.AppendLine("select @tname='drop Procedure ' + left(@tname,len(@tname)-1)");
                strSql.AppendLine("exec(@tname)");
                strSql.AppendLine("--=====================================================");

                foreach (var p in pros) {
                    strSql.AppendLine("GO");
                    //strSql.AppendLine("if exists (select * from sysobjects where id = OBJECT_ID('[" + p + "]'))");
                    //strSql.AppendLine("drop Procedure " + p);
                    var pp = this.SelectProcedure(dbName, p);
                    strSql.AppendLine(pp);
                }
            }

            //获取视图
            var views = this.GetViews(dbName);
            if (views.Count > 0) {
                strSql.AppendLine("GO");
                strSql.AppendLine("--=====================================================");
                strSql.AppendLine("--执行创建视图请先执行语句");
                strSql.AppendLine("declare @v_tname varchar(8000)");
                strSql.AppendLine("set @v_tname=''");
                //strSql.AppendLine("select @v_tname=@v_tname + OBJECT_SCHEMA_NAME(id) + '.' + Name + ',' from sysobjects where xtype='V'");
                strSql.AppendLine("select @v_tname=@v_tname + Name + ',' from sysobjects where xtype='V'");
                strSql.AppendLine("select @v_tname='drop view ' + left(@v_tname,len(@v_tname)-1)");
                strSql.AppendLine("exec(@v_tname)");
                strSql.AppendLine("--=====================================================");

                foreach (var v in views) {
                    strSql.AppendLine("GO");
                    var vv = this.SelectProcedure(dbName, v);
                    strSql.AppendLine(vv);
                }
            }
            return strSql.ToString();
        }

        public string GetDbName() {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("select db_name()");
            return this.ExecuteScalar(strSql.ToString()).ToString();
        }

        public bool IsDbLink() {
            try {
                using (var conn = new SqlConnection(dbStr)) {
                    conn.Open();
                    return conn.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception) {
                return false;
            }

        }
    }
}
