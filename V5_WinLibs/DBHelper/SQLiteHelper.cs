using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using System.Data;

namespace V5_WinLibs.DBHelper {
    public class SQLiteHelper {

        /// <summary>
        /// 数据库是否打开
        /// </summary>
        /// <param name="dbStr"></param>
        /// <returns></returns>
        public static bool IsOpen(string dbStr) {
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                return conn.State == System.Data.ConnectionState.Open;
            }
        }

        private static string GetDBStr(string dbStr) {
            return string.Format("Data Source=" + dbStr, AppDomain.CurrentDomain.BaseDirectory);// 
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="databaseFileName"></param>
        public static void CreateDataBase(string databaseFileName) {
            SQLiteConnection.CreateFile(databaseFileName);
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="dbStr"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet Query1(string dbStr, string sql) {
            dbStr = GetDBStr(dbStr);
            var ds = new DataSet();
            using (var conn = new SQLiteConnection(dbStr)) {
                try {
                    conn.Open();
                    IDbDataAdapter adapter = new SQLiteDataAdapter();
                    IDbCommand commond = conn.CreateCommand();
                    commond.CommandText = sql;
                    commond.CommandType = CommandType.Text;
                    adapter.SelectCommand = commond;
                    adapter.Fill(ds);
                }
                catch (Exception ex) {
                    conn.Close();
                    throw ex;  
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行压缩数据库
        /// </summary>
        /// <returns>压缩数据库</returns>
        public static void ExecuteZip(string dbStr) {
            dbStr = GetDBStr(dbStr);
            using (SQLiteConnection connection = new SQLiteConnection(dbStr)) {
                using (SQLiteCommand cmd = new SQLiteCommand("VACUUM", connection)) {
                    try {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SQLite.SQLiteException E) {
                        connection.Close();
                    }
                }
            }
        }

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Execute(string dbStr, string sql) {
            dbStr = GetDBStr(dbStr);
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
        public static object ExecuteScalar(string dbStr, string sql) {
            dbStr = GetDBStr(dbStr);
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
        public static IEnumerable<dynamic> Query(string dbStr, string sql) {
            dbStr = GetDBStr(dbStr);
            IEnumerable<dynamic> result = null;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.Query(sql);
            }
            return result;
        }

        public static IEnumerable<T> Query<T>(string dbStr, string sql) {
            dbStr = GetDBStr(dbStr);
            IEnumerable<T> result = null;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.Query<T>(sql);
            }
            return result;
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Execute(string dbStr, string sql, object param) {
            dbStr = GetDBStr(dbStr);
            int result = 0;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.Execute(sql, param);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string dbStr, string sql, object param) {
            dbStr = GetDBStr(dbStr);
            object result = null;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.ExecuteScalar(sql, param);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Query(string dbStr, string sql, object param) {
            dbStr = GetDBStr(dbStr);
            IEnumerable<dynamic> result = null;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.Query(sql, param);
            }
            return result;
        }

        public static IEnumerable<T> Query<T>(string dbStr, string sql, object param) {
            dbStr = GetDBStr(dbStr);
            IEnumerable<T> result = null;
            using (var conn = new SQLiteConnection(dbStr)) {
                conn.Open();
                result = conn.Query<T>(sql, param);
            }
            return result;
        }
        #endregion

    }
}
