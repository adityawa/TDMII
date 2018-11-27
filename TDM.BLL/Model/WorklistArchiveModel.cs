using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL.Model
{
    public class WorklistArchiveModel : BaseModel
    {
        public int DocType { get; set; }
        public string DocTypeDesc { get; set; }
        public int DocId { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? RespondDate { get; set; }
        public int LastActor { get; set; }
        public string LastActorDesc { get; set; }
        public string Actioner { get; set; }
        public int LastLevel { get; set; }
        public bool IsCompleted { get; set; }
        public int LastRole { get; set; }
        public string LastRoleDesc { get; set; }
    }
}
