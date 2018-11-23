using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL.Model
{
    public class WorklistModel :BaseModel
    {
        public int DocType { get; set; }
        public string DocTypeDesc { get; set; }
        public int DocId { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? RespondDate { get; set; }
        public int NextApprover { get; set; }
        public string NextApproverDesc { get; set; }
        public string Actioner { get; set; }
        public string ActionerDesc { get; set; }
        public int CurrLevel { get; set; }
        public int Actor { get; set; }
    }
}
