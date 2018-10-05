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
    public abstract class BaseBLL
    {
        protected TDMDBEntities context;
        public virtual TModel MapToModel<TModel, T>(T fromEntity) where T:class where TModel : class, new()
        {
            return new TModel();
            
        }
        public virtual TEntity MapToEntity<TEntity, T>(T fromModel) where T : class where TEntity:class,new()
        {
            return new TEntity();
        }
    }
}
