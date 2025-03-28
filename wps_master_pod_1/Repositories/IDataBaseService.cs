using System.Data;

namespace wps_master_pod_1.Repositories
{
    public interface IDataBaseService
    {
        IDbConnection db { get; set; }
    }
}