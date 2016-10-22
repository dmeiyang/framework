using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amy.WebUI.Controllers;

namespace Amy.WebUI.Areas.Demo.Controllers
{
    public class FormsController : BasicController
    {
        public ActionResult Layouts()
        {
            return View();
        }

        public ActionResult Basic()
        {
            return View();
        }

        public ActionResult Validation()
        {
            return View();
        }

        public ActionResult Editable()
        {
            return View();
        }

        public ActionResult Wizard()
        {
            return View();
        }

        public ActionResult FileUpload()
        {
            return View();
        }

        public ActionResult Save()
        {
            return JRSuccess("操作成功！");
        }

        public JsonResult GetData(string type)
        {
            switch(type)
            {
                case "group":
                    return Json(new List<Data>() { new Data() { text = "admin", value = "0" }, new Data() { text = "user", value = "1" } }, JsonRequestBehavior.AllowGet);
                case "status":
                    return Json(new List<Data>() { new Data() { text = "未激活", value = "0" }, new Data() { text = "已激活", value = "1" } }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new List<Data>() { new Data() { text = "admin", value = "0" }, new Data() { text = "user", value = "1" } }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }

    public class Data
    {
        public string text { get; set; }

        public string value { get; set; }
    }
}