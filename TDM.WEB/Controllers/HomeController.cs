using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDM.WEB.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return CheckSession();
        }

        public ActionResult CreateSPK()
        {
            return View();
        }
    }
}