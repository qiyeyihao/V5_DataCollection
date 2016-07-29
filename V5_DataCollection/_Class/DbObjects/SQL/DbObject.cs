using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace V5_DataCollection._Class.DbObjects.SQL {
    public class DbObject {
        private string _dbconnectStr;
        public string DbConnectStr {
            set { _dbconnectStr = value; }
            get { return _dbconnectStr; }
        }
        SqlConnection connect = new SqlConnection();

        #region 得到数据库的名字列表 GetDBList()

        /// <summary>
        /// 得到数据库的名字列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetDBList() {
            string strSql = "select name from sysdatabases";
            List<string> dblist = new List<string>();
            SqlDataReader reader = ExecuteReader("master", strSql);
            while (reader.Read()) {
                dblist.Add(reader.GetString(0));
            }
            reader.Close();
            return dblist;
        }
        #endregion


        #region 打开数据库 OpenDB(string DbName)

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="DbName">要打开的数据库</param>
        /// <returns></returns>
        private SqlCommand OpenDB(string DbName) {
            try {
                if (connect.ConnectionString == "") {
                    connect.ConnectionString = _dbconnectStr;
                }
                if (connect.ConnectionString != _dbconnectStr) {
                    connect.Close();
                    connect.ConnectionString = _dbconnectStr;
                }
                SqlCommand dbCommand = new SqlCommand();
                dbCommand.Connection = connect;
                if (connect.State == System.Data.ConnectionState.Closed) {
                    connect.Open();
                }
                dbCommand.CommandText = "use [" + DbName + "]";
                dbCommand.ExecuteNonQuery();
                return dbCommand;

            }
            catch (System.Exception ex) {
                string str = ex.Message;
                return null;
            }

        }
        #endregion

        #region ADO.NET 操作

        public int ExecuteSql(string DbName, string SQLString) {
            SqlCommand dbCommand = OpenDB(DbName);
            dbCommand.CommandText = SQLString;
            int rows = dbCommand.ExecuteNonQuery();
            return rows;
        }
        public DataSet Query(string DbName, string SQLString) {
            DataSet ds = new DataSet();
            try {
                OpenDB(DbName);
                SqlDataAdapter sda = new SqlDataAdapter(SQLString, connect);
                sda.Fill(ds, "ds");
            }
            catch (System.Data.SqlClient.SqlException ex) {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        public SqlDataReader ExecuteReader(string DbName, string strSQL) {
            try {
                OpenDB(DbName);
                SqlCommand cmd = new SqlCommand(strSQL, connect);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (SqlException ex) {
                throw ex;
            }
        }
        public object GetSingle(string DbName, string SQLString) {
            try {
                SqlCommand dbCommand = OpenDB(DbName);
                dbCommand.CommandText = SQLString;
                object obj = dbCommand.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                    return null;
                }
                else {
                    return obj;
                }
            }
            catch {
                return null;
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string DbName, string storedProcName, IDataParameter[] parameters, string tableName) {

            OpenDB(DbName);
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connect, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);

            return dataSet;

        }
        private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters) {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters) {
                if (parameter != null) {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null)) {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        #endregion
    }
}
