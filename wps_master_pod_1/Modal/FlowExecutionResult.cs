namespace wps_master_pod_1.Modal
{
    public class FlowExecutionResult
    {
        public bool Success { get; set; }
        public DateTime Started { get; set; }
        public DateTime Completed { get; set; }
        public double ExecutionTimeMs { get; set; }
        public List<string> ExecutedNodes { get; set; }
        public Dictionary<string, object> NodeResults { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}
