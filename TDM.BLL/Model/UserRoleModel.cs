using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL.Model
{
    public class UserRoleModel :BaseModel
    {
        public int RoleID { get; set; }
        public int EmployeeId { get; set; }

        public string RoleName { get; set; }
        public string EmpName { get; set; }
    }
}
