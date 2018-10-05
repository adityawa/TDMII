using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL.Model
{
    public class EmployeeModel:BaseModel
    {
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public int ReportTo { get; set; }
        public string ReportToName { get; set; }
        public int RoleApps { get; set; }
        public string RoleAppsName { get; set; }
        public string Dept { get; set; }
    }
}
