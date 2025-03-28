using System.Data;

namespace wps_master_pod_1.Modal
{
    public class Response
    {
        /// <summary>
        /// Data received by this request
        /// </summary>
        public DataTable? Data { get; set; }

        /// <summary>
        /// Does it have Error
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Message provided
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// for Other use like (POCO, DTO)
        /// </summary>
        public dynamic? Other { get; set; }
    }
}
