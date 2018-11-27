using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDM.BLL.Model;
using TDM.DAL;
using TDM.DAL.DAPPERDA;
namespace TDM.BLL
{
    public class WorklistArchiveBll
    {
        public List<WorklistArchiveModel> GetCompleteSPK(string creator)
        {
            List<WorklistArchiveModel> ls = new List<WorklistArchiveModel>();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var qry = context.tb_Worklist_Archive
                    .Include("tb_Master")
                    .Include("tb_employee")
                    .Include("tb_role")
                    .Where(x => x.IsCompleted == true && x.tb_Master.Category == MyEnums.enumMaster.DocumentType.ToString() && x.CreatedBy==creator);
                foreach (var item in qry)
                {
                    ls.Add(new WorklistArchiveModel
                    {
                        Id = (Int32)item.Id,
                        DocType = item.DocType,
                        DocTypeDesc=item.tb_Master.Value,
                        DocId=item.DocId,
                        Status=item.Status,
                        StartDate=item.StartDate,
                        RespondDate=item.RespondDate,
                        LastActorDesc=item.tb_employee.EmpName,
                        Actioner=item.Actioner,
                        LastRoleDesc=item.tb_role.RoleName,
                        LastLevel=(int)item.LastLevel
                    });
                }
            }
            return ls;
        }
    }
}
