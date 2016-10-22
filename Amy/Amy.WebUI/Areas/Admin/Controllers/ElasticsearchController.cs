using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Amy.WebUI.Controllers;

namespace Amy.WebUI.Areas.Admin.Controllers
{
    public class ElasticsearchController : BasicController
    {
        protected Amy.Unit.Elasticsearch.IConnection<Models.Article> client = Amy.Unit.Elasticsearch.NET.ConnectionBase<Models.Article>.GetInstance();

        public static readonly string index = "db_test";
        public static readonly string index_type = "test";

        /// <summary>
        /// 索引管理
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        [HttpPost]
        public ActionResult CreateIndex(string index, string type, int shares = 5, int replicas = 1)
        {
            var result = client.CreateIndex(index, shares, replicas);

            if (!result)
                return JRCommonHandleResult(result);

            var data = new
            {
                Id = new { type = "string", index = "not_analyzed" },
                Title = new { type = "string", analyzer = "ik" },
                Description = new { type = "string", analyzer = "ik" },
                Author = new { type = "string", index = "not_analyzed" },
                AuthorSpelling = new { type = "string", index = "not_analyzed" },
                Sources = new { type = "string", index = "not_analyzed" },
                Tags = new { type = "string", index = "not_analyzed" },
                Hits = new { type = "integer" },
                Tips = new { type = "double" },
                IsDisplay = new { type = "boolean" },
                Comments = new
                {
                    properties = new
                    {
                        Id = new { type = "string", index = "not_analyzed" },
                        Content = new { type = "string", analyzer = "ik" },
                        DateTime = new { type = "date" },
                    }
                },
                DateTime = new { type = "date" },
            };

            result = client.CreateMapping(index, type, data);

            return JRCommonHandleResult(result);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        [HttpPost]
        public ActionResult InitData(string index, string type)
        {
            var temp1 = new Models.Article()
            {
                Id = IDHelper.Id32,
                Title = "每一个老板心里都住着一个郭德纲",
                Description = "只要别挖前东家的人同时又做和前东家一样的业务，老板们也就只好睁一只眼闭一只眼。所以才有金山系做游戏开枝散叶，网易门户前主编们全面开花，阿里中供铁军撑起了O2O半壁江山。",
                Author = "高山流水",
                AuthorSpelling = "万卷出版公司".ToSpelling(true),
                Sources = new List<string>() { "不上班" },
                Tags = new string[] { "互联网" },
                Hits = 1009,
                Tips = 545,
                IsDisplay = true,
                Comments = new List<Models.Comment>() { new Models.Comment() { Id = IDHelper.Id32, Content = "精辟，敬仰如高山流水之绵绵不绝", DateTime = DateTime.Now } },
                DateTime = DateTime.Now,
            };

            var temp2 = new Models.Article()
            {
                Id = IDHelper.Id32,
                Title = "这内容创业的盛世，如你所愿",
                Description = "100年前有今日头条的话，你会看到下面的标题：“京汉铁路延长线开工，学界耆宿称破坏龙脉影响风水”，“外国传教士收养中国小孩做药引”，“汪精卫陈璧君大婚，民国第一美男子被绿茶婊搞定”，当然还有“民国药丸，大清国回来吧”。",
                Author = "我欲成魔为Ni",
                AuthorSpelling = "三峡电子音像出版社".ToSpelling(true),
                Sources = new List<string>() { "虎嗅" },
                Tags = new string[] { "互联网", "IT" },
                Hits = 898,
                Tips = 300,
                IsDisplay = true,
                Comments = new List<Models.Comment>() { new Models.Comment() { Id = IDHelper.Id32, Content = "辞藻之华丽，美轮美奂，小弟拜服！！！", DateTime = DateTime.Now } },
                DateTime = DateTime.Now,
            };

            var list = new List<Models.Article>();

            list.Add(temp1);
            list.Add(temp2);

            return JRCommonHandleResult(client.BulkPutDocument(index, type, list));
        }

        /// <summary>
        /// 基础示例
        /// </summary>
        public ActionResult Basic(string key, string author, int? hit, string hits, int? minTip, int? maxTip, DateTime? startTime, DateTime? endTime, int offset = 0, int limit = 10)
        {
            ViewBag.Key = key;
            ViewBag.Author = author;
            ViewBag.Hits = hits;

            #region 组装查询条件
            #region 1、必须包含条件组
            var mustQuery = new List<object>();

            // 标题或者描述包含key
            if (!string.IsNullOrEmpty(key))
            {
                mustQuery.Add(new { multi_match = new { fields = new string[] { "Title", "Description" }, query = key } });
            }
            #endregion

            #region 2、必须不包含条件组
            var mustNotQuery = new List<object>();
            #endregion

            #region 3、提升得分条件组
            var shouldQuery = new List<object>();
            #endregion

            #region 4、筛选条件组
            // https://www.elastic.co/guide/en/elasticsearch/reference/current/term-level-queries.html
            var filterQuery = new List<object>();

            // 作者包含author
            if (!string.IsNullOrEmpty(author))
            {
                filterQuery.Add(new { query_string = new { default_field = "Author", query = author } });
            }

            // 点击数等于hit，单值
            if (hit != null)
            {
                filterQuery.Add(new { term = new { Hits = hit } });
            }

            // 点击数等于hits，多值
            if (!string.IsNullOrEmpty(hits))
            {
                filterQuery.Add(new { terms = new { Hits = hits.ToSplit(',') } });
            }

            // 小费金额>=或者<=（gte代表大于等于，gt代表大于，lte代表小于等于，lt代表小于）
            dynamic tipCondition = new System.Dynamic.ExpandoObject();

            if (minTip != null)
            {
                tipCondition.gte = minTip;
            }

            if (maxTip != null)
            {
                tipCondition.lte = maxTip;
            }

            if (minTip != null || maxTip != null)
            {
                filterQuery.Add(new { range = new { Tips = tipCondition } });
            }

            // 发布时间>=或者<=（gte代表大于等于，gt代表大于，lte代表小于等于，lt代表小于）
            dynamic timeCondition = new System.Dynamic.ExpandoObject();

            if (startTime != null)
            {
                timeCondition.gte = startTime;
            }

            if (endTime != null)
            {
                timeCondition.lte = endTime;
            }

            if (startTime != null || endTime != null)
            {
                filterQuery.Add(new { range = new { DateTime = timeCondition } });
            }
            #endregion

            #region 5、数据排序
            #endregion

            #region 6、分组聚合
            var aggQuery = new
            {
                group_by_author = new
                {
                    terms = new
                    {
                        field = "Author",//分组字段
                        size = 6,//数据条数
                        order = new List<object>() {
                            new { _term = "desc" }
                        }
                    },
                    aggs = new
                    {
                        average_tips = new
                        {
                            avg = new { field = "Hits" }
                        }
                    }

                }
            };
            #endregion

            #region 7、组装查询条件
            var data = new
            {
                query = new
                {
                    @bool = new { must = mustQuery, must_not = mustNotQuery, should = shouldQuery, filter = filterQuery }
                },
                sort = new List<object>()
                {
                    //new { _score = new { order = "desc" } },
                    new { DateTime = new { order = "desc" } },
                },
                highlight = new
                {
                    pre_tags = new string[] { "<b style=\"color:red;\">" },
                    post_tags = new string[] { "</b>" },
                    fields = new { Title = new { }, Description = new { } }
                },
                from = offset,
                size = limit,
                aggs = aggQuery,
            };
            #endregion
            #endregion

            var searchResult = client.Search(index, index_type, data);

            var result = searchResult.hits.hits.Select(x => new Models.Article()
            {
                Id = x._source.Id,
                Title = x.highlight == null ? x._source.Title : x.highlight.GetValue("Title") == null ? x._source.Title : x.highlight.GetValue("Title")[0].ToString(),
                Description = x.highlight == null ? x._source.Description : x.highlight.GetValue("Description") == null ? x._source.Description : x.highlight.GetValue("Description")[0].ToString(),
                Author = x._source.Author,
                Sources = x._source.Sources,
                Tags = x._source.Tags,
                Hits = x._source.Hits,
                Tips = x._source.Tips,
                IsDisplay = x._source.IsDisplay,
                Comments = x._source.Comments,
                DateTime = x._source.DateTime
            }).ToPageList((offset + limit) / limit, limit, (int)searchResult.hits.total);

            return View(new Models.SearchResult<Models.Article>() { Aggregations = searchResult.aggregations, DataList = result });
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        public ActionResult Add()
        {
            ViewBag.Title = "添加文档";

            return PartialView(new Models.Article());
        }

        /// <summary>
        /// 编辑文档
        /// </summary>
        public ActionResult Edit(string id)
        {
            ViewBag.Title = "编辑文档";

            var entity = client.GetDocument(index, index_type, id);

            return entity == null ? PartialView("Add", new Models.Article()) : PartialView("Add", entity);
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        [HttpPost]
        public ActionResult Save(Models.Article model)
        {
            if (!ModelState.IsValid)
                return JRFaild();

            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = IDHelper.Id32;
                model.AuthorSpelling = model.Author.ToSpelling(true);
                model.Sources = Request.Form["Sources"].ToSplit(',').ToList();
                model.Tags = Request.Form["Tags"].ToSplit(',');
                model.Tips = 0;
                model.Hits = 0;
                model.DateTime = DateTime.Now;
                model.Comments = new List<Models.Comment>();

                return JRCommonHandleResult(client.PutDocument(index, index_type, model.Id, model));
            }
            else
            {
                var entity = client.GetDocument(index, index_type, model.Id);

                if (entity == null)
                    return JRFaild("所选文档已经不存在了，请重新选择文档！！！");

                entity.Title = model.Title;
                entity.Description = model.Description;
                entity.Author = model.Author;
                entity.AuthorSpelling = model.Author.ToSpelling(true);
                entity.Sources = Request.Form["Sources"].ToSplit(',').ToList();
                entity.Tags = Request.Form["Tags"].ToSplit(',');
                entity.IsDisplay = model.IsDisplay;

                return JRCommonHandleResult(client.PutDocument(index, index_type, entity.Id, entity));
            }
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        public ActionResult Delete(string ids)
        {
            var array = ids.ToSplit(',');

            if (array.Length <= 0)
                return JRFaild("删除失败，您没有选择要删除的对象，请选择！！！");

            return JRCommonHandleResult(client.BulkDeleteDocument(index, index_type, array));
        }

        /// <summary>
        /// 局部更新文档
        /// </summary>
        public ActionResult Update(string id)
        {
            var data = new { doc = new { Hits = 1 } };

            return JRCommonHandleResult(client.UpdateDocument(index, index_type, id, data));
        }

        /// <summary>
        /// 局部追加文档
        /// </summary>
        public ActionResult Append(string id)
        {
            var data = new
            {
                script = new
                {
                    inline = "ctx._source.Hits += hit",
                    @params = new { hit = 1 }
                }
            };

            return JRCommonHandleResult(client.UpdateDocument(index, index_type, id, data));
        }

        #region 废弃代码
        public void Search(Models.ArticleCondition model)
        {
            model.Limit = model.Limit == 0 ? 10 : model.Limit;

            var data = new
            {
                query = new
                {
                    @bool = new
                    {
                        must = new List<object>()
                        {
                            //new { multi_match = new { fields=new string[] { "Title", "Description" }, query = model.Key } },
                            //new { query_string = new { default_field = "Title", query=model.Key } },
                            //new { query_string = new { default_field = "Description", query=model.Key } }
                        },
                        must_not = new List<object>()
                        {
                            //new { query_string = new { default_field = "Title", query=model.Key } }
                        },
                        should = new List<object>()
                        {
                            //new { query_string = new { default_field = "Title", query="郭德纲" } }
                        },
                        filter = new List<object>()
                        {
                            //new { query_string = new { default_field = "Title", query="郭德纲" } }
                        }
                    }
                },
                aggs = new
                {
                    //avg_grade = new { avg = new { field = "Tips" } },//求Tips平均数，avg_grade为系统函数名
                    //genres = new { terms = new { field = "Tips" } },//求根据Tips分组后各组总数，genres
                    //tips_ranges = new { range = new { field = "Tips", ranges = new List<object>() { new { to = 300 }, new { @from = 300, to = 600 } } } },//求根据int范围分组后各组总数，tips_ranges为自定义函数名
                    //datetime_ranges = new { range = new { field = "DateTime", format="yyyy/MM/dd", ranges = new List<object>() { new { to = "2016/10/10" }, new { @from = "2016/10/09", to = "2016/10/10" } } } },//求根据date范围分组后各组总数，datetime_ranges为自定义函数名
                    //tips_without = new { missing = new { field = "XXX" } },//求文档中不包含字段XXX的数据总数，tips_without为自定义函数名
                    group_by_author = new
                    {
                        terms = new
                        {
                            field = "Author",
                            size = 10,
                            order = new List<object>() {
                                new { _term = "desc" }
                            }
                        },
                        aggs = new
                        {
                            average_tips = new
                            {
                                avg = new { field = "Tips" }
                            }
                        }

                    }
                },
                sort = new List<object>()
                {
                    new { Tips = new { order = "desc" } },
                    new { DateTime = new { order = "desc" } },
                    new { _score = new { order = "desc" } },
                },
                from = model.Offset,
                size = model.Limit,
            };

            var searchResult = client.Search("db_test", "test", data);

            var result = searchResult.hits.hits.Select(x => new Models.Article()
            {
                Id = x._source.Id,
                Title = x._source.Title,
                Description = x._source.Description,
                Author = x._source.Author,
                Sources = x._source.Sources,
                Tags = x._source.Tags,
                Hits = x._source.Hits,
                Tips = x._source.Tips,
                IsDisplay = x._source.IsDisplay,
                Comments = x._source.Comments,
                DateTime = x._source.DateTime
            });

            foreach (var v in result)
            {
                Response.Write(v.ToJsonByJsonNet());
                Response.Write("<hr/>");
            }

            Response.Write(searchResult.aggregations);
        }
        #endregion
    }
}