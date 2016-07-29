using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Collections;
using System.Drawing;

namespace V5_WinLibs.Core {
    /// <summary>
    /// 采集分类
    /// </summary>
    public class CollectionHelper {
        public CollectionHelper() {
        }

        #region DownLoad Url
        /// <summary>
        /// 下载文件到本地
        /// </summary>
        /// <param name="?"></param>
        public static string DownloadFile(string address, int Timeout) {
            string tempString = "";
            try {
                WebRequest request1 = WebRequest.Create(address);
                request1.Timeout = Timeout;//加上超时时间
                WebResponse response1 = request1.GetResponse();
                long num1 = response1.ContentLength;
                num1 = ((num1 == -1) || (num1 > 0x7fffffff)) ? ((long)0x7fffffff) : num1;
                byte[] buffer1 = new byte[Math.Min(0x2000, (int)num1)];
                Stream stream2 = response1.GetResponseStream();
                stream2.Position = 0;
                byte[] vBytes = new byte[stream2.Length];
                stream2.Read(vBytes, 0, (int)stream2.Length);
                string s1 = Encoding.UTF8.GetString(vBytes);
                tempString = s1;
            }
            catch {
                tempString = "";
            }
            return tempString;
        }
        /// <summary>
        /// 得到页面的内容     $UrlIsFalse$    $GetFalse$
        /// </summary>
        /// <param name="url">地址 http://www.qbzw.com</param>
        /// <param name="timeout">返回超时 单位 毫秒</param>
        /// <param name="EnCodeType">编码 gb2312 等</param>
        /// <returns></returns>
        public static string GetHttpPage(string url, int timeout, Encoding EnCodeType) {
            string strResult = string.Empty;
            if (url.Length < 10)
                return "$UrlIsFalse$";
            try {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Encoding = EnCodeType;
                //MyWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                wc.Headers.Add("Content-Type", "text/xml");
                strResult = wc.DownloadString(url);
            }
            catch (Exception) {
                strResult = "$GetFalse$";
            }
            return strResult;
        }

        /// <summary>
        /// 自动获取网页内容  $UrlIsFalse$    $GetFalse$
        /// </summary>
        public static string GetHttpPage(string url, int timeout) {
            string strResult = string.Empty;
            if (url.Length < 10)
                return "$UrlIsFalse$";
            try {
                WebClient wc = new WebClient(); //创建WebClient实例myWebClient
                wc.Credentials = CredentialCache.DefaultCredentials;
                byte[] myDataBuffer = wc.DownloadData(url);
                string strWebData = Encoding.Default.GetString(myDataBuffer);
                Match charSetMatch = Regex.Match(strWebData, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string webCharSet = charSetMatch.Groups[2].Value;
                webCharSet = webCharSet.Replace("\"", "");
                strWebData = Encoding.GetEncoding(webCharSet).GetString(myDataBuffer);
                strResult = strWebData;
            }
            catch (Exception ex) {
                strResult = "$GetFalse$";
            }
            return strResult;
        }

        public static string GetHttpPage(string url, int timeout, string webCharSet) {
            string strResult = string.Empty;
            if (url.Length < 10)
                return "$UrlIsFalse$";
            try {
                WebClient wc = new WebClient(); //创建WebClient实例myWebClient
                wc.Credentials = CredentialCache.DefaultCredentials;
                byte[] myDataBuffer = wc.DownloadData(url);
                string strWebData = Encoding.Default.GetString(myDataBuffer);
                strWebData = Encoding.GetEncoding(webCharSet).GetString(myDataBuffer);
                strResult = strWebData;
            }
            catch (Exception ex) {
                strResult = "$GetFalse$";
            }
            return strResult;
        }
        #endregion


        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="pageStr">截取的字符串</param>
        /// <param name="strStart">开始字符</param>
        /// <param name="strEnd">结束字符</param>
        /// <param name="inStart">是否包含开始字符</param>
        /// <param name="inEnd">是否包含结束字符</param>
        /// <returns>返回匹配的字符串</returns>
        public static string GetBody(string pageStr, string strStart, string strEnd, bool inStart, bool inEnd) {
            pageStr = pageStr.Trim();
            int start = pageStr.IndexOf(strStart);
            if (strStart.Length == 0 || start < 0)
                return "$StartFalse$";
            pageStr = pageStr.Substring(start + strStart.Length, pageStr.Length - start - strStart.Length);
            int end = pageStr.IndexOf(strEnd);
            if (strEnd.Length == 0 || end < 0)
                return "$EndFalse$";
            string strResult = pageStr.Substring(0, end);
            if (inStart)
                strResult = strStart + strResult;
            if (inEnd)
                strResult += strEnd;
            return strResult.Trim();
        }

        /// <summary>
        /// 截取字符
        /// </summary>
        /// <param name="sStr">字符</param>
        /// <param name="Patrn">表达式</param>
        /// <returns></returns>
        public static string[] CutStr(string sStr, string Patrn) {
            string[] RsltAry;
            Regex tmpreg = new Regex(Patrn, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection sMC = tmpreg.Matches(sStr);
            if (sMC.Count != 0) {
                RsltAry = new string[sMC.Count];
                for (int i = 0; i < sMC.Count; i++) {
                    RsltAry[i] = sMC[i].Groups[1].Value;
                }
            }
            else {
                RsltAry = new string[1];
                RsltAry[0] = "";
            }
            return RsltAry;
        }

        /// <summary>
        /// 格式化地址
        /// </summary>
        /// <param name="BaseUrl">http://www.v5soft.com</param>
        /// <param name="theUrl">/index.html</param>
        /// <returns></returns>
        public static string FormatUrl(string BaseUrl, string theUrl) {
            int pathLevel = 0;
            string hostName;
            theUrl = theUrl.ToLower();
            hostName = BaseUrl;
            if (BaseUrl.IndexOf("/", 8) > -1) {
                hostName = BaseUrl.Substring(0, BaseUrl.IndexOf("/", 8));
            }
            BaseUrl = BaseUrl.Substring(0, BaseUrl.LastIndexOf("/"));
            if (theUrl.StartsWith("./")) {
                theUrl = theUrl.Remove(0, 1);
                theUrl = BaseUrl + theUrl;
            }
            else if (theUrl.StartsWith("/")) {
                theUrl = hostName + theUrl;
            }
            else if (theUrl.StartsWith("../")) {
                while (theUrl.StartsWith("../")) {
                    pathLevel = ++pathLevel;
                    theUrl = theUrl.Remove(0, 3);
                }
                for (int i = 0; i < pathLevel; i++) {
                    BaseUrl = BaseUrl.Substring(0, BaseUrl.LastIndexOf("/", BaseUrl.Length - 2));
                }
                theUrl = BaseUrl + "/" + theUrl;
            }
            if (!theUrl.StartsWith("http://") && !theUrl.StartsWith("https://")) {
                theUrl = BaseUrl + "/" + theUrl;
            }
            return theUrl;
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="pageStr">截取的字符串</param>
        /// <param name="strStart">开始字符串</param>
        /// <param name="strEnd">结束字符串</param>
        /// <returns>匹配导的所有连接 ArrayList类型</returns>
        public static ArrayList GetArray(string pageStr, string strStart, string strEnd) {
            ArrayList linkArray = new ArrayList();
            int start = pageStr.IndexOf(strStart);
            if (strStart.Length == 0 || start < 0) {
                linkArray.Add("$StartFalse$");
                return linkArray;
            }
            int end = pageStr.IndexOf(strEnd);
            if (strEnd.Length == 0 || end < 0) {
                linkArray.Add("$EndFalse$");
                return linkArray;
            }
            Regex myRegex = new Regex(@"(" + strStart + ").+?(" + strEnd + ")", RegexOptions.IgnoreCase);
            MatchCollection matches = myRegex.Matches(pageStr);
            foreach (Match match in matches)
                linkArray.Add(match.ToString());
            if (linkArray.Count == 0) {
                linkArray.Add("$NoneLink$");
                return linkArray;
            }
            string TempStr = string.Empty;
            for (int i = 0; i < linkArray.Count; i++) {
                TempStr = linkArray[i].ToString();
                TempStr = TempStr.Replace(strStart, "");
                TempStr = TempStr.Replace(strEnd, "");
                linkArray[i] = (object)TempStr;
            }
            return linkArray;
        }

        /// <summary>
        /// 替换远程图片并保存图片
        /// </summary>
        /// <param name="pageStr"></param>
        /// <param name="SavePath"></param>
        /// <param name="CDir"></param>
        /// <param name="webUrl"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public static ArrayList ReplaceSaveRemoteFile(string pageStr, string SavePath, string CDir, string webUrl, string isSave) {
            ArrayList replaceArray = new ArrayList();
            Regex imgReg = new Regex(@"<img.+?[^\>]>", RegexOptions.IgnoreCase);
            MatchCollection matches = imgReg.Matches(pageStr);
            string TempStr = string.Empty;
            string TitleImg = string.Empty;
            foreach (Match match in matches) {
                if (TempStr != string.Empty)
                    TempStr += "$Array$" + match.ToString();
                else
                    TempStr = match.ToString();
            }
            string[] TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            imgReg = new Regex(@"src\s*=\s*.+?\.(gif|jpg|bmp|jpeg|psd|png|svg|dxf|wmf|tiff)", RegexOptions.IgnoreCase);
            for (int i = 0; i < TempArr.Length; i++) {
                matches = imgReg.Matches(TempArr[i]);
                foreach (Match match in matches) {
                    if (TempStr != string.Empty)
                        TempStr += "$Array$" + match.ToString();
                    else
                        TempStr = match.ToString();
                }
            }
            if (TempStr.Length > 0) {
                imgReg = new Regex(@"src\s*=\s*", RegexOptions.IgnoreCase);
                TempStr = imgReg.Replace(TempStr, "");
            }
            if (TempStr.Length == 0) {
                replaceArray.Add(pageStr);
                return replaceArray;
            }
            TempStr = TempStr.Replace("\"", "");
            TempStr = TempStr.Replace("'", "");
            TempStr = TempStr.Replace(" ", "");
            SavePath = SavePath + "/UserFiles/";// +DirFileIO.GetDateDir();
            //SavePath = SavePath + "/UserFiles/" + DirFileIO.GetDateDir();
            if (!System.IO.Directory.Exists(SavePath))
                System.IO.Directory.CreateDirectory(SavePath);

            //去掉重复图片
            TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            for (int i = 0; i < TempArr.Length; i++) {
                if (TempStr.IndexOf(TempArr[i]) == -1)
                    TempStr += "$Array$" + TempArr[i];
            }
            TempStr = TempStr.Substring(7);

            TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            string ImageArr = string.Empty;
            for (int i = 0; i < TempArr.Length; i++) {
                imgReg = new Regex(TempArr[i]);
                string RemoteFileUrl = DefiniteUrl(TempArr[i], webUrl);
                if (isSave == "1") {
                    string fileType = RemoteFileUrl.Substring(RemoteFileUrl.LastIndexOf('.'));
                    string filename = string.Empty;
                    //filename = DirFileIO.GetDateFile();
                    filename += fileType;
                    if (SaveRemotePhoto(SavePath + "/" + filename, RemoteFileUrl)) {
                        // RemoteFileUrl = CDir + "/UserFiles/" + DirFileIO.GetDateDir() + "/" + filename;
                    }
                }
                pageStr = imgReg.Replace(pageStr, RemoteFileUrl);
                if (i == 0) {
                    TitleImg = RemoteFileUrl;
                    ImageArr = RemoteFileUrl;
                }
                else
                    ImageArr += "|||" + RemoteFileUrl;
            }
            replaceArray.Add(pageStr);
            replaceArray.Add(TitleImg);
            replaceArray.Add(ImageArr);
            return replaceArray;
        }

        /// <summary>
        /// 替换字符为远程的链接  比如 index.aspx 替换为http://www.qbzw.com/index.aspx
        /// </summary>
        /// <param name="PrimitiveUrl">参考Url/param>
        /// <param name="ConsultUrl">要替换的url</param>
        /// <returns></returns>
        public static string DefiniteUrl(string PrimitiveUrl, string ConsultUrl) {
            if (ConsultUrl.Substring(0, 7) != "http://")
                ConsultUrl = "http://" + ConsultUrl;
            ConsultUrl = ConsultUrl.Replace(@"\", "/");
            ConsultUrl = ConsultUrl.Replace("://", @":\\");
            PrimitiveUrl = PrimitiveUrl.Replace(@"\", "/");

            if (ConsultUrl.Substring(ConsultUrl.Length - 1) != "/") {
                if (ConsultUrl.IndexOf('/') > 0) {
                    if (ConsultUrl.Substring(ConsultUrl.LastIndexOf("/"), ConsultUrl.Length - ConsultUrl.LastIndexOf("/")).IndexOf('.') == -1)
                        ConsultUrl += "/";
                }
                else
                    ConsultUrl += "/";
            }
            string[] ConArray = ConsultUrl.Split('/');
            string returnStr = string.Empty;
            string[] PriArray;
            int pi = 0;
            if (PrimitiveUrl.Substring(0, 7) == "http://")
                returnStr = PrimitiveUrl.Replace("://", @":\\");
            else if (PrimitiveUrl.Substring(0, 1) == "/")
                returnStr = ConArray[0] + PrimitiveUrl;
            else if (PrimitiveUrl.Substring(0, 2) == "./") {
                PrimitiveUrl = PrimitiveUrl.Substring(PrimitiveUrl.Length - 2, 2);
                if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                    returnStr = ConsultUrl + PrimitiveUrl;
                else
                    returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + PrimitiveUrl;
            }
            else if (PrimitiveUrl.Substring(0, 3) == "../") {
                while (PrimitiveUrl.Substring(0, 3) == "../") {
                    PrimitiveUrl = PrimitiveUrl.Substring(3);
                    pi++;
                }
                for (int i = 0; i < ConArray.Length - 1 - pi; i++) {
                    if (returnStr.Length > 0)
                        returnStr = returnStr + ConArray[i];
                    else
                        returnStr = ConArray[i];
                }
                returnStr = returnStr + PrimitiveUrl;
            }
            else {
                if (PrimitiveUrl.IndexOf('/') > -1) {
                    PriArray = PrimitiveUrl.Split('/');
                    if (PriArray[0].IndexOf('.') > -1) {
                        if (PrimitiveUrl.Substring(PrimitiveUrl.Length - 1) == "/")
                            returnStr = "http://" + PrimitiveUrl;
                        {
                            if (PriArray[PriArray.Length - 1].IndexOf('.') > -1)
                                returnStr = "http:\\" + PrimitiveUrl;
                            else
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                        }
                    }
                    else {
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                            returnStr = ConsultUrl + PrimitiveUrl;
                        else
                            returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + PrimitiveUrl;
                    }
                }
                else {
                    if (PrimitiveUrl.IndexOf('.') > -1) {
                        string lastUrl = ConsultUrl;//.Substring(ConsultUrl.LastIndexOf('.'));
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/") {
                            if (lastUrl == "com" || lastUrl == "cn" || lastUrl == "net" || lastUrl == "org")
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                            else
                                returnStr = ConsultUrl + PrimitiveUrl;
                        }
                        else {
                            if (lastUrl == "com" || lastUrl == "cn" || lastUrl == "net" || lastUrl == "org")
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                            else
                                returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + "/" + PrimitiveUrl;
                        }
                    }
                    else {
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                            returnStr = ConsultUrl + PrimitiveUrl + "/";
                        else
                            returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + "/" + PrimitiveUrl + "/";
                    }
                }
            }

            if (returnStr.Substring(0, 1) == "/")
                returnStr = returnStr.Substring(1);
            if (returnStr.Length > 0) {
                returnStr = returnStr.Replace("//", "/");
                returnStr = returnStr.Replace(@":\\", "://");
            }
            else
                returnStr = "$False$";
            return returnStr;
        }

        /// <summary>
        /// 保存远程图片到本地
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="RemoteFileUrl"></param>
        /// <returns></returns>
        public static bool SaveRemotePhoto(string fileName, string RemoteFileUrl) {
            try {
                WebRequest request = WebRequest.Create(RemoteFileUrl);
                request.Timeout = 20000;
                Stream stream = request.GetResponse().GetResponseStream();
                Image getImage = Image.FromStream(stream);
                getImage.Save(fileName);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// 过滤脚本于标签
        /// </summary>
        /// <param name="ConStr">字符串</param>
        /// <param name="TagName">标签名称</param>
        /// <param name="FType">类型 1.2.3</param>
        /// <returns></returns>
        public static string ScriptHtml(string ConStr, string TagName, int FType) {
            Regex myReg;
            switch (FType) {
                case 1:
                    myReg = new Regex("<" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
                case 2:
                    myReg = new Regex("<" + TagName + "([^>])*>.*?</" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
                case 3:
                    myReg = new Regex("<" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    myReg = new Regex("</" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
            }
            return ConStr;
        }
        /// <summary>
        /// 过滤所有标签
        /// </summary>
        /// <param name="ConStr"></param>
        /// <returns></returns>
        public static string NoHtml(string ConStr) {
            Regex myReg = new Regex(@"(\<.[^\<]*\>)", RegexOptions.IgnoreCase);
            ConStr = myReg.Replace(ConStr, "");
            myReg = new Regex(@"(\<\/[^\<]*\>)", RegexOptions.IgnoreCase);
            ConStr = myReg.Replace(ConStr, "");
            ConStr = ConStr.Replace("\r\n", "");
            ConStr = ConStr.Replace(" ", "");
            return ConStr;
        }
        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="_strHTML"></param>
        /// <returns></returns>
        public static string[] GetImgTag(string _strHTML) {
            Regex reg = new Regex("IMG[^>]*?src\\s*=\\s*(?:\"(?<1>[^\"]*)\"|'(?<1>[^\']*)')", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string[] strAry = new string[reg.Matches(_strHTML).Count];
            int i = 0;
            foreach (Match match in reg.Matches(_strHTML)) {
                strAry[i] = GetImgUrl(match.Value);
                i++;
            }
            return strAry;
        }
        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="imgTagStr"></param>
        /// <returns></returns>
        private static string GetImgUrl(string imgTagStr) {
            //
            string str = "";
            //Regex reg = new Regex("http://.+.(?:jpg|gif|bmp|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex reg = new Regex("src\\s*=[\"|'](.+.[gif|jpg|bmp|jpeg|psd|png|svg|dxf|wmf|tiff])[\"|']", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match match in reg.Matches(imgTagStr)) {
                str = match.Groups[1].Value;
            }
            return str;
        }
    }
}