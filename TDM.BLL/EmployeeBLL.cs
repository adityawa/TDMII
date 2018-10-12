using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDM.DAL;
using TDM.DAL.DAPPERDA;
using TDM.BLL.Model;
namespace TDM.BLL
{
    public class EmployeeBLL : IBLL<EmployeeModel, tb_employee>
    {
        private int result_affected = 0;
        public EmployeeBLL() { }
        public IEnumerable<EmployeeModel> List()
        {
            List<EmployeeModel> ls = new List<EmployeeModel>();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var qry = context.tb_employee
                    .Join(context.tb_Master, e => e.RoleApps, m => m.Id, (e, m) => new { e, m })
                    .Where(x => x.m.Category == MyEnums.enumMaster.RoleApps.ToString())
                    .Select(x => new
                    {
                        x.e.EmpID,
                        x.e.EmpName,
                        x.e.EmpNo,
                        x.e.ReportTo,
                        x.e.RoleApps,
                        x.m.Value,
                        x.e.Dept
                    });

                foreach (var item in qry)
                {
                    ls.Add(new EmployeeModel
                    {
                        Id = item.EmpID,
                        EmpNo = item.EmpNo,
                        EmpName = item.EmpName,
                        ReportTo = item.ReportTo == null ? 0 : (int)item.ReportTo,
                        RoleApps = (int)item.RoleApps,
                        RoleAppsName = item.Value,
                        Dept=item.Dept
                    });
                }
            }
            return ls;
        }

        public EmployeeModel FindById(int Id)
        {
            EmployeeModel emp = new EmployeeModel();
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var ent = context.tb_employee.SingleOrDefault(x => x.EmpID == Id);
                if (ent != null)
                {
                    emp =MapToModel(ent);
                }
            }
            return emp;
        }

        public int Insert(EmployeeModel model, out string errMsg)
        {
            errMsg = string.Empty;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    tb_employee tbEmp = MapToEntity(model);
                    context.tb_employee.Add(tbEmp);
                    result_affected = context.SaveChanges();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message == null ? ex.InnerException.Message : ex.Message;
                }
                
            }
            return result_affected;
        }

        public int Update(EmployeeModel model, out string errMsg)
        {
            errMsg = string.Empty;

            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var exist = context.tb_employee.FirstOrDefault(x => x.EmpID == model.Id);
                    exist.EmpName = model.EmpName;
                    exist.EmpNo = model.EmpNo;
                    exist.Dept = model.Dept;
                    exist.RoleApps = model.RoleApps;
                    exist.ModifiedBy = model.ModifiedBy;
                    exist.ModifiedDate = model.ModifiedDate;

                    result_affected = context.SaveChanges();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message == null ? ex.InnerException.Message : ex.Message;
                }
                
            }
            return result_affected;
        }

        public int Delete(int Id, out string errMsg)
        {
            errMsg = string.Empty;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                try
                {
                    var exist = context.tb_employee.FirstOrDefault(x => x.EmpID == Id);
                    if (exist != null)
                    {
                       context.tb_employee.Remove(exist);
                       result_affected = context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message == null ? ex.InnerException.Message : ex.Message;
                }
                
            }
            return result_affected;
        }

      


        public EmployeeModel MapToModel(tb_employee fromEntity)
        {
            if (fromEntity == null)
                throw new Exception("Source Data cannot be null");
            EmployeeModel model = new EmployeeModel();
            model.Id = fromEntity.EmpID;
            model.EmpNo = fromEntity.EmpNo;
            model.EmpName = fromEntity.EmpName;
            model.ReportTo = fromEntity.ReportTo == null ? 0 : (int)fromEntity.ReportTo;
            model.RoleApps = fromEntity.RoleApps == null ? 0 : (int)fromEntity.RoleApps;
            model.Dept = fromEntity.Dept;
            model.CreatedBy = fromEntity.CreatedBy;
            model.CreatedDate = fromEntity.CreatedDate;
            model.ModifiedBy = fromEntity.ModifiedBy;
            model.ModifiedDate = fromEntity.ModifiedDate;
            return model;
        }

        public tb_employee MapToEntity(EmployeeModel fromModel)
        {
            if (fromModel == null)
                throw new Exception("Source Data cannot be null");
            tb_employee entity = new tb_employee();
            entity.EmpID = fromModel.Id;
            entity.EmpNo = fromModel.EmpNo;
            entity.EmpName = fromModel.EmpName;
            entity.ReportTo = fromModel.ReportTo;
            entity.RoleApps = fromModel.RoleApps;
            entity.CreatedBy = fromModel.CreatedBy;
            entity.CreatedDate = fromModel.CreatedDate;
            entity.ModifiedBy = fromModel.ModifiedBy;
            entity.ModifiedDate = fromModel.ModifiedDate;
            entity.Dept = fromModel.Dept;
            return entity;
        }
    }
}
