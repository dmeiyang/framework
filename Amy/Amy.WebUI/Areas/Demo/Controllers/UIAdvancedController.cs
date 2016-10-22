using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amy.WebUI.Controllers;

namespace Amy.WebUI.Areas.Demo.Controllers
{
    public class UIAdvancedController : BasicController
    {
        public ActionResult Preloader()
        {
            return View();
        }

        public ActionResult NestableList()
        {
            return View();
        }

        public ActionResult DropdownList()
        {
            return View();
        }

        public ActionResult Notific()
        {
            return View();
        }

        public ActionResult Toastr()
        {
            return View();
        }
    }
}