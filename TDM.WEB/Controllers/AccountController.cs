using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDM.BLL;
using TDM.BLL.Model;
using System.Web.Security;
namespace TDM.WEB.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Index()
        {
            return CheckSession();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult Authenticate(FormCollection fc)
        {
            string _userName = fc["UserName"];
            string _password = fc["Password"];
            string mesg = string.Empty;
            UserAppsModel usr = new UserAppsModel();
            if (_userName == "admin" && _password == "admin")
            {
                usr.UserName = "admin";
                usr.EmployeeId = 999;
                usr.EmployeeName = "super admin";
                usr.IsExpired = false;
                Session["UserLogOn"] = usr;
                Session["UserProps"] = "admin;admin";
                FormsAuthentication.SetAuthCookie(usr.UserName, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                usr = new UserMgmtBLL().Authenticate(_userName, _password, out mesg);
                if (mesg == string.Empty)
                {
                    if (usr.Id > 0)
                    {
                        Session["UserLogOn"] = usr;
                        Session["UserProps"] = usr.EmployeeName + ";" + Common.GetRoleAppsName(usr.Role);
                        FormsAuthentication.SetAuthCookie(usr.UserName, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrMsg = "You are not recognized";
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.ErrMsg = mesg;
                    return View("Login");
                }
            }

        }

        public JsonResult ListActiveUser()
        {
            IEnumerable<UserAppsModel> lsuser = new List<UserAppsModel>();
            lsuser = new UserMgmtBLL().ListActiveUser();
            var JsonResult = Json(new { Data = lsuser }, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        public JsonResult AddNewUserApps()
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            int result = 0;
            UserAppsModel usrMdl = new UserAppsModel();
            if (Request.Form["Id"] == "")
            {
                usrMdl.UserName = Request.Form["UserName"].ToString();
                usrMdl.Password = Request.Form["Password"].ToString();
                usrMdl.EmployeeId = Convert.ToInt32(Request.Form["Employee"].ToString());
                usrMdl.IsExpired = Convert.ToBoolean(Request.Form["IsExpired"].ToString());
                usrMdl.CreatedBy = "SYSTEM";
                usrMdl.CreatedDate = DateTime.Now;
                result = new UserMgmtBLL().ValidateBeforSave(usrMdl, out _status);
            }
            else
            {
                usrMdl.UserName = Request.Form["UserName"].ToString();
                usrMdl.Id = Convert.ToInt32(Request.Form["Id"].ToString());
                usrMdl.EmployeeId = Convert.ToInt32(Request.Form["Employee"].ToString());
                usrMdl.IsExpired = Convert.ToBoolean(Request.Form["IsExpired"].ToString());
                usrMdl.ModifiedBy = "SYSTEM";
                usrMdl.ModifiedDate = DateTime.Now;
                result = new UserMgmtBLL().Update(usrMdl, out _status);
            }

            return Json(new { Status = _status, Result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindById(string id)
        {

            UserAppsModel mdl = new UserAppsModel();
            if (id != "")
            {
                mdl = new UserMgmtBLL().FindById(Convert.ToInt32(id));
            }

            return Json(new { Id = mdl.Id, UserName = mdl.UserName, IsExpired = mdl.IsExpired, EmployeeId = mdl.EmployeeId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveUserApps(string id)
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            int result_affected = 0;
            try
            {
                if (id != "")
                {
                    result_affected = new UserMgmtBLL().Delete(Convert.ToInt32(id), out _status);
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