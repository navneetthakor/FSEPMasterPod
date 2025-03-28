namespace wps_master_pod_1.Repositories
{
    public interface IStackStore
    {
        void AddWorker(string worker_id);
        string? GetWorker();
    }
}