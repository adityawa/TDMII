using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDM.WEB.Controllers
{
    public class UserAppsController : BaseController
    {
        // GET: UserApps
        public ActionResult Index()
        {
            return CheckSession();
        }
    }
}