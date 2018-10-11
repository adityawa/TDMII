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
    public class AjaxDataController : Controller
    {
        // GET: AjaxData
        public JsonResult GetAllEmployee()
        {
            IEnumerable<EmployeeModel> ls = new List<EmployeeModel>();
            List<Select2DropDownModel> empddl = new List<Select2DropDownModel>();
            ls = new EmployeeBLL().List();
            foreach (var item in ls)
            {
                empddl.Add(new Select2DropDownModel
                {
                    id = item.Id.ToString(),
                    text = item.EmpName
                });
            }

            var JSONResult = Json(new { Data = empddl }, JsonRequestBehavior.AllowGet);
            JSONResult.MaxJsonLength = Int32.MaxValue;
            return JSONResult;
        }

        public JsonResult GetUserRole()
        {
            IEnumerable<MasterModel> ls = new List<MasterModel>();
            List<Select2DropDownModel> empddl = new List<Select2DropDownModel>();
            ls = new MasterBLL().List("RoleApps");
            foreach (var item in ls)
            {
                empddl.Add(new Select2DropDownModel
                {
                    id = item.Id.ToString(),
                    text = item.Value
                });
            }

            var JSONResult = Json(new { Data = empddl }, JsonRequestBehavior.AllowGet);
            JSONResult.MaxJsonLength = Int32.MaxValue;
            return JSONResult;
        }
    }
}