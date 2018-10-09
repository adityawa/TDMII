using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDM.BLL;
using TDM.BLL.Model;
namespace TDM.WEB.Controllers
{
    public class EmployeeController : BaseController
    {
        // GET: Employee

        public ActionResult Index()
        {
            return CheckSession();
        }

        public JsonResult ListEmployee()
        {
            IEnumerable<EmployeeModel> ls = new List<EmployeeModel>();
            ls = new EmployeeBLL().List();
            var JsonResult = Json(new { Data = ls }, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }
    }
}