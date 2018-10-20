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

                ViewBag.RoleID = role.RoleName;
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
    }
}