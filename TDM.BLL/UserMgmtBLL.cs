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
        public UserAppsModel Authenticate(string username, string password, out string mesg)
        {
            UserAppsModel model = new UserAppsModel();
            mesg = string.Empty;
            using (TDMDBEntities context = new TDMDBEntities())
            {

                var _q = context.tb_userApps.SingleOrDefault(x => x.UserName == username);
                if (_q != null)
                {
                    if (_q.Password == new Common().Decrypt(password, TDM_KEY))
                    {
                        var qry = context.tb_userApps.Join(context.tb_employee, u => u.EmployeeId, e => e.EmpID, (u, e) => new { u, e }).Select(s => new
                        {
                            s.u.UserName,
                            s.u.Id,
                            s.u.EmployeeId,
                            s.e.EmpName,
                            s.e.RoleApps
                        }).SingleOrDefault();

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
                var qry = context.tb_userApps.Join(context.tb_employee, u => u.EmployeeId, e => e.EmpID, (u, e) => new { u, e }).Select(s => new
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
    }
}
