using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDM.BLL;
using TDM.BLL.Model;
namespace TDM.WEB.Controllers
{
    public class UserRoleController : BaseController
    {
        // GET: UserRole
        public ActionResult Index()
        {
            if (Request.QueryString["RoleId"] != null)
            {
                int role_id=Convert.ToInt32(Request.QueryString["RoleId"].ToString());
                RoleModel role = new RoleBLL().FindById(role_id);
                ViewBag.RoleID = role_id;
                ViewBag.Role = role.RoleName;
            }
            
            return CheckSession();
        }

        public JsonResult GetUserByRole(int id)
        {
            IEnumerable<UserRoleModel> ls = new List<UserRoleModel>();
            ls = new UserRoleBLL().ListUserRole(id);
            var JsonResult = Json(new { Data = ls }, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        public JsonResult AddNewUserRole()
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            int result = 0;
            UserRoleModel usrRoleMdl = new UserRoleModel();

            usrRoleMdl.RoleID = Convert.ToInt32( Request.Form["RoleID"].ToString());
            usrRoleMdl.EmployeeId = Convert.ToInt32( Request.Form["EmployeeID"].ToString());
                
            usrRoleMdl.CreatedBy = "SYSTEM";
            usrRoleMdl.CreatedDate = DateTime.Now;
            result = new UserRoleBLL().Insert(usrRoleMdl, out _status);
         
            return Json(new { Status = _status, Result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveUserInRole(string id)
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            int result_affected = 0;
            try
            {
                if (id != "")
                {
                    result_affected = new UserRoleBLL().Delete(Convert.ToInt32(id), out _status);
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