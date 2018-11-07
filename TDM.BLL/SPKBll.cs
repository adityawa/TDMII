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

        public int Insert(SPKHeaderModel hdr, List<SPKAdditionalModel> lsAdditional, SPKEquipmentModel misc, SPKAttachmentModel attch, out string errMsg)
        {
            errMsg = string.Empty;
            imap = config.CreateMapper();
            int _spkId = 0;
            tb_spkHdr hdrEnt = new tb_spkHdr();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    hdrEnt.SPKDate = hdr.SPKDate;
                }
            }
            return result_affected;
        }


    }
}

