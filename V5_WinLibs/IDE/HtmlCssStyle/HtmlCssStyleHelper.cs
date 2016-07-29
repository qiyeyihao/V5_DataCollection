using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_IDELibs.HtmlCssStyle {
    public class HtmlCssStyleHelper {
        public void Test() {
            var htmlContent = "";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var docNode = doc.DocumentNode;
            var node = docNode.CssSelect("table[bx-name='tables']").FirstOrDefault();
        }
    }
}
