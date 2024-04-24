using System.Data;
using System.Data.SqlClient;

namespace PSL.Laundry.CentralService.DataAccessLayer
{
    public abstract class DAL
    {
        string connectionString;
        public DAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}