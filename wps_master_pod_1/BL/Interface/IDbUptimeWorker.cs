using wps_master_pod_1.Modal;

namespace wps_master_pod_1.BL.Interface
{
    public interface IDbUptimeWorker
    {
        Response AddServer(int clinet_id, int server_id, string worker_id);
        Response RemoveServer(int client_id, int server_id);
    }
}