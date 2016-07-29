
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V5_IDE._Class {

    public static class Class1 {

        public static DataSet ToDataSet<T>(this IEnumerable<T> list) {
            Type elementType = typeof(T);
            var ds = new DataSet();
            var t = new DataTable();
            ds.Tables.Add(t);
            elementType.GetProperties().ToList().ForEach(propInfo => t.Columns.Add(propInfo.Name, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
            foreach (T item in list) {
                var row = t.NewRow();
                elementType.GetProperties().ToList().ForEach(propInfo => row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value);
                t.Rows.Add(row);
            }
            return ds;
        }

        //public static DataSet ExecuteDataSet(this IDbConnection cnn, IDbDataAdapter adapter, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) {
        //    var ds = new DataSet();
        //    var command = new CommandDefinition(sql, (object)param, null, commandTimeout, commandType, buffered ? CommandFlags.Buffered : CommandFlags.None);
        //    var identity = new Identity(command.CommandText, command.CommandType, cnn, null, param == null ? null : param.GetType(), null);
        //    var info = GetCacheInfo(identity, param, command.AddToCache);
        //    bool wasClosed = cnn.State == ConnectionState.Closed;
        //    if (wasClosed) cnn.Open();
        //    adapter.SelectCommand = command.SetupCommand(cnn, info.ParamReader);
        //    adapter.Fill(ds);
        //    if (wasClosed) cnn.Close();
        //    return ds;
        //}
    }
}
