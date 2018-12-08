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
    public class UserMgmtBLL:BaseDAWithDapper
    {
        protected const string TDM_KEY = "TDM2018";
        private int result_affected = 0;
        public UserAppsModel Authenticate(string username, string password, out string mesg)
        {
            UserAppsModel model = new UserAppsModel();
            mesg = string.Empty;
            using (TDMDBEntities context = new TDMDBEntities())
            {

                var _q = context.tb_userApps.SingleOrDefault(x => x.UserName == username);
                if (_q != null)
                {
                    if (password == new Common().Decrypt(_q.Password, TDM_KEY))
                    {
                        var qry = context.tb_userApps.Join(context.tb_employee, u => u.EmployeeId, e => e.EmpID, (u, e) => new { u, e }).Select(s => new
                        {
                            s.u.UserName,
                            s.u.Id,
                            s.u.EmployeeId,
                            s.e.EmpName,
                            s.e.RoleApps,       
                        })
                        .SingleOrDefault(x=>x.UserName==username);

                        model.Id = qry.Id;
                        model.UserName = qry.UserName;
                        model.EmployeeId = qry.EmployeeId;
                        model.EmployeeName = qry.EmpName;
                        model.Role = (int)qry.RoleApps;
                    }
                    else
                    {
                        mesg = "Password Salah";
                    }
                }
                else
                {
                    mesg = "UserName tidak ditemukan";
                }
            }
            return model;
        }
        public IEnumerable<UserAppsModel> ListActiveUser()
        {
            List<UserAppsModel> ls = new List<UserAppsModel>();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var qry = context.tb_userApps
                    .Join(context.tb_employee, u => u.EmployeeId, e => e.EmpID, (u, e) => new { u, e })
                    .Where(x=>x.u.IsExpired==false)
                    .Select(s => new
                {
                    s.u.Id,
                    s.u.UserName,
                    s.u.Password,
                    s.u.IsExpired,
                    s.u.EmployeeId,
                    s.e.EmpName
                });
                foreach (var item in qry)
                {
                    ls.Add(new UserAppsModel
                    {
                        Id=item.Id,
                        UserName=item.UserName,
                        Password=item.Password,
                        IsExpired=item.IsExpired,
                        EmployeeId=item.EmployeeId,
                        EmployeeName=item.EmpName
                    });
                    
                }
                
            }

            return ls;
        }

        private bool ValidateBeforeUpdate(UserAppsModel usr, out string err)
        {
            bool isValid = true;
            err = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (usr.UserName == string.Empty)
            {
                sb.Append("User Name tidak boleh kosong /n");
                isValid = false;
            }
            if (usr.EmployeeId == 0)
            {
                sb.Append("Employee ID tidak boleh kosong /n");
                isValid = false;
            }
            return isValid;
        }
        public int ValidateBeforSave(UserAppsModel usr, out string mesg)
        {
            int result_affected = 0;
            mesg = string.Empty;
            StringBuilder sb=new StringBuilder();
            if (usr.UserName == string.Empty)
            {
                sb.Append("User Name tidak boleh kosong /n");
            }
            if (usr.EmployeeId == 0)
            {
                sb.Append("Employee ID tidak boleh kosong /n");
            }
            if (usr.Password == string.Empty)
            {
                sb.Append("Password tidak boleh kosong /n");
            }
            //cek is any same username
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var _u = context.tb_userApps.SingleOrDefault(x => x.UserName == usr.UserName);
                if (_u != null)
                    sb.Append("User Name sudah terpakai, silahkan mengganti dengan yang lain");
            }

            mesg = sb.ToString();
            if (mesg == string.Empty)
            {
                usr.Password = new Common().Encrypt(usr.Password, TDM_KEY);
                string sSql = "Insert INTO tb_UserApps (UserName, Password, IsExpired, EmployeeId, CreatedBy)"
                    + "VALUES (@UserName, @Password, @IsExpired, @EmployeeId, @CreatedBy)";
                result_affected = InsertOrUpdate<UserAppsModel>(sSql, usr);
            }
          
            return result_affected;
        }
        public int Update(UserAppsModel model, out string errMsg)
        {
            StringBuilder sb = new StringBuilder();
            int errctr = 0;
            errMsg = MyEnums.enumStatus.SUCCESS.ToString();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    //cek is any same username
                   
                        var _u = context.tb_userApps.SingleOrDefault(x => x.UserName == model.UserName);
                        if (_u != null)
                        {
                            if (model.Id != _u.Id)
                            {
                                sb.Append("User Name sudah terpakai, silahkan mengganti dengan yang lain");
                                errctr += 1;
                            }
                            
                        }

                        if (errctr <= 0)
                        {
                            var detil = context.tb_userApps.FirstOrDefault(x => x.Id == model.Id);
                            if (detil != null && ValidateBeforeUpdate(model, out errMsg))
                            {
                                detil.UserName = model.UserName;
                                detil.EmployeeId = model.EmployeeId;
                                detil.IsExpired = model.IsExpired;
                                detil.ModifiedBy = model.ModifiedBy;
                                detil.ModifiedDate = model.ModifiedDate;

                                result_affected = context.SaveChanges();
                            }
                        }
                        else
                        {
                            errMsg = sb.ToString();
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
        public void map(tb_userApps ent, out UserAppsModel obj)
        {
            obj = new UserAppsModel();
            if(ent==null)
               throw new Exception("source cannot be null");
            obj.Id = ent.Id;
            obj.UserName = ent.UserName;
            obj.Password = ent.Password;
            obj.IsExpired = ent.IsExpired;
            obj.EmployeeId = ent.EmployeeId;
        }
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
        public int GetEmployeeIDByUserName(string userName)
        {
            int _empId = 0;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var qry = context.tb_userApps.Single(x => x.UserName == userName);
                _empId = qry.EmployeeId;
            }
            return _empId;
        }

        public int UpdateUserPassword(string username, string oldPassword, string newpassword, out string mesg)
        {
            mesg = string.Empty;
            result_affected = 0;
            string encrypted_password = new Common().Encrypt(oldPassword, TDM_KEY);
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var qry = context.tb_userApps.SingleOrDefault(x => x.UserName == username && x.Password == encrypted_password);
                    if (qry != null)
                    {
                        qry.Password =new Common().Encrypt( newpassword, TDM_KEY);
                        result_affected = context.SaveChanges();
                    }
                    else
                    {
                        mesg = "username and oldpassword not found";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
               
            }
            return result_affected;
        }
    }
}
