using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDM.BLL;
using TDM.BLL.Model;
using TDM.WEB.Models;
namespace TDM.WEB.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return CheckSession();
        }

        public JsonResult GetWorklist()
        {
            List<WorklistModel> ls = new List<WorklistModel>();
            if (Session["UserLogOn"] != null)
            {
                ls = new WorklistBLL().GetPendingItems(Utilities.GetUserNameLogon((UserAppsModel)Session["UserLogOn"]));
            }
            return Json(new { Data = ls }, JsonRequestBehavior.AllowGet);
        }
    }
}