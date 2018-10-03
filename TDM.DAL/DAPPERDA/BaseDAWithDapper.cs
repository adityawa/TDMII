using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using TDM.DAL;
namespace TDM.DAL.DAPPERDA
{
    public abstract class BaseDAWithDapper
    {
        protected SqlConnection sqlcon;
        public BaseDAWithDapper()
        {
            sqlcon = SqlConnectionMgr.GetSqlConn();
        }
        public virtual IEnumerable<T> GetDataByDapper<T>(string sSql) where T : class, new()
        {
            List<T> ls = new List<T>();
            using (sqlcon)
            {
                try
                {
                    ls = sqlcon.Query<T>(sSql).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return ls;
        }

        public virtual T FindById<T>(string sSql, int Id) where T : class, new()
        {
            throw new NotImplementedException();
        }




        public int Delete<T>(string sSql, T entity) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public T FindById<T>(string sSql, T entity) where T : class, new()
        {
            throw new NotImplementedException();
        }


        public int InsertOrUpdate<T>(string sSql, T entity) where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
