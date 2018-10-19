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
    
    public class RoleBLL :IBLL<RoleModel, tb_role>
    {
        private MapperConfiguration config = null;
        private IMapper imap;
        private int result_affected = 0;
        public RoleBLL()
        {
            if (config == null)
            {
                 config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<RoleModel, tb_role>();
                    cfg.CreateMap<tb_role, RoleModel>();
                });
                
                 
            }
        }
        public IEnumerable<RoleModel> List()
        {
            List<RoleModel> ls = new List<RoleModel>();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var entity = context.tb_role;
                foreach (var item in entity)
                {
                    IMapper imap = config.CreateMapper();
                    var mapped = imap.Map<tb_role, RoleModel>(item);
                    ls.Add(mapped);
                }
            }
            return ls;
        }

        public RoleModel FindById(int Id)
        {
            RoleModel model = new RoleModel();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                IMapper imap = config.CreateMapper();
                tb_role role = context.tb_role.FirstOrDefault(x => x.Id == Id);
                model = imap.Map<tb_role, RoleModel>(role);
            }

            return model;
        }

        public int Insert(RoleModel model, out string errMsg)
        {
            errMsg = string.Empty;
            tb_role role_entity = new tb_role();   
            try
            {
                IMapper imap = config.CreateMapper();
                role_entity = imap.Map<RoleModel, tb_role>(model);
                using (TDMDBEntities context = new TDMDBEntities())
                {
                    context.tb_role.Add(role_entity);
                    result_affected = context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return result_affected;
        }

        public int Update(RoleModel model, out string errMsg)
        {
            errMsg = string.Empty;
            result_affected = 0;
            try
            {
                using (TDMDBEntities context = new TDMDBEntities())
                {
                    var detil = context.tb_role.SingleOrDefault(x => x.Id == model.Id);
                    if (detil != null)
                    {
                        detil.RoleName = model.RoleName;
                        detil.ModifiedBy = model.ModifiedBy;
                        detil.ModofiedDate = DateTime.Now;
                        result_affected = context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return result_affected;
        }

        public int Delete(int Id, out string errMsg)
        {
            errMsg = string.Empty;
            result_affected = 0;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                //cek is exist in transaction
                var cek = context.tb_userRole.SingleOrDefault(x => x.RoleId == Id);
                if (cek != null)
                {
                    errMsg = "Data cannot be deleted because already used in transaction";
                }
                else
                {
                    var delete = context.tb_role.SingleOrDefault(x => x.Id == Id);
                    if (delete != null)
                    {
                        context.tb_role.Remove(delete);
                        result_affected = context.SaveChanges();
                    }
                }
            }
            return result_affected;
        }

        public RoleModel MapToModel(tb_role fromEntity)
        {
            throw new NotImplementedException();
        }

        public tb_role MapToEntity(RoleModel fromModel)
        {
            throw new NotImplementedException();
        }
    }
}
