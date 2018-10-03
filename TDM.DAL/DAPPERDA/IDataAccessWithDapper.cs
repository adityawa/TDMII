using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.DAL.DAPPERDA
{
    interface IDataAccessWithDapper<T> where T: class
    {
         IEnumerable<T> GetDataByDapper(string sSql);
         T FindById(string sSql, Int32 Id);
         T FindById(string sSql, T entity);
         int InsertOrUpdate<T>(string sSql, T entity);
         int Delete(string sSql, T entity);
    }
}
