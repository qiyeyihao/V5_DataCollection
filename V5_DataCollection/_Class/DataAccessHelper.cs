using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using V5_WinLibs.Core;

namespace V5_DataCollection._Class {
    /// <summary>
    /// 数据IOC注入
    /// </summary>
    public static class DataAccessHelper {
        public static readonly string AssemblyPath = "V5_DAL." + ConfigurationManager.AppSettings["dbType"];
        public static readonly string DbType = ConfigurationManager.AppSettings["dbType"];
        static CacheManageHelper cache = CacheManageHelper.Instance;
        /// <summary>
        /// 创建对象或从缓存获取
        /// </summary>
        public static object CreateObject(string AssemblyPath, string ClassNamespace) {
            object objType = cache.Get(ClassNamespace);//从缓存读取
            if (objType == null) {
                try {
                    objType = Assembly.Load(AssemblyPath).CreateInstance(ClassNamespace);//反射创建
                    cache.Add(ClassNamespace, objType);// 写入缓存
                }
                catch { }
            }
            return objType;
        }
        private static object GetIDAL(object obj1, object obj2, object obj3, string DALName) {
            object objType;
            if (DbType == "OleDb") {
                objType = obj1;
            }
            else if (DbType == "SqlServer") {
                objType = obj2;
            }
            else if (DbType == "SQLite") {
                objType = obj2;
            }
            else {
                string ClassNamespace = AssemblyPath + "." + DALName;
                objType = CreateObject(AssemblyPath, ClassNamespace);
            }
            return objType;
        }
    }
}
