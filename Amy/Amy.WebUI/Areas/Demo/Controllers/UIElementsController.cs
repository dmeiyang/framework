using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amy.WebUI.Controllers;

namespace Amy.WebUI.Areas.Demo.Controllers
{
    public class UIElementsController : BasicController
    {
        public ActionResult Generals()
        {
            return View();
        }

        public ActionResult Buttons()
        {
            return View();
        }

        public ActionResult ProgressBars()
        {
            return View();
        }

        public ActionResult Modals()
        {
            return View();
        }

        public ActionResult EditModal()
        {
            return View();
        }

        public ActionResult SaveModal()
        {
            return JRSuccess("操作成功！");
        }

        public ActionResult Icons()
        {
            return View();
        }

        public ActionResult Panels()
        {
            return View();
        }
    }
}