﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amy.WebUI.Controllers;

namespace Amy.WebUI.Areas.Admin.Controllers
{
    public class ManagerController : BasicController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialHeader()
        {
            return View();
        }

        public ActionResult PartialFooter()
        {
            return View();
        }
    }
}