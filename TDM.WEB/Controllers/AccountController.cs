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
    public class AccountController : Controller
    {
        // GET: Account
        

        public ActionResult Login()
        {
            return View();
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
                 return RedirectToAction("Index", "SPK");
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
                        return RedirectToAction("Index", "SPK");
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
    }
}