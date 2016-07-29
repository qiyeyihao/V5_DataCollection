/*
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_DataCollection._Class.Common {
    public class CommonHelper {

        public static frmMain FormMain = null;
        public static string SQLiteConnectionStringPublishLog = "System\\V5_DataPublishLog.db";
        public static string SQLiteConnectionString = "System\\V5.DataCollection.db";
        /// <summary>
        /// 替换标签内容
        /// </summary>
        /// <param name="regexContent"></param>
        public static string ReplaceSystemRegexTag(string regexContent) {
            regexContent = regexContent.Replace("\\(\\*)", ".+?");
            regexContent = regexContent.Replace("\\[参数]", "([\\S\\s]*?)");//([\\S\\s].*?)   多个一个点
            return regexContent;
        }
    }
}
