
namespace wps_master_pod_1.Repositories
{
    public class StackStore : IStackStore
    {
        private Stack<string> _stack;
        public StackStore()
        {
            _stack = new Stack<string>();
        }

        public void AddWorker(string worker_id)
        {
            _stack.Push(worker_id);
        }

        public string? GetWorker()
        {
            if (_stack.Count == 0)
                return null;
            return _stack.Pop();
        }
    }
}
