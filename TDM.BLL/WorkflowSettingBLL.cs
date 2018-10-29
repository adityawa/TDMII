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
       
      

        public int Insert(WorkflowSettingHeader wfhdr, List<WorkflowSettingModel> lsmodel, out string errMsg)
        {
            errMsg = string.Empty;
            tb_workflowSetting wfdetil = new tb_workflowSetting();
            imap = config.CreateMapper();
            int _hdrId=0;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        tb_workflowSettingHdr _entHdr = new tb_workflowSettingHdr();
                        _entHdr.ApprovalLevel = wfhdr.ApprovalLevel;
                        _entHdr.Version = wfhdr.Version;
                        _entHdr.TypeID = wfhdr.TypeID;
                        _entHdr.CreatedBy = wfhdr.CreatedBy;
                        _entHdr.CreatedDate = DateTime.Now;
                        context.tb_workflowSettingHdr.Add(_entHdr);
                        result_affected += context.SaveChanges();
                        _hdrId=_entHdr.Id;
                        foreach (var item in lsmodel)
                        {
                            wfdetil = imap.Map<WorkflowSettingModel, tb_workflowSetting>(item);
                            wfdetil.HeaderID = _hdrId;
                            context.tb_workflowSetting.Add(wfdetil);
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
