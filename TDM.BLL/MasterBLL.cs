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
            string sSql = "select Category, Value from tb_Master where Category='"+category+"'";
            return GetDataByDapper<MasterModel>(sSql).ToList();
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
