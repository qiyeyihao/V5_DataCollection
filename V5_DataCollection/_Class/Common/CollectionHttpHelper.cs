
using DotNet4.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_Utility.Core;

namespace V5_DataCollection._Class.Common {

    /// <summary>
    /// 采集Http帮助
    /// </summary>
    public class CollectionHttpHelper {
        /// <summary>
        /// 浏览器对象
        /// </summary>
        private WebBrowser webBrowser = new WebBrowser();
        /// <summary>
        /// 结果输出
        /// </summary>
        /// <param name="html"></param>
        public delegate void OutHtml(string html);
        /// <summary>
        /// 
        /// </summary>
        public event OutHtml OutHtmlHandler;

        private bool _IsWebBrowser = false;
        /// <summary>
        /// 是否WebBrowser
        /// </summary>
        public bool WebBrowser {
            get { return _IsWebBrowser; }
            set { _IsWebBrowser = value; }
        }

        /// <summary>
        /// 获取网页内容
        /// </summary>
        /// <param name="url"></param>
        public void GetHtml(string url, string cookie) {
            if (!this.WebBrowser) {
                var http = new HttpHelper4();
                var httpItem = new HttpItem();
                httpItem.URL = url;
                if (!string.IsNullOrEmpty(cookie)) {
                    httpItem.Cookie = cookie;
                }
                var httpResult = http.GetHtml(httpItem);
                var html = httpResult.Html;
                if (OutHtmlHandler != null) {
                    OutHtmlHandler(html);
                }
            }
            else {
                webBrowser.ObjectForScripting = false;
                webBrowser.ScriptErrorsSuppressed = true;
                if (!string.IsNullOrEmpty(cookie)) {
                    webBrowser.Document.Cookie = cookie;
                }
                webBrowser.DocumentCompleted += (object sender, WebBrowserDocumentCompletedEventArgs e) => {
                    var html = webBrowser.Document.Body.Parent.OuterHtml;
                    if (OutHtmlHandler != null) {
                        OutHtmlHandler(html);
                    }
                };
                webBrowser.Navigate(new Uri(url));
            }

        }
    }

    public class TestHelper {

        public void Test1() {
            var c = new CollectionHttpHelper();
            c.WebBrowser = true;
            c.OutHtmlHandler += (string html) => {
                System.Console.WriteLine(html);
            };
            c.GetHtml("http://www.v5soft.com", string.Empty);
        }

    }
}
