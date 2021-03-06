﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TDM.BLL;
using TDM.BLL.Model;
namespace TDM.WEB.Models
{
    public  class Utilities
    {
        public static string GetUserNameLogon(UserAppsModel _user)
        {
            if (_user != null)
            {
                return _user.UserName;
            }
            else
            {
                return " ";
            }
        }

        public static int GetEmployeeID(UserAppsModel _user)
        {
            if (_user != null)
            {
                return new UserMgmtBLL().GetEmployeeIDByUserName(_user.UserName);
            }
            else
            {
                return 0;
            }
        }
    }
}