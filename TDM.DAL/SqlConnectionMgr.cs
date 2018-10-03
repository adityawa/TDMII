using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
namespace TDM.DAL
{
    public class SqlConnectionMgr
    {
        public static SqlConnection GetSqlConn()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TDMConnStr"].ConnectionString.ToString());
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            return conn;
        }
    }
}
