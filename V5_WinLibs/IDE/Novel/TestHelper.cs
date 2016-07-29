using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PanGu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V5_IDE._Class.Novel {
    public class TestHelper {

        String _InitSource = "盘古分词 简介: 盘古分词 是由eaglet 开发的一款基于字典的中英文分词组件\r\n" +
            "主要功能: 中英文分词，未登录词识别,多元歧义自动识别,全角字符识别能力\r\n" +
            "主要性能指标:\r\n" +
            "分词准确度:90%以上\r\n" +
            "处理速度: 300-600KBytes/s Core Duo 1.8GHz\r\n" +
            "用于测试的句子:\r\n" +
            "长春市长春节致词\r\n" +
            "长春市长春药店\r\n" +
            "IＢM的技术和服务都不错\r\n" +
            "张三在一月份工作会议上说的确实在理\r\n" +
            "于北京时间5月10日举行运动会\r\n" +
            "我的和服务必在明天做好";

        public void Test1() {
            
            PanGu.Segment.Init();

            var segment = new Segment();
            var ops = new PanGu.Match.MatchOptions();
            ops.MultiDimensionality = true;
            ICollection<WordInfo> words = segment.DoSegment(_InitSource, ops);
        }





        public void TestIndex() {
            var novel = new NovelHelper();
            novel.IndexPathDir = "D:\\abc";
            //创建索引
            List<ModelNovel> list = new List<ModelNovel>();
            for (int i = 0; i < 10000; i++) {
                list.Add(new ModelNovel() {
                    Id = Guid.NewGuid().ToString(),
                    Content = _InitSource + "content" + i,
                    CreateTime = DateTime.Now
                });
            }
            //novel.CreateIndex(list);
            //搜索索引
            int recordCount = 0;
            var ll = novel.SearchIndex("盘古分词 简介: 盘古分词 是由eaglet 开发的一款基于字典的中英文分词组件", 10, 1, ref recordCount);
            //var splits = novel.SplitWords("盘古分词 简介: 盘古分词 是由eaglet 开发的一款基于字典的中英文分词组件");
        }


        public void TestCreateIndex1() {
            var novel = new NovelHelper();
            novel.IndexPathDir = "D:\\123";

            List<ModelNovel> list = new List<ModelNovel>();
            using (var sr = new StreamReader("D:\\1.txt",Encoding.Default)) {
                string line = "";
                while ((line = sr.ReadLine()) != null) {
                    list.Add(new ModelNovel() {
                        Id = Guid.NewGuid().ToString(),
                        Content = line,
                        CreateTime = DateTime.Now
                    });
                }
            }
            novel.CreateIndex(list);
        }

        public void TestReadIndex1() {

            var novel = new NovelHelper();
            novel.IndexPathDir = "D:\\123";

            int recordCount = 0;
            var ll = novel.SearchIndex("修真", 10, 1, ref recordCount);
        }
    }
}
