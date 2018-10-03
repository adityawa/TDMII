using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDM.WEB.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult CheckSession()
        {
            if (Session["UserLogOn"] == null)
            {

                return Redirect(@Url.Action("Login", "Account"));
            }
            else
            {
                return View();
            }

        }
    }
}