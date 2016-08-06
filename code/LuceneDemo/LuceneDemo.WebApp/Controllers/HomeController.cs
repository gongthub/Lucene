using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneDemo.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuceneDemo.WebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            //AddDatas();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult Index(string words)
        {
            List<Model.Content> models = SearchFromIndexData(words);

            return View(models);
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            var retJson = new { state = true, msg = "" };
            try
            {
                CreateIndexByData();
                retJson = new { state = true, msg = "添加索引成功！" };
                return Json(retJson,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retJson = new { state = false, msg = "添加索引失败！" + ex.Message };
                return Json(retJson, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        private void CreateIndexByData()
        {
            string indexPath = Server.MapPath("~/IndexData");//索引文档保存位置         
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            //IndexReader:对索引库进行读取的类
            bool isExist = IndexReader.IndexExists(directory); //是否存在索引库文件夹以及索引库特征文件
            if (isExist)
            {
                //如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则解锁
                //Q:存在问题 如果一个用户正在对索引库写操作 此时是上锁的 而另一个用户过来操作时 将锁解开了 于是产生冲突 --解决方法后续
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            //创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
            //补充:使用IndexWriter打开directory时会自动对索引库文件上锁
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
            BLL.Content content = new BLL.Content();
            List<Model.Content> models = content.GetModels();
            //--------------------------------遍历数据源 将数据转换成为文档对象 存入索引库
            foreach (var model in models)
            {
                Document document = new Document(); //new一篇文档对象 --一条记录对应索引库中的一个文档
                //向文档中添加字段  Add(字段,值,是否保存字段原始值,是否针对该列创建索引)
                document.Add(new Field("id", model.ID.ToString(), Field.Store.YES, Field.Index.ANALYZED));//--所有字段的值都将以字符串类型保存 因为索引库只存储字符串类型数据

                //Field.Store:表示是否保存字段原值。指定Field.Store.YES的字段在检索时才能用document.Get取出原值  
                //Field.Index.NOT_ANALYZED:指定不按照分词后的结果保存--是否按分词后结果保存取决于是否对该列内容进行模糊查询
                document.Add(new Field("title", model.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                //Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                //WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离
                document.Add(new Field("info", model.Info, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(document); //文档写入索引库
            }
            writer.Close();//会自动解锁
            directory.Close(); //不要忘了Close，否则索引结果搜不到
        }

        /// <summary>
        /// 查询
        /// </summary>
        private List<Model.Content> SearchFromIndexData(string searchStrs)
        {
            string indexPath = Server.MapPath("~/IndexData");
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            //搜索条件
            PhraseQuery query = new PhraseQuery();
            //把用户输入的关键字进行分词
            foreach (string word in SplitContent.SplitWords(searchStrs))
            {
                query.Add(new Term("info", word));
            }
            //query.Add(new Term("content", "C#"));//多个查询条件时 为且的关系
            query.SetSlop(100); //指定关键词相隔最大距离

            //TopScoreDocCollector盛放查询结果的容器
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            searcher.Search(query, null, collector);//根据query查询条件进行查询，查询结果放入collector容器
            //TopDocs 指定0到GetTotalHits() 即所有查询结果中的文档 如果TopDocs(20,10)则意味着获取第20-30之间文档内容 达到分页的效果
            ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;

            //展示数据实体对象集合
            List<Model.Content> models = new List<Model.Content>();
            for (int i = 0; i < docs.Length; i++)
            {
                int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document
                Model.Content model = new Model.Content();
                model.Title = doc.Get("title");
                //book.ContentDescription = doc.Get("content");//未使用高亮
                //搜索关键字高亮显示 使用盘古提供高亮插件
                model.Info = SplitContent.HightLight(searchStrs, doc.Get("info"));
                model.ID = Convert.ToInt32(doc.Get("id"));
                models.Add(model);
            }
            return models;
        }


        private void AddDatas()
        {
            BLL.Content ser = new Content();
            Model.Content model = new Model.Content();
            for (int i = 0; i < 1000000; i++)
            {
                model.Title = "Lucene应用" + i.ToString();
                model.Info = "Lucene打手机打开手机的拉开手机都卡死机读卡打开了手机的卡拉是京东卡拉斯角度看打了卡手机打了卡就是靠大家爱思考诶UR欧我玩儿vjdfhsdkjfhsdfnjs" + i.ToString();
                model.Author = "gongtao" + i.ToString();
                model.CreateTime = DateTime.Now;
                ser.Add(model);
            }
        }

    }
}
