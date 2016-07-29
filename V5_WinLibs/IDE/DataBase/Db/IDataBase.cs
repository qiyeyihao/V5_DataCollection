using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V5_IDE._Class.DataBase.Db {
    /// <summary>
    /// 数据库基类
    /// </summary>
    public interface IDataBase {

        #region 访问器
        /// <summary>
        /// 数据库类型
        /// </summary>
        string DbType { get; }
        /// <summary>
        /// 数据库字符串
        /// </summary>
        string DbStr { get; }

        #endregion

        #region Table
        /// <summary>
        /// 重命名表名称
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="oldTableName"></param>
        /// <param name="newTableName"></param>
        void reTableName(string dbName, string oldTableName, string newTableName);
        #endregion

        #region Base
        /// <summary>
        /// 数据库是否连接
        /// </summary>
        /// <returns></returns>
        bool IsDbLink();
        /// <summary>
        /// 获取数据库版本号
        /// </summary>
        string GetVersion();
        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        string GetDbName();
        /// <summary>
        /// 生成数据库表创建脚本
        /// </summary>
        string CreateDataBaseScript(string dbName, string tbName);
        /// <summary>
        /// 获取数据库中的表
        /// </summary>
        List<string> GetDataTables(string dbString);

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        List<string> GetDataBases();

        /// <summary>
        /// 获取所有视图
        /// </summary>
        List<string> GetViews(string dbString);

        /// <summary>
        /// 获取所有存储过程
        /// </summary>
        List<string> GetProcedures(string dbString);
        #endregion

        #region 数据库
        /// <summary>
        /// 创建数据库
        /// </summary>
        void CreateDataBase();
        /// <summary>
        /// 删除数据库
        /// </summary>
        void DropDataBase();
        #endregion

        #region 表
        /// <summary>
        /// 创建表
        /// </summary>
        void CreateTable();
        /// <summary>
        /// 删除表
        /// </summary>
        void DropTable(string dbName, string tbName);
        /// <summary>
        /// 修改表
        /// </summary>
        void AlterTable();

        /// <summary>
        /// 获取表的结构
        /// </summary>
        List<ColumnInfo> GetTableStruct(string dbName, string tbName);
        #endregion

        #region 视图
        /// <summary>
        /// 创建视图
        /// </summary>
        void CreateView();
        /// <summary>
        /// 删除视图
        /// </summary>
        void DropView();
        /// <summary>
        /// 修改视图
        /// </summary>
        void AlterView();
        #endregion

        #region 存储过程
        /// <summary>
        /// 创建存储过程
        /// </summary>
        void CreateProcedure();
        /// <summary>
        /// 删除存储过程
        /// </summary>
        void DropProcedure();
        /// <summary>
        /// 修改存储过程
        /// </summary>
        void AlterProcedure();
        /// <summary>
        /// 获取存储过程
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="objName">对象名称</param>
        /// <returns></returns>
        string SelectProcedure(string dbName, string objName);
        /// <summary>
        /// 获取存储过程脚本
        /// </summary>
        /// <param name="dbName"></param>
        string CreateProcedureScript(string dbName);
        #endregion

        #region Execute
        /// <summary>
        /// 执行返回影响的行数
        /// </summary>
        int Execute(string sql);
        /// <summary>
        /// 执行返回首行首列
        /// </summary>
        object ExecuteScalar(string sql);
        /// <summary>
        /// 查询返回结果集
        /// </summary>
        IEnumerable<dynamic> Query(string sql);
        #endregion

        //public DataSet ToDataSet<T>(this IList<T> list) {
        //    Type elementType = typeof(T);
        //    var ds = new DataSet();
        //    var t = new DataTable();
        //    ds.Tables.Add(t);
        //    elementType.GetProperties().ToList().ForEach(propInfo => t.Columns.Add(propInfo.Name, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
        //    foreach (T item in list) {
        //        var row = t.NewRow();
        //        elementType.GetProperties().ToList().ForEach(propInfo => row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value);
        //        t.Rows.Add(row);
        //    }
        //    return ds;
        //}
    }
}
