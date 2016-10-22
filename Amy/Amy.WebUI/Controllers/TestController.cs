using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amy.WebUI.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DataList(DateTime? start, DateTime? end, int page = 1)
        {
            var list = new List<Test>();

            list.Add(new Test()
            {
                Id = 1,
                DateTime = Convert.ToDateTime("2016-10-11"),
                CreateDay = "2016-10-11",
                CreateTime="09:12",
                Gold = 10000,
                Sno = "001",
                Money = 100,
                Type = "支付宝支付",
                Status = "交易失败"
            });

            list.Add(new Test()
            {
                Id = 2,
                DateTime = Convert.ToDateTime("2016-9-13"),
                CreateDay = "2016-9-13",
                CreateTime = "09:12",
                Gold = 10001,
                Sno = "002",
                Money = 200,
                Type = "支付宝支付",
                Status = "交易失败"
            });

            list.Add(new Test()
            {
                Id = 3,
                DateTime = Convert.ToDateTime("2016-07-13"),
                CreateDay = "2016-07-13",
                CreateTime = "09:12",
                Gold = 10000,
                Sno = "003",
                Money = 100,
                Type = "支付宝支付",
                Status = "交易失败"
            });

            list.Add(new Test()
            {
                Id = 4,
                DateTime = Convert.ToDateTime("2015-10-13"),
                CreateDay = "2015-10-13",
                CreateTime = "09:12",
                Gold = 10000,
                Sno = "004",
                Money = 100,
                Type = "支付宝支付",
                Status = "交易失败"
            });

            if (start != null)
            {
                list = list.Where(x => x.DateTime >= Convert.ToDateTime(start)).ToList();
            }

            if (end != null)
            {
                list = list.Where(x => x.DateTime <= Convert.ToDateTime(end)).ToList();
            }

            list = list.Skip((page - 1) * 10).Take(10).ToList();

            var result = new { total = list.Count(), list = list };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public class Test
        {
            public int Id { get; set; }

            public DateTime DateTime { get; set; }

            public string CreateDay { get; set; }

            public string CreateTime { get; set; }

            public int Gold { get; set; }

            public string Sno { get; set; }

            public int Money { get; set; }

            public string Type { get; set; }

            public string Status { get; set; }
        }

        public ActionResult Shearphoto()
        {
            return View();
        }
    }
}