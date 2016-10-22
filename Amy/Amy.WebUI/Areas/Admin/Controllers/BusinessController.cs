using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amy.WebUI.Areas.Admin.Controllers
{
    public class BusinessController : Controller
    {
        public ActionResult Index()
        {
            return View(Models.BusinessExample.GetList());
        }

        
    }
}