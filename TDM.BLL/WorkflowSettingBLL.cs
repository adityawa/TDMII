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

    public class WorkflowSettingBLL : BaseDAWithDapper
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
                    cfg.CreateMap<WorkflowSettingHeader, tb_workflowSettingHdr>();
                    cfg.CreateMap<tb_workflowSettingHdr, WorkflowSettingHeader>();
                });
            }
        }

        public IEnumerable<WorkflowSettingHeader> GetHeader()
        {
            List<WorkflowSettingHeader> ls = new List<WorkflowSettingHeader>();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var qry = context.tb_workflowSettingHdr
                    .Join(context.tb_Master, w => w.TypeID, m => m.Id, (w, m) => new { w, m })
                    .Where(x => x.w.IsActive == true && x.m.Category == MyEnums.enumMaster.DocumentType.ToString())
                    .Select(n => new
                    {
                        n.w.Id,
                        n.w.TypeID,
                        n.w.Version,
                        n.w.IsActive,
                        n.m.Value,
                        n.w.ApprovalLevel
                    });

                foreach (var item in qry)
                {
                    ls.Add(new WorkflowSettingHeader
                    {
                        Id = item.Id,
                        TypeID = item.TypeID,
                        TypeName = item.Value,
                        Version = item.Version,
                        IsActive = (bool)item.IsActive,
                        ApprovalLevel = item.ApprovalLevel
                    });
                }
            }
            return ls;
        }

        public int Insert(WorkflowSettingHeader wfhdr, List<WorkflowSettingModel> lsmodel, out string errMsg)
        {
            errMsg = string.Empty;
            tb_workflowSetting wfdetil = new tb_workflowSetting();
            imap = config.CreateMapper();
            int _hdrId = 0;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var _cekstillEmpty = context.tb_workflowSettingHdr.Count();

                        tb_workflowSettingHdr _entHdr = new tb_workflowSettingHdr();
                        _entHdr.ApprovalLevel = wfhdr.ApprovalLevel;
                        _entHdr.Version = wfhdr.Version;
                        _entHdr.TypeID = wfhdr.TypeID;
                        _entHdr.CreatedBy = wfhdr.CreatedBy;
                        _entHdr.CreatedDate = DateTime.Now;
                        _entHdr.IsActive = true;
                        context.tb_workflowSettingHdr.Add(_entHdr);
                        result_affected += context.SaveChanges();
                        _hdrId = _entHdr.Id;
                        foreach (var item in lsmodel)
                        {
                            wfdetil = imap.Map<WorkflowSettingModel, tb_workflowSetting>(item);
                            wfdetil.HeaderID = _hdrId;
                            wfdetil.CreatedBy = "SYSTEM";
                            wfdetil.CreatedDate = DateTime.Now;
                            context.tb_workflowSetting.Add(wfdetil);
                            result_affected += context.SaveChanges();
                        }

                        if (_cekstillEmpty > 0)
                        {
                            if (NonActivateSetting(_entHdr, _hdrId) > 0)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                            }
                        }
                        else
                        {
                            transaction.Commit();
                        }
                            
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

        public int Delete(int _id, out string errMsg)
        {
            errMsg = string.Empty;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var _remove = context.tb_workflowSetting.Where(x => x.HeaderID == _id);
                        if (_remove != null)
                        {
                            context.tb_workflowSetting.RemoveRange(_remove);
                            result_affected = context.SaveChanges();

                            if (result_affected > 0)
                            {
                                var _header = context.tb_workflowSettingHdr.SingleOrDefault(x => x.Id == _id);
                                if (_header != null)
                                {
                                    context.tb_workflowSettingHdr.Remove(_header);
                                    result_affected += context.SaveChanges();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        errMsg = ex.Message;
                    }
                }
            }
            return result_affected;
        }

        private int NonActivateSetting(tb_workflowSettingHdr entiti, int exceptId)
        {
            int result = 0;
            entiti.Id = exceptId;
            string sSql = "UPDATE tb_workflowSettingHdr SET IsActive=false where TypeID=@TypeID AND ID <> @Id";
            result = InsertOrUpdate<tb_workflowSettingHdr>(sSql, entiti);
            return result;
        }

        public WorkflowSettingHeader Detail(int id)
        {
            WorkflowSettingHeader detil = new WorkflowSettingHeader();
            IMapper imap = config.CreateMapper();
            try
            {
                using (TDMDBEntities context = new TDMDBEntities())
                {
                    var _details = context.tb_workflowSettingHdr.SingleOrDefault(x => x.Id == id);
                    detil = imap.Map<tb_workflowSettingHdr, WorkflowSettingHeader>(_details);

                    var qry_setting_detail = context.tb_workflowSetting.Where(x => x.HeaderID == id);
                    foreach (var item in qry_setting_detail)
                    {
                        WorkflowSettingModel md=new WorkflowSettingModel();
                        md=imap.Map<tb_workflowSetting, WorkflowSettingModel>(item);
                        detil.ls_details.Add(md);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return detil;
        }

        public int Update(WorkflowSettingHeader wfHdr, List<WorkflowSettingModel> lsdetails, out string errMsg)
        {
            errMsg = string.Empty;
            tb_workflowSetting wfdetil = new tb_workflowSetting();
            imap = config.CreateMapper();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var _exist = context.tb_workflowSettingHdr.SingleOrDefault(x => x.Id == wfHdr.Id);
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        _exist.TypeID = wfHdr.TypeID;
                        _exist.Version = wfHdr.Version;
                        _exist.ApprovalLevel = wfHdr.ApprovalLevel;
                        _exist.ModifiedBy = "SYSTEM";
                        _exist.ModifiedDate = DateTime.Now;
                        result_affected += context.SaveChanges();

                        var toBeDelete = context.tb_workflowSetting.Where(x => x.HeaderID == wfHdr.Id);
                        if (toBeDelete != null)
                        {
                           context.tb_workflowSetting.RemoveRange(toBeDelete);
                           context.SaveChanges();
                        }

                        foreach (var item in lsdetails)
                        {
                            wfdetil = imap.Map<WorkflowSettingModel, tb_workflowSetting>(item);
                            wfdetil.HeaderID = wfHdr.Id;
                            wfdetil.CreatedBy = "SYSTEM";
                            wfdetil.CreatedDate = DateTime.Now;
                            context.tb_workflowSetting.Add(wfdetil);
                            result_affected += context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return result_affected;
        }

    }
}
