using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDM.BLL;
using TDM.BLL.Model;
namespace TDM.WEB.Controllers
{
    public class RoleController : BaseController
    {
        // GET: Role
        public ActionResult Index()
        {
            return CheckSession();
        }

        public JsonResult GetRole()
        {
            IEnumerable<RoleModel> roles = new List<RoleModel>();
            roles = new RoleBLL().List();
            var JsonResult = Json(new { Data = roles }, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        public JsonResult AddNewRoleApps()
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            int result = 0;
            RoleModel roleMdl = new RoleModel();
            if (Request.Form["Id"] == "")
            {
                roleMdl.RoleName = Request.Form["Role"].ToString();


                roleMdl.CreatedBy = "SYSTEM";
                roleMdl.CreatedDate = DateTime.Now;
                result = new RoleBLL().Insert(roleMdl, out _status);
            }
            else
            {
                roleMdl.RoleName = Request.Form["Role"].ToString();
                roleMdl.Id = Convert.ToInt32(Request.Form["Id"].ToString());

                roleMdl.ModifiedBy = "SYSTEM";
                roleMdl.ModifiedDate = DateTime.Now;
                result = new RoleBLL().Update(roleMdl, out _status);
            }

            return Json(new { Status = _status, Result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindById(string id)
        {

            RoleModel mdl = new RoleModel();
            if (id != "")
            {
                mdl = new RoleBLL().FindById(Convert.ToInt32(id));
            }

            return Json(new { Id = mdl.Id, RoleName = mdl.RoleName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveRole(string id)
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            int result_affected = 0;
            try
            {
                if (id != "")
                {
                    result_affected = new RoleBLL().Delete(Convert.ToInt32(id), out _status);
                    if (result_affected <= 0)
                    {
                        _status = "An Error occured when delete user application";
                    }
                }
            }
            catch (Exception ex)
            {
                _status = ex.Message;
            }
            return Json(new { Status = _status, Result = result_affected }, JsonRequestBehavior.AllowGet);
        }
    }
}