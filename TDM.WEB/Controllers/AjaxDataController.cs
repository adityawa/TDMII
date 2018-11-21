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
            empddl.Add(new Select2DropDownModel
            {
                id="0",
                text="--choose--"
            });
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

        public JsonResult GetDocumentType()
        {
            IEnumerable<MasterModel> ls = new List<MasterModel>();
            List<Select2DropDownModel> empddl = new List<Select2DropDownModel>();
            ls = new MasterBLL().List("DocumentType");
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

        public JsonResult GetAvailableAction()
        {
            IEnumerable<MasterModel> ls = new List<MasterModel>();
            List<Select2DropDownModel> empddl = new List<Select2DropDownModel>();
            ls = new MasterBLL().List("WFAction");
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

        public JsonResult GetBranch()
        {
            IEnumerable<MasterModel> ls = new List<MasterModel>();
            List<Select2DropDownModel> empddl = new List<Select2DropDownModel>();
            ls = new MasterBLL().List("Branch");
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

        public JsonResult GetCity()
        {
            IEnumerable<MasterModel> ls = new List<MasterModel>();
            List<Select2DropDownModel> cityddl = new List<Select2DropDownModel>();
            ls = new MasterBLL().List("City");
            foreach (var item in ls)
            {
                cityddl.Add(new Select2DropDownModel
                {
                    id = item.Id.ToString(),
                    text = item.Value
                });
            }

            var JSONResult = Json(new { Data = cityddl }, JsonRequestBehavior.AllowGet);
            JSONResult.MaxJsonLength = Int32.MaxValue;
            return JSONResult;
        }

        public JsonResult GetRoleDdl()
        {
            IEnumerable<RoleModel> roles = new List<RoleModel>();
            List<Select2DropDownModel> roleddl = new List<Select2DropDownModel>();
            roles = new RoleBLL().List();
            foreach (var item in roles)
            {
                roleddl.Add(new Select2DropDownModel
                {
                    id = item.Id.ToString(),
                    text = item.RoleName
                });
            }
            var JsonResult = Json(new { Data = roleddl }, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        public JsonResult GetAction(string type, string currlevel)
        {
            List<string> lsActions = new List<string>();
            List<Select2DropDownModel> actionddl = new List<Select2DropDownModel>();
            if (Session["UserLogon"] != null)
            {
                string _usr = Utilities.GetUserNameLogon((UserAppsModel)Session["UserLogon"]);
                lsActions = new WorkflowSettingBLL().GetActionList(_usr, Convert.ToInt32( type), Convert.ToInt32( currlevel));
                foreach (string s in lsActions)
                {
                    actionddl.Add(new Select2DropDownModel
                    {
                        id=s.ToString(),
                        text=s.ToString()
                    });
                }
            }

            var JsonResult = Json(new { Data = actionddl }, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }
    }
}