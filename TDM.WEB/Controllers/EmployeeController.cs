using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
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

        private bool ValidateModel(EmployeeModel empl, out string mesg){
            bool IsValid=true;
            mesg = string.Empty;
            StringBuilder sb = new StringBuilder();
            
            if (empl.EmpName == string.Empty)
            {
                sb.AppendFormat("Employee Name must be filled", Environment.NewLine);
                IsValid = false;
            }

            if (empl.RoleApps == 0)
            {
                sb.AppendFormat("Role Application must be filled", Environment.NewLine);
                IsValid = false;
            }
            mesg = sb.ToString();
            return IsValid;
        }
        public JsonResult AddOrUpdateEmployee()
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            int result_affected = 0;
            if (Session["UserLogOn"] != null)
            {
                try
                {
                    EmployeeModel empl = new EmployeeModel();
                   
                    if (Request.Form["EmpId"] == "")
                    {
                        empl.EmpNo = Request.Form["EmpNo"];
                        empl.EmpName = Request.Form["EmpName"];
                        empl.ReportTo = Request.Form["ReportTo"] == "null" ? 0 : Int32.Parse(Request.Form["ReportTo"].ToString());
                        empl.RoleApps = Int32.Parse(Request.Form["Role"].ToString());
                        empl.Dept = Request.Form["Department"];
                        empl.CreatedBy = "SYSTEM";
                        empl.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        empl.Id = Convert.ToInt32(Request.Form["EmpId"]);
                        empl.EmpNo = Request.Form["EmpNo"];
                        empl.EmpName = Request.Form["EmpName"];
                        empl.Dept = Request.Form["Department"];
                        empl.ReportTo = Request.Form["ReportTo"] == "null" ? 0 : Int32.Parse(Request.Form["ReportTo"].ToString());
                        empl.RoleApps = Int32.Parse(Request.Form["Role"].ToString());
                        empl.ModifiedBy = "SYSTEM";
                        empl.ModifiedDate = DateTime.Now;
                    }
                    if (ValidateModel(empl, out _status))
                    {
                        result_affected = new EmployeeBLL().Insert(empl, out _status);
                    }
                   
                }
                catch (Exception ex)
                {
                    _status = ex.Message;
                }
            }

            return Json(new { Status = _status }, JsonRequestBehavior.AllowGet);
        }
    }
}