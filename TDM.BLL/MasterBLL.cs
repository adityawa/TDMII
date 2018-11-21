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
    public class MasterBLL: BaseDAWithDapper, IBLL<MasterModel, tb_Master>
    {

        public IEnumerable<MasterModel> List(string category)
        {
            string sSql = "select Id, Category, Value from tb_Master where Category='"+category+"'";
            return GetDataByDapper<MasterModel>(sSql).ToList();
        }
        public int FindByValue(string _val, string _category)
        {
            int _retId = 0;
            using (TDMDBEntities context = new TDMDBEntities())
            {
                var _qry = context.tb_Master.Where(x => x.Value == _val && x.Category==_category).SingleOrDefault();
                if (_qry != null)
                    _retId = _qry.Id;
            }
            return _retId;
        }
        public MasterModel FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public int Insert(MasterModel entity, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public int Update(MasterModel entity, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public int Delete(int Id, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public MasterModel MapToModel(tb_Master fromEntity)
        {
            throw new NotImplementedException();
        }

        public tb_Master MapToEntity(MasterModel fromModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MasterModel> List()
        {
            throw new NotImplementedException();
        }
    }
}
