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
    public class SPKBll : BaseDAWithDapper
    {
        private MapperConfiguration config = null;
        private IMapper imap;
        private int result_affected = 0;
        public SPKBll()
        {
            if (config == null)
            {
                config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<SPKHeaderModel, tb_spkHdr>();
                    cfg.CreateMap<tb_spkHdr, SPKHeaderModel>();
                    cfg.CreateMap<SPKEquipmentModel, tb_KetTambahan>();
                    cfg.CreateMap<tb_KetTambahan, SPKEquipmentModel>();

                    cfg.CreateMap<SPKAdditionalModel, tb_PerlTambahan>();
                    cfg.CreateMap<tb_PerlTambahan, SPKAdditionalModel>();
                    cfg.CreateMap<SPKAttachmentModel, tb_Attachment>();
                    cfg.CreateMap<tb_Attachment, SPKAttachmentModel>();

                    cfg.CreateMap<WorklistModel, tb_Worklist>();
                    cfg.CreateMap<tb_Worklist, WorklistModel>();
                });
            }
        }
        public int Insert(SPKHeaderModel hdr, List<SPKAdditionalModel> lsAdditional, SPKEquipmentModel misc, List<SPKAttachmentModel> attchs, int docType, out string errMsg)
        {
            errMsg = string.Empty;
            imap = config.CreateMapper();
            int _spkId = 0;
           
            tb_spkHdr hdrEnt = new tb_spkHdr();
            tb_KetTambahan miscEnt = new tb_KetTambahan();
            tb_PerlTambahan additionalEnt = new tb_PerlTambahan();

            using (TDMDBEntities context = new TDMDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        imap = config.CreateMapper();
                        hdrEnt = imap.Map<SPKHeaderModel, tb_spkHdr>(hdr);
                        context.tb_spkHdr.Add(hdrEnt);
                        result_affected = context.SaveChanges();
                        _spkId = hdrEnt.Id;

                    
                        foreach (var item in lsAdditional)
                        {
                            additionalEnt.No = item.No;
                            additionalEnt.SPKId = _spkId;
                            additionalEnt.AdditionalItem = item.Additional;
                            context.tb_PerlTambahan.Add(additionalEnt);
                            result_affected += context.SaveChanges();
                        }

                        miscEnt = imap.Map<SPKEquipmentModel, tb_KetTambahan>(misc);
                        miscEnt.SPKId = _spkId;
                        context.tb_KetTambahan.Add(miscEnt);
                        result_affected += context.SaveChanges();

                        foreach (var item in attchs)
                        {
                            tb_Attachment attchEnt = new tb_Attachment();
                            attchEnt.Attachment = item.Attachment;
                            attchEnt.AttachmentName = item.AttachmentName;
                            attchEnt.DocType = item.DocType;
                            attchEnt.DocId = _spkId;
                            attchEnt.CreatedBy = "SYSTEM";
                            attchEnt.CreatedDate = DateTime.Now;
                            context.tb_Attachment.Add(attchEnt);
                            result_affected += context.SaveChanges();
                        }

                        //insert to worklist
                        tb_Worklist _entWorklist = new tb_Worklist();
                        _entWorklist.DocId = _spkId;
                        _entWorklist.DocType = docType;
                        _entWorklist.Status = MyEnums.workflowStatus.PENDING.ToString();
                        _entWorklist.StartDate = DateTime.Now;
                        _entWorklist.CurrLevel = 0;
                        _entWorklist.NextApprover = Convert.ToInt32(new WorkflowSettingBLL().GetNextActorId(docType, _entWorklist.CurrLevel+1));
                        _entWorklist.CreatedBy = hdr.CreatedBy;
                        _entWorklist.CreatedDate = DateTime.Now;
                        context.tb_Worklist.Add(_entWorklist);
                        result_affected += context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        errMsg = ex.Message;
                    }

                }
            }


            return _spkId;
        }
        public SPKHeaderModel Detail(int SPKId)
        {
            SPKHeaderModel _spk = new SPKHeaderModel();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var _qrySPK = context.tb_spkHdr.Find(SPKId);
                if (_qrySPK != null)
                {
                    imap = config.CreateMapper();
                    _spk = imap.Map<tb_spkHdr, SPKHeaderModel>(_qrySPK);

                  //  var _qryEquipment = context.tb_KetTambahan.Where(x => x.SPKId == SPKId).FirstOrDefault();
                    var _qryAdditional = context.tb_PerlTambahan.Where(x => x.SPKId == SPKId);
                    var _qryAttachment = context.tb_Attachment.Where(x => x.DocId == SPKId);
                    //if (_qryEquipment!=null)
                    //{
                    //    _spk.spkequipment = new SPKEquipmentModel();
                    //    _spk.spkequipment.Id = _qryEquipment.Id;
                    //    _spk.spkequipment.SPKId = _qryEquipment.SPKId;
                    //    _spk.spkequipment.IsKaroseri = _qryEquipment.IsKaroseri;
                    //    _spk.spkequipment.Karoseri = _qryEquipment.Karoseri;
                    //    _spk.spkequipment.IsOnTheRoad = _qryEquipment.IsOnTheRoad;
                    //    _spk.spkequipment.IsOffTheRoad = _qryEquipment.IsOffTheRoad;
                    //    _spk.spkequipment.IsChooseNo = _qryEquipment.IsChooseNo;
                    //    _spk.spkequipment.PlatNo = _qryEquipment.PlatNo;
                    //}
                    _spk.lsadditional = new List<SPKAdditionalModel>();
                    foreach (var item in _qryAdditional)
                    {
                        _spk.lsadditional.Add(new SPKAdditionalModel
                        {
                            Id=item.Id
                            ,SPKId=item.SPKId
                            ,No=(int)item.No
                            ,Additional=item.AdditionalItem
                        });
                    }
                    _spk.lsspkattachment = new List<SPKAttachmentModel>();
                    foreach (var att in _qryAttachment)
                    {
                        _spk.lsspkattachment.Add(new SPKAttachmentModel
                        {
                            Id=(Int32)att.Id,
                            DocId=att.DocId,
                            AttachmentName=att.AttachmentName,
                            DocType=(int)att.DocType
                        });
                    }
                }
            }
            return _spk;
        }
        public SPKAttachmentModel GetDocAttachment(int id)
        {
            SPKAttachmentModel att = new SPKAttachmentModel();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var _qry = context.tb_Attachment.SingleOrDefault(x => x.Id == id);
                if (_qry != null)
                {
                    att.Id =(Int32) _qry.Id;
                    att.AttachmentName = _qry.AttachmentName;
                    att.Attachment = _qry.Attachment;
                    att.DocId = _qry.DocId;
                    att.DocType = _qry.DocType==null?0:(int)_qry.DocType;
                }
            }
            
            return att;
        }
        public int DeleteAttachment(int id)
        {
            result_affected = 0;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var _remove = context.tb_Attachment.SingleOrDefault(x => x.Id == id);
                try
                {
                    context.tb_Attachment.Remove(_remove);
                    result_affected = context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return result_affected;
        }
        public int Update(SPKHeaderModel hdr, List<SPKAdditionalModel> lsAdditional, SPKEquipmentModel misc, List<SPKAttachmentModel> attchs, int docType, string action, out string errMsg)
        {
            errMsg = string.Empty;
            imap = config.CreateMapper();
            
            tb_spkHdr hdrEnt = new tb_spkHdr();
            tb_KetTambahan miscEnt = new tb_KetTambahan();
            tb_PerlTambahan additionalEnt = new tb_PerlTambahan();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var update = context.tb_spkHdr.SingleOrDefault(x => x.Id == hdr.Id);
                        if (update != null)
                        {
                            update.SPKDate = hdr.SPKDate;
                            update.JanjiPenyerahan = hdr.JanjiPenyerahan;
                            update.LOTNo = hdr.LOTNo;
                            update.Buyer = hdr.Buyer;
                            update.BuyerAddress = hdr.BuyerAddress;
                            update.KTP = hdr.KTP;
                            update.Phone = hdr.Phone;
                            update.STNKName = hdr.STNKName;
                            update.STNKAddress = hdr.STNKAddress;
                            update.Email = hdr.Email;
                            update.Branch = hdr.Branch;
                            update.Merk = hdr.Merk;
                            update.Warna = hdr.Warna;
                            update.Karoseri = hdr.Karoseri;
                            update.MachineNo = hdr.MachineNo;
                            update.RangkaNo = hdr.RangkaNo;
                            update.Pembiayaan = hdr.Pembiayaan;
                            update.Year = hdr.Year;
                            update.Via = hdr.Via;
                            update.PercentageBunga = hdr.PercentageBunga;
                            update.OTRPrice = hdr.OTRPrice;
                            update.KaroseriPrice = hdr.KaroseriPrice;
                            update.TotalPrice = hdr.TotalPrice;
                            update.DP = hdr.DP;
                            update.TandaJadi = hdr.TandaJadi;
                            update.Pembayaran = hdr.Pembayaran;
                            update.TransferVia = hdr.TransferVia;
                            update.AlamatKirim = hdr.AlamatKirim;
                            update.City = hdr.City;
                            update.IsKaroseri = hdr.IsKaroseri;
                            update.KaroseriDesc = hdr.KaroseriDesc;
                            update.IsOnTR = hdr.IsOnTR;
                            update.IsOffTR = hdr.IsOffTR;
                            update.IsChooseNo = hdr.IsChooseNo;
                            update.ChooseNo = hdr.ChooseNo;
                            update.ModifiedBy = hdr.ModifiedBy;
                            update.ModifiedDate = DateTime.Now;
                            context.SaveChanges();



                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                
               
            }
        }
    }
}

