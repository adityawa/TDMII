using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDM.DAL;
using TDM.DAL.DAPPERDA;
using TDM.BLL.Model;
using AutoMapper;
namespace TDM.BLL
{

    public class WorkflowSettingBLL :BaseDAWithDapper
    {
        private MapperConfiguration config = null;
        private IMapper imap;
        private int result_affected = 0;
        public WorkflowSettingBLL()
        {
            if (config == null)
            {
                config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<WorkflowSettingModel, tb_workflowSetting>();
                    cfg.CreateMap<tb_workflowSetting, WorkflowSettingModel>();
                });
            }
        }
       
      

        public int Insert(List<WorkflowSettingModel> lsmodel, out string errMsg)
        {
            errMsg = string.Empty;
            tb_workflowSetting wfentity = new tb_workflowSetting();
            imap = config.CreateMapper();

            using (TDMDBEntities context = new TDMDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in lsmodel)
                        {
                            wfentity = imap.Map<WorkflowSettingModel, tb_workflowSetting>(item);
                            context.tb_workflowSetting.Add(wfentity);
                            result_affected += context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }

                }
                
            }
            return result_affected;
        }

        
    }
}
