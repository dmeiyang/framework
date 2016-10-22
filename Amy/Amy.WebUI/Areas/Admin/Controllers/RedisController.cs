using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amy.WebUI.Areas.Admin.Controllers
{
    public class RedisController : Controller
    {
        public void Index()
        {
            Amy.Runtime.Caching.ICache redis = new Amy.Runtime.Caching.Redis.RedisCache();

            redis.FlushAll();

            Amy.Runtime.Caching.IListCache<Test> redisList = new Amy.Runtime.Caching.Redis.RedisListCache<Test>();

            redisList.PrependItemToList("test", new Test() { Id = "001", Name = "成龙", Age = 18 });
            redisList.PrependItemToList("test", new Test() { Id = "002", Name = "刘德华", Age = 18 });
            redisList.PrependItemToList("test", new Test() { Id = "003", Name = "胡歌", Age = 19 });

            var temp = redisList.DequeueItemFromList("test");

            Response.Write(temp.ToJsonByJsonNet());
            Response.Write("<hr/>");

            //var list = redisList.GetList("test");

            //foreach (var v in list)
            //{
            //    Response.Write(v.ToJsonByJsonNet());
            //    Response.Write("<hr/>");
            //}
        }
    }

    public class Test
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}