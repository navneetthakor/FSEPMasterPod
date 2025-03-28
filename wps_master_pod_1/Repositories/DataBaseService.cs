using ServiceStack.OrmLite;
using System.Data;

namespace wps_master_pod_1.Repositories
{
    public class DataBaseService : IDataBaseService
    {
        /// <summary>
        /// Connection string for the database
        /// </summary>
        private string connectionString { get; } = "Server=localhost;Database=master_db_wps;User=root;Password=N@vneet2810;";

        /// <summary>
        /// Database connection object
        /// </summary>
        public IDbConnection db { get; set; }

        /// <summary>
        /// Constructor for the DataBaseService class
        /// </summary>
        public DataBaseService()
        {
            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialect.Provider);
            db = dbFactory.OpenDbConnection();
        }
    }
}
