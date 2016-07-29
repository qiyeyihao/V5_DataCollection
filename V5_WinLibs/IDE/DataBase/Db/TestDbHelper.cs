using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V5_IDE._Class.DataBase.Db {
    public class TestDbHelper {
        public void Test1() {
            var db = new SqliteHelper(@"D:\a.db");
            //IDataBase db = new SqliteHelper(@"Data Source=D:\V5.DataCollection.db;");
            //获取版本号
            //var ver = db.GetVersion();
            //获取所有数据库
            var l = db.GetDataBases();
            //获取所有表
            var tables = db.GetDataTables("V5_CMS");
            //获取所有视图
            var views = db.GetViews("V5_CMS");
            //获取所有存储过程
            //var pros = db.GetProcedures("V5_CMS");
            //获取指定库指定表的数据库结构
            var tbStruct = db.GetTableStruct("V5_CMS", "S_Test");
            //获取数据库表的脚本
            var sqlScript = db.CreateDataBaseScript("V5_CMS", "S_Test");
            //获取存储过程内容
            //var procContent = db.SelectProcedure("V5_CMS", "Pro_Article");
            //生成存储过程脚本
            //var procScript = db.CreateProcedureScript("V5_YiMei");
        }
    }
}
