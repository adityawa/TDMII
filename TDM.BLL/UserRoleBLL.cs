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
    public class UserRoleBLL :BaseDAWithDapper
    {
        private int result_Affected = 0;
        private MapperConfiguration config = null;
        private IMapper imap;
        public UserRoleBLL()
        {
            if (config == null)
            {
                config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserRoleModel, tb_userRole>();
                    cfg.CreateMap<tb_userRole, RoleModel>();
                });


            }
        }
        public IEnumerable<UserRoleModel> ListUserRole(int RoleID)
        {
            IEnumerable<UserRoleModel> ls = new List<UserRoleModel>();
            string sSql = "SELECT ur.*, e.EmpName, r.RoleName FROM tb_userRole ur INNER JOIN tb_employee e"
                + " ON ur.EmployeeID=e.EmpID"
                + " INNER JOIN tb_role r ON ur.RoleId=r.Id WHERE ur.RoleId="+RoleID;
            ls = GetDataByDapper<UserRoleModel>(sSql);
            return ls;
        }

        public UserRoleModel FindByID(int Id)
        {
            UserRoleModel urm = new UserRoleModel();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var entity = context.tb_userRole.SingleOrDefault(x => x.Id == Id);
                    if (entity != null)
                    {
                        IMapper imap = config.CreateMapper();
                        urm = imap.Map<tb_userRole, UserRoleModel>(entity);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return urm;
        }

        public int Insert(UserRoleModel urModel, out string errMsg)
        {
            errMsg = string.Empty;
            tb_userRole urEntity;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                if (ValidateModel(urModel, out errMsg))
                {
                    try
                    {
                        IMapper imap = config.CreateMapper();
                        urEntity = imap.Map<UserRoleModel, tb_userRole>(urModel);
                        context.tb_userRole.Add(urEntity);
                        result_Affected = context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex.Message;
                    }
                }
            }
            return result_Affected;
        }

        public int Update(UserRoleModel urm, out string mesg)
        {
            mesg = string.Empty;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var edit = context.tb_userRole.SingleOrDefault(x => x.Id == urm.Id);
                    if (edit != null)
                    {
                        edit.EmployeeId = urm.EmployeeId;
                        edit.ModifiedBy = urm.ModifiedBy;
                        edit.ModifiedDate = urm.ModifiedDate;
                        result_Affected = context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    mesg = ex.Message;
                }
                
            }
            return result_Affected;
        }

        public int Delete(int Id, out string mesg)
        {
            mesg = string.Empty;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var remove = context.tb_userRole.SingleOrDefault(x => x.Id == Id);
                    if (remove != null)
                    {
                        context.tb_userRole.Remove(remove);
                        result_Affected = context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    mesg = ex.Message;
                }
               
            }
            return result_Affected;
        }

        public bool ValidateModel(UserRoleModel urm, out string mesg)
        {
            int err_ctr = 0;
            StringBuilder sb = new StringBuilder();
            mesg = string.Empty;
            bool IsOk = true;
            if (urm.RoleID == null || urm.RoleID==0)
            {
                err_ctr += 1;
                sb.Append("Role must be specified");
            }
            if (urm.EmployeeId == null || urm.EmployeeId == 0)
            {
                err_ctr += 1;
                sb.Append("Employee must be specified");
            }

            if (err_ctr > 0)
            {
                IsOk = false;
                mesg = sb.ToString();
            }
            return IsOk;
        }
    }
}
