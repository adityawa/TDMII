﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL.Model
{
    public class UserAppsModel:BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsExpired { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
