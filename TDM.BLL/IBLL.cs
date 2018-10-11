using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL
{
    public interface IBLL<T, TEntity> where T:class where TEntity:class
    {
        IEnumerable<T> List();
        
        T FindById(int Id);
        int Insert(T entity, out string errMsg);
        int Update(T entity, out string errMsg);
        int Delete(int Id, out string errMsg);
        T MapToModel(TEntity fromEntity);
        TEntity MapToEntity(T fromModel);
    }
}
