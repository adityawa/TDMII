using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL.Model
{
    public class WorkflowSettingModel:BaseModel
    {
        public int TypeId { get; set; }
        public int ApprovalLevel { get; set; }
        public string Actor { get; set; }
        public string ActorID { get; set; }
        public string Action { get; set; }
    }
}
