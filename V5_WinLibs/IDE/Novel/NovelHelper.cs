using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
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
    public class NovelHelper {
        private string _IndexPathDir = string.Empty;
        /// <summary>
        /// 索引库目录
        /// </summary>
        public string IndexPathDir {
            get {
                if (string.IsNullOrEmpty(_IndexPathDir)) {
                    throw new Exception("索引库目录为空!");
                }
                return _IndexPathDir;
            }
            set { _IndexPathDir = value; }
        }

        /// <summary>
        /// 创建索引库
        /// </summary>
        /// <param name="listNovel"></param>
        public void CreateIndex(List<ModelNovel> listNovel) {
            var dirPath = _IndexPathDir;
            var dirIndex = FSDirectory.Open(dirPath);
            var isExist = IndexReader.IndexExists(dirIndex);
            if (isExist) {
                //如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则解锁
                //Q:存在问题 如果一个用户正在对索引库写操作 此时是上锁的 而另一个用户过来操作时 将锁解开了 于是产生冲突 --解决方法后续
                if (IndexWriter.IsLocked(dirIndex)) {
                    IndexWriter.Unlock(dirIndex);
                }
            }
            //创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
            //补充:使用IndexWriter打开directory时会自动对索引库文件上锁
            IndexWriter writer = new IndexWriter(dirIndex, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);

            foreach (var model in listNovel) {
                Document document = new Document(); //new一篇文档对象 --一条记录对应索引库中的一个文档
                //向文档中添加字段  Add(字段,值,是否保存字段原始值,是否针对该列创建索引)
                document.Add(new Field("id", model.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));//--所有字段的值都将以字符串类型保存 因为索引库只存储字符串类型数据
                //Field.Store:表示是否保存字段原值。指定Field.Store.YES的字段在检索时才能用document.Get取出原值  //Field.Index.NOT_ANALYZED:指定不按照分词后的结果保存--是否按分词后结果保存取决于是否对该列内容进行模糊查询
                document.Add(new Field("title", model.Content, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                //Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                //WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离
                document.Add(new Field("content", model.Content, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                document.Add(new Field("createtime", model.CreateTime.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                writer.AddDocument(document); //文档写入索引库
                /*
                 if(book.IT == IndexType.Insert) {
                    document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    writer.AddDocument(document);
                } else if(book.IT == IndexType.Delete) {
                    writer.DeleteDocuments(new Term("id", book.Id.ToString()));
                } else if(book.IT == IndexType.Modify) {
                    //先删除 再新增
                    writer.DeleteDocuments(new Term("id", book.Id.ToString()));
                    document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    writer.AddDocument(document);
                }
                */
            }

            writer.Dispose();//会自动解锁
            dirIndex.Dispose(); //不要忘了Close，否则索引结果搜不到

        }

        /// <summary>
        /// 所以搜索
        /// </summary>
        /// <param name="keyWord">关键词</param>
        public List<ModelNovel> SearchIndex(string keyWord, int pageSize, int currentIndex, ref int recordCount) {

            if (pageSize < 10)
                pageSize = 10;
            if (currentIndex < 1)
                currentIndex = 1;

            var startIndex = (currentIndex - 1) * pageSize;

            string indexPath = _IndexPathDir;
            var directory = FSDirectory.Open(new System.IO.DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            //搜索条件
            PhraseQuery query = new PhraseQuery();
            //把用户输入的关键字进行分词
            foreach (string word in SplitWords(keyWord)) {
                query.Add(new Term("content", word));
            }
            //query.Add(new Term("content", keyWord));
            //query.Add(new Term("content", "C#"));//多个查询条件时 为且的关系
            query.Slop = 100; //指定关键词相隔最大距离

            //TopScoreDocCollector盛放查询结果的容器
            TopScoreDocCollector collector = TopScoreDocCollector.Create(pageSize, true);
            searcher.Search(query, null, collector);//根据query查询条件进行查询，查询结果放入collector容器

            //TopDocs 指定0到GetTotalHits() 即所有查询结果中的文档 如果TopDocs(20,10)则意味着获取第20-30之间文档内容 达到分页的效果
            //ScoreDoc[] docs = collector.TopDocs(0, collector.TotalHits).ScoreDocs;
            recordCount = collector.TotalHits;
            ScoreDoc[] docs = collector.TopDocs(startIndex, pageSize).ScoreDocs;

            //展示数据实体对象集合
            List<ModelNovel> listNovel = new List<ModelNovel>();
            for (int i = 0; i < docs.Length; i++) {
                int docId = docs[i].Doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document

                var model = new ModelNovel();
                //model.Content = doc.Get("content");
                //book.ContentDescription = doc.Get("content");//未使用高亮
                //搜索关键字高亮显示 使用盘古提供高亮插件
                //book.ContentDescription = Common.SplitContent.HightLight(Request.QueryString["SearchKey"], doc.Get("content"));
                model.Content = HightLight(keyWord, doc.Get("content"));
                model.Id = doc.Get("id");
                model.CreateTime = DateTime.Parse(doc.Get("createtime"));
                listNovel.Add(model);
            }
            return listNovel;
        }

        public string[] SplitWords(string content) {
            List<string> strList = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();//指定使用盘古 PanGuAnalyzer 分词算法
            TokenStream ts = analyzer.TokenStream("", new StringReader(content));
            //while ((token = tokenStream.ge) != null) { //Next继续分词 直至返回null
            //    strList.Add(token.Term); //得到分词后结果
            //}
            bool hasnext = ts.IncrementToken();
            Lucene.Net.Analysis.Tokenattributes.ITermAttribute ita;
            while (hasnext) {
                ita = ts.GetAttribute<Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();
                Console.WriteLine(ita.Term);
                if (!strList.Contains(ita.Term)) {
                    strList.Add(ita.Term);
                }
                hasnext = ts.IncrementToken();
            }
            return strList.ToArray();
        }

        public string HightLight(string keyword, string content) {
            //创建HTMLFormatter,参数为高亮单词的前后缀
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
                //new PanGu.HighLight.SimpleHTMLFormatter("<font style=\"font-style:normal;color:#cc0000;\"><b>", "</b></font>");
                new PanGu.HighLight.SimpleHTMLFormatter("$$", "$$");
            //创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
                            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
                            new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 1000;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keyword, content);
        }
    }
}
