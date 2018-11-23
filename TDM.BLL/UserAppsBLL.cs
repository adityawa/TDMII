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
    public class UserAppsBLL : IBLL<UserAppsModel, tb_userApps>
    {
        private int result_affected = 0;
        private const string TDM_KEY = "TDM2018";
        public UserAppsBLL() { }
        public IEnumerable<UserAppsModel> List()
        {
            List<UserAppsModel> ls = new List<UserAppsModel>();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var qry = context.tb_userApps
                    .Join(context.tb_employee, e => e.EmployeeId, m => m.EmpID, (e, m) => new { e, m })
                    .Where(x => x.m.IsActive == true)
                    .Select(x => new
                    {
                        x.e.Id,
                        x.m.EmpName,
                        x.e.EmployeeId,
                        x.e.UserName,
                        x.e.Password,
                        x.e.IsExpired,
                        x.e.CreatedBy,
                        x.e.CreatedDate
                    });

                foreach (var item in qry)
                {
                    ls.Add(new UserAppsModel
                    {
                        Id = item.Id,
                        EmployeeId = item.EmployeeId,
                        EmployeeName = item.EmpName,
                        UserName = item.UserName,
                        Password = item.Password,
                        IsExpired = item.IsExpired,
                        
                    });
                }
            }
            return ls;
        }

        public UserAppsModel FindById(int Id)
        {
            UserAppsModel usr = new UserAppsModel();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var q = context.tb_userApps.SingleOrDefault(x => x.Id == Id);
                    if (q != null)
                    {
                        usr = MapToModel(q);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return usr;
        }

        public int Insert(UserAppsModel model, out string errMsg)
        {
            errMsg = MyEnums.enumStatus.SUCCESS.ToString();
            tb_userApps tentity = MapToEntity(model);
            try
            {
                using (TDMDBEntities context = new TDMDBEntities())
                {
                    context.tb_userApps.Add(tentity);
                    result_affected = context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return result_affected;
        }

        public int Update(UserAppsModel model, out string errMsg)
        {
            errMsg = MyEnums.enumStatus.SUCCESS.ToString();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var detil = context.tb_userApps.FirstOrDefault(x => x.Id == model.Id);
                    if (detil != null)
                    {
                        detil.UserName = model.UserName;
                        detil.EmployeeId = model.EmployeeId;
                        detil.IsExpired = model.IsExpired;
                        detil.ModifiedBy = model.ModifiedBy;
                        detil.ModifiedDate = model.ModifiedDate;

                        result_affected = context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }
               
            }
            return result_affected;
        }

        public int Delete(int Id, out string errMsg)
        {
            errMsg = MyEnums.enumStatus.SUCCESS.ToString();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var delete = context.tb_userApps.FirstOrDefault(x => x.Id == Id);
                    if (delete != null)
                    {
                        context.tb_userApps.Remove(delete);
                        result_affected = context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }
               
            }

            return result_affected;
        }

        public 
       

        public UserAppsModel MapToModel(tb_userApps fromEntity)
        {
            if (fromEntity == null)
                throw new Exception("Source Data cannot be null");
            UserAppsModel model = new UserAppsModel();
            model.Id = fromEntity.Id;
            model.UserName = fromEntity.UserName;
            model.Password = fromEntity.Password;
            model.IsExpired = fromEntity.IsExpired;
            model.EmployeeId = fromEntity.EmployeeId;
            model.CreatedBy = fromEntity.CreatedBy;
            model.CreatedDate = fromEntity.CreatedDate;
            model.ModifiedBy = fromEntity.ModifiedBy;
            model.ModifiedDate = fromEntity.ModifiedDate;

            return model;
        }

        public tb_userApps MapToEntity(UserAppsModel fromModel)
        {
            if (fromModel == null)
                throw new Exception("Source Data cannot be null");
            tb_userApps entity = new tb_userApps();
            entity.Id = fromModel.Id;
            entity.UserName = fromModel.UserName;
            entity.Password = new Common().Encrypt(fromModel.Password, TDM_KEY);
            entity.IsExpired = fromModel.IsExpired;
            entity.EmployeeId = fromModel.EmployeeId;
            entity.CreatedBy = fromModel.CreatedBy;
            entity.CreatedDate = fromModel.CreatedDate;
            entity.ModifiedBy = fromModel.ModifiedBy;
            entity.ModifiedDate = fromModel.ModifiedDate;

            return entity;
        }
    }
}
