using Amy.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amy.WebUI.Areas.Admin.Controllers
{
    public class RedisController : BasicController
    {
        //友情提示：reids缓存，建议对key进行特殊意义处理，例如{类型}_{模块}_{key}
        protected static Amy.Runtime.Caching.ICache client = new Amy.Runtime.Caching.Redis.RedisCache();

        protected static Amy.Runtime.Caching.IListCache lclient = new Amy.Runtime.Caching.Redis.RedisListCache();

        public ActionResult Basic(int page = 1)
        {
            var list = client.GetAll();

            var temp = list.Skip((page - 1) * 10).Take(10).Select<string, Models.CacheObject>(x =>
                {
                    var array = x.ToSplit('_');

                    var model = new Models.CacheObject()
                    {
                        Id = x,
                        Key = array[3],
                        CacheType = array[0],
                        Module = array[1],
                        ValueType=array[2],
                        Value = array[0] == "object" ? client.Get<object>(x).ToJsonByJsonNet() : lclient.GetList<object>(x).ToJsonByJsonNet(),
                    };

                    switch (model.ValueType)
                    {
                        case "string":
                            model.Value = client.Get<string>(model.Id);
                            break;
                        case "int":
                            model.Value = client.Get<int>(model.Id).ToString();
                            break;
                        case "siteconfig":
                            model.Value = client.Get<Models.SiteConfig>(model.Id).ToJsonByJsonNet();
                            break;
                        case "recarticle":
                            model.Value = lclient.GetList<Models.Article>(model.Id, 1, 2).ToJsonByJsonNet();
                            break;
                        case "reccomment":
                            model.Value = lclient.GetList<Models.Comment>(model.Id, 1, 2).ToJsonByJsonNet();
                            break;
                    }

                    return model;
                });

            return View(temp.ToPageList(page, 10, list.Count()));
        }

        public ActionResult Add()
        {
            ViewBag.Title = "添加缓存";

            return View(new Models.CacheObject() { CacheType = "object", Module = "global", ValueType = "string" });
        }

        public ActionResult Edit(string id)
        {
            ViewBag.Title = "编辑缓存";

            var array = id.ToSplit('_');

            var model = new Models.CacheObject()
            {
                Id = id,
                Key = array[3],
                CacheType = array[0],
                Module = array[1],
                ValueType = array[2],
                //Value = array[0] == "object" ? client.Get<object>(id).ToJsonByJsonNet() : lclient.GetList<object>(id).ToJsonByJsonNet()
            };

            switch (model.ValueType)
            {
                case "string":
                    model.Value = client.Get<string>(model.Id);
                    break;
                case "int":
                    model.Value = client.Get<int>(model.Id).ToString();
                    break;
                case "siteconfig":
                    model.Value = client.Get<Models.SiteConfig>(model.Id).ToJsonByJsonNet();
                    break;
                case "recarticle":
                    model.Value = lclient.GetList<Models.Article>(model.Id, 1, 2).ToJsonByJsonNet();
                    break;
                case "reccomment":
                    model.Value = lclient.GetList<Models.Comment>(model.Id, 1, 2).ToJsonByJsonNet();
                    break;
            }

            return View("Add", model);
        }

        public ActionResult Save(Models.CacheObject model)
        {
            if (!ModelState.IsValid)
                return JRFaild();

            bool result = false;

            var key = string.Format("{0}_{1}_{2}_{3}", model.CacheType, model.Module, model.ValueType, model.Key);

            switch (model.ValueType)
            {
                case "string":
                    result = client.Set<string>(key, model.Value);
                    break;
                case "int":
                    result = client.Set<int>(key, model.Value.ToInt32());
                    break;
                case "siteconfig":
                    result = client.Set<Models.SiteConfig>(key, model.Value.ToObjectByJsonNet<Models.SiteConfig>());
                    break;
                case "recarticle":
                    result = lclient.AddItemToList<Models.Article>(key, model.Value.ToObjectByJsonNet<Models.Article>());
                    break;
                case "reccomment":
                    result = lclient.AddItemToList<Models.Comment>(key, model.Value.ToObjectByJsonNet<Models.Comment>());
                    break;
            }

            return JRCommonHandleResult(result);
        }

        public ActionResult Persist()
        {
            client.SaveAsync();

            return JRCommonHandleResult(true);
        }

        public void Index()
        {
            Amy.Runtime.Caching.ICache redis = new Amy.Runtime.Caching.Redis.RedisCache();

            redis.FlushAll();

            //Amy.Runtime.Caching.IListCache<Test> redisList = new Amy.Runtime.Caching.Redis.RedisListCache<Test>();

            //redisList.PrependItemToList("test", new Test() { Id = "001", Name = "成龙", Age = 18 });
            //redisList.PrependItemToList("test", new Test() { Id = "002", Name = "刘德华", Age = 18 });
            //redisList.PrependItemToList("test", new Test() { Id = "003", Name = "胡歌", Age = 19 });

            //var temp = redisList.DequeueItemFromList("test");

            //Response.Write(temp.ToJsonByJsonNet());
            //Response.Write("<hr/>");

            //var list = redisList.GetList("test");

            //foreach (var v in list)
            //{
            //    Response.Write(v.ToJsonByJsonNet());
            //    Response.Write("<hr/>");
            //}
        }
    }
}