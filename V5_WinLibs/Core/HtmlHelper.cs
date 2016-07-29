using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace V5_WinLibs.Core {
    public class HtmlHelper {
        static CollectionHelper http = new CollectionHelper();
        /// <summary>
        /// 过滤所有Html标签
        /// </summary>
        /// <param name="HTMLStr"></param>
        /// <returns></returns>
        public static string ParseTags(string HTMLStr) {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }
        /// <summary>
        /// 正则表达式标签替换
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ParseCollectionStrings(string s) {
            string[] chars = "\\,^,$,{,[,.,(,*,+,?,!,#,|".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < chars.Length; i++) {
                s = s.Replace(chars[i], "\\" + chars[i]);
            }
            s = Regex.Replace(s, @"\s+", "\\s+");
            return s;
        }
        public static string UnParseCollectionStrings(string s) {
            string[] chars = "\\,^,$,{,[,.,(,*,+,?,!,#,|".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < chars.Length; i++) {
                s = s.Replace("\\" + chars[i], chars[i]);
            }
            s = Regex.Replace(s, "\\s+", @"\s+");
            return s;
        }
        /// <summary>
        /// 获取图片链接地址 必须有Http开头
        /// </summary>
        /// <param name="imgTagStr"></param>
        /// <returns></returns>
        public static string GetImgUrl(string imgTagStr) {
            string str = "";
            Regex reg = new Regex("http://.+.(?:jpg|gif|bmp|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match match in reg.Matches(imgTagStr)) {
                str = match.Value;
            }
            return str;
        }
        /// <summary>
        /// 获取图片Src 
        /// </summary>
        /// <param name="imgTagStr"></param>
        /// <returns></returns>
        public static string GetImgUrlSrc(string imgTagStr) {
            string str = "";
            Regex reg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match match in reg.Matches(imgTagStr)) {
                str = match.Groups["imgUrl"].Value;//.Value;
            }
            return str;
        }
        /// <summary>
        /// 替换Html字符的图片标签到Ubb标签
        /// </summary>
        /// <param name="strHtml"></param>
        /// <param name="url"></param>
        /// <param name="removeWHtml"></param>
        /// <returns></returns>
        public static string ReplaceNormalHtml(string strHtml, string url, bool removeWHtml) {
            //<img.+?/>
            Regex reg = new Regex("<img.+?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //string[] strAry = new string[reg.Matches(strHtml).Count];
            //int i = 0;
            foreach (Match match in reg.Matches(strHtml)) {
                strHtml = strHtml.Replace(match.Value, "[IMG]" + CollectionHelper.DefiniteUrl(GetImgUrlSrc(match.Value), url) + "[/IMG]");
            }
            if (removeWHtml)
                return NoHTML(strHtml);
            else
                return strHtml;
        }
        /// <summary>
        /// 替换a标签的连接并下载
        /// </summary>
        /// <param name="strHtml">要替换的内容</param>
        /// <param name="url">参考地址</param>
        /// <param name="?">下载文件格式</param>
        /// <returns></returns>
        public static string ReplaceNornalHtmlByHref(string strHtml, string strUrl, string strDownFormat) {
            Regex reg = new Regex("<(?:jpg|gif|bmp|png)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //string[] strAry = new string[reg.Matches(strHtml).Count];
            //int i = 0;
            foreach (Match match in reg.Matches(strHtml)) {
                strHtml = strHtml.Replace(match.Value, "[A]" + CollectionHelper.DefiniteUrl(GetImgUrlSrc(match.Value), strUrl) + "[/A]");
            }
            return strHtml;
        }
        /// <summary>
        /// UBB代码处理函数
        /// </summary>
        /// <param name="sDetail">输入字符串</param>
        /// <returns>输出字符串</returns>
        public static string UBBToHTML(string sDetail) {
            Regex r;
            Match m;
            #region 处理空格
            sDetail = sDetail.Replace(" ", "&nbsp;");
            #endregion
            #region html标记符
            sDetail = sDetail.Replace("<", "&lt;");
            sDetail = sDetail.Replace(">", "&gt;");
            #endregion
            #region 处[b][/b]标记
            r = new Regex(@"(\[b\])([ \S\t]*?)(\[\/b\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<B>" + m.Groups[2].ToString() + "</B>");
            }
            #endregion
            #region 处[i][/i]标记
            r = new Regex(@"(\[i\])([ \S\t]*?)(\[\/i\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<I>" + m.Groups[2].ToString() + "</I>");
            }
            #endregion
            #region 处[u][/u]标记
            r = new Regex(@"(\[U\])([ \S\t]*?)(\[\/U\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<U>" + m.Groups[2].ToString() + "</U>");
            }
            #endregion
            #region 处[p][/p]标记
            //处[p][/p]标记
            r = new Regex(@"((\r\n)*\[p\])(.*?)((\r\n)*\[\/p\])", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<P class=\"pstyle\">" + m.Groups[3].ToString() + "</P>");
            }
            #endregion
            #region 处[sup][/sup]标记
            //处[sup][/sup]标记
            r = new Regex(@"(\[sup\])([ \S\t]*?)(\[\/sup\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<SUP>" + m.Groups[2].ToString() + "</SUP>");
            }
            #endregion
            #region 处[sub][/sub]标记
            //处[sub][/sub]标记
            r = new Regex(@"(\[sub\])([ \S\t]*?)(\[\/sub\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<SUB>" + m.Groups[2].ToString() + "</SUB>");
            }
            #endregion
            #region 处[img][/img]标记
            //处[img][/img]标记
            r = new Regex(@"(\[img\])([ \S\t]*?)(\[\/img\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<img src=\"" + m.Groups[2].ToString() + "\" />");
            }
            #endregion
            #region 处[url][/url]标记
            //处[url][/url]标记
            r = new Regex(@"(\[url\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<A href=\"" + m.Groups[2].ToString() + "\" target=\"_blank\"><IMG border=0 src=\"images/url.gif\">" +
                    m.Groups[2].ToString() + "</A>");
            }
            #endregion
            #region 处[url=xxx][/url]标记
            //处[url=xxx][/url]标记
            r = new Regex(@"(\[url=([ \S\t]+)\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<A href=\"" + m.Groups[2].ToString() + "\" target=\"_blank\"><IMG border=0 src=\"images/url.gif\">" +
                    m.Groups[3].ToString() + "</A>");
            }
            #endregion
            #region 处[email][/email]标记
            //处[email][/email]标记
            r = new Regex(@"(\[email\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<A href=\"mailto:" + m.Groups[2].ToString() + "\" target=\"_blank\"><IMG border=0 src=\"images/email.gif\">" +
                    m.Groups[2].ToString() + "</A>");
            }
            #endregion
            #region 处[email=xxx][/email]标记
            //处[email=xxx][/email]标记
            r = new Regex(@"(\[email=([ \S\t]+)\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<A href=\"mailto:" + m.Groups[2].ToString() + "\" target=\"_blank\"><IMG border=0 src=\"images/email.gif\">" +
                    m.Groups[3].ToString() + "</A>");
            }
            #endregion
            #region 处[size=x][/size]标记
            //处[size=x][/size]标记
            r = new Regex(@"(\[size=([1-7])\])([ \S\t]*?)(\[\/size\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<FONT SIZE=" + m.Groups[2].ToString() + ">" +
                    m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处[color=x][/color]标记
            //处[color=x][/color]标记
            r = new Regex(@"(\[color=([\S]+)\])([ \S\t]*?)(\[\/color\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<FONT COLOR=" + m.Groups[2].ToString() + ">" +
                    m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处[font=x][/font]标记
            //处[font=x][/font]标记
            r = new Regex(@"(\[font=([\S]+)\])([ \S\t]*?)(\[\/font\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<FONT FACE=" + m.Groups[2].ToString() + ">" +
                    m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处理图片链接
            //处理图片链接
            r = new Regex("\\[picture\\](\\d+?)\\[\\/picture\\]", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<A href=\"ShowImage.aspx?Type=ALL&Action=forumImage&ImageID=" + m.Groups[1].ToString() +
                    "\" target=\"_blank\"><IMG border=0 Title=\"点击打开新窗口查看\" src=\"ShowImage.aspx?Action=forumImage&ImageID=" + m.Groups[1].ToString() +
                    "\"></A>");
            }
            #endregion
            #region 处理[align=x][/align]
            //处理[align=x][/align]
            r = new Regex(@"(\[align=([\S]+)\])([ \S\t]*?)(\[\/align\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<P align=" + m.Groups[2].ToString() + ">" +
                    m.Groups[3].ToString() + "</P>");
            }
            #endregion
            #region 处[H=x][/H]标记
            //处[H=x][/H]标记
            r = new Regex(@"(\[H=([1-6])\])([ \S\t]*?)(\[\/H\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<H" + m.Groups[2].ToString() + ">" +
                    m.Groups[3].ToString() + "</H" + m.Groups[2].ToString() + ">");
            }
            #endregion
            #region 处理[list=x][*][/list]
            //处理[list=x][*][/list]
            r = new Regex(@"(\[list(=(A|a|I|i| ))?\]([ \S\t]*)\r\n)((\[\*\]([ \S\t]*\r\n))*?)(\[\/list\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                string strLI = m.Groups[5].ToString();
                Regex rLI = new Regex(@"\[\*\]([ \S\t]*\r\n?)", RegexOptions.IgnoreCase);
                Match mLI;
                for (mLI = rLI.Match(strLI); mLI.Success; mLI = mLI.NextMatch()) {
                    strLI = strLI.Replace(mLI.Groups[0].ToString(), "<LI>" + mLI.Groups[1]);
                }
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                    "<UL TYPE=\"" + m.Groups[3].ToString() + "\"><B>" + m.Groups[4].ToString() + "</B>" +
                    strLI + "</UL>");
            }
            #endregion
            #region 处理换行
            //处理换行，在每个新行的前面添加两个全角空格
            r = new Regex(@"(\r\n((&nbsp;)|　)+)(?<正文>\S+)", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch()) {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<BR>　　" + m.Groups["正文"].ToString());
            }
            //处理换行，在每个新行的前面添加两个全角空格
            sDetail = sDetail.Replace("\r\n", "<BR>");
            #endregion
            return sDetail;
        }
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string NoHTML(string Htmlstring) {
            if (Htmlstring.Trim() == "")
                return "";
            Htmlstring = Regex.Replace(Htmlstring, @"<br*?>", "$br$", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"\r\n", "$br$", RegexOptions.IgnoreCase);
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("$br$", "<br/>");
            return Htmlstring;
        }
    }
}
