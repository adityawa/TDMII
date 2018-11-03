using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL.Model
{
    public class WorkflowSettingModel:BaseModel
    {
        public int HeaderID { get; set; }
        public int ApprovalLevel { get; set; }
        public string Actor { get; set; }
        public string ActorID { get; set; }
        public string Action { get; set; }

        public int Order { get; set; }
        public string action_desc { get; set; }
        public string RoleDesc { get; set; }
    }

    public class WorkflowSettingHeader :BaseModel
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public int ApprovalLevel { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public List<WorkflowSettingModel> ls_details;
    }
}
