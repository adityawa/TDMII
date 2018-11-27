using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDM.DAL;
using TDM.DAL.DAPPERDA;
using TDM.BLL.Model;
namespace TDM.BLL
{
    public class WorklistBLL
    {
        public List<WorklistModel> GetPendingItems(string usrName)
        {
            List<WorklistModel> colls = new List<WorklistModel>();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var _qry = context.tb_userApps.Where(x => x.UserName == usrName).SingleOrDefault();
                if (_qry != null)
                {
                    var _roles = context.tb_userRole.Where(x => x.EmployeeId == _qry.EmployeeId).Select(x => x.RoleId).ToList();

                    var _qryPending = context.tb_Worklist
                        .Include("tb_role")
                        .Include("tb_Master")
                        .Where(x => _roles.Contains((int)x.NextApprover) && x.tb_Master.Category == MyEnums.enumMaster.DocumentType.ToString());

                    foreach (var item in _qryPending)
                    {
                        colls.Add(new WorklistModel
                        {
                            Id = (Int32)item.Id,
                            DocType = item.DocType,
                            DocTypeDesc = item.tb_Master.Value,
                            Status = item.Status,
                            StartDate = item.StartDate,
                            RespondDate = item.RespondDate,
                            NextApprover = item.NextApprover == null ? 0 : (int)item.NextApprover,
                            NextApproverDesc = item.tb_role.RoleName,
                            CurrLevel = item.CurrLevel,
                            DocId = item.DocId
                        });
                    }
                }  
               
            }
            return colls;
        }

        public List<WorklistModel> GetSPKStatus(string creator)
        {
            List<WorklistModel> colls = new List<WorklistModel>();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                    var _qryPending = context.tb_Worklist
                        .Include("tb_role")
                        .Include("tb_Master")
                        .Where(x => x.CreatedBy==creator && x.tb_Master.Category == MyEnums.enumMaster.DocumentType.ToString());

                    foreach (var item in _qryPending)
                    {
                        colls.Add(new WorklistModel
                        {
                            Id = (Int32)item.Id,
                            DocType = item.DocType,
                            DocTypeDesc = item.tb_Master.Value,
                            Status = item.Status,
                            StartDate = item.StartDate,
                            RespondDate = item.RespondDate,
                            NextApprover = item.NextApprover == null ? 0 : (int)item.NextApprover,
                            NextApproverDesc = item.tb_role.RoleName,
                            CurrLevel = item.CurrLevel,
                            DocId = item.DocId
                        });
                    }
                

            }
            return colls;
        }

       
    }
}
