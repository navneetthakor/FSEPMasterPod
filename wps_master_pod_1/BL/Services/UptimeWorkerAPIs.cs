using wps_master_pod_1.Modal;
using RestSharp;

namespace wps_master_pod_1.BL.Services
{
    public class UptimeWorkerAPIs
    {
        /// <summary>
        /// Register server to worker pod
        /// </summary>
        /// <param name="serverModal">server modal</param>
        /// <returns>Response object (check 'IsError' property to see that wheather request successed or not) </returns>
        public static Response RegisterServer(ServerModal serverModal)
        {
            //create restClient
            RestClient client = new RestClient("http://localhost:5004/");

            //preparing request to register server 
            RestRequest request = new RestRequest("api/RegisterServer/register", Method.Post);
            request.AddJsonBody(serverModal);

            //sending request to worker pod
            RestResponse rr = client.Execute(request);

            // marked for removal
            if (!rr.IsSuccessStatusCode)
            {
                return new Response() { Data = null, IsError = true, ErrorMessage = "Worker pod is not available", StatusCode = 500 };
            }
            return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200 };
        }

        public static Response RemoveServer(int client_id, int server_id)
        {
            //create restClient
            RestClient client = new RestClient("http://localhost:5004/");

            //preparing request to remove server
            RestRequest request = new RestRequest($"api/RegisterServer/remove?client_id={client_id}&server_id={server_id}", Method.Delete);

            //send request 
            RestResponse rr = client.Execute(request);

            // if error occured
            if (!rr.IsSuccessStatusCode)
            {
                return new Response() { Data = null, IsError = true, ErrorMessage = "Worker pod is not available", StatusCode = 500 };
            }

            return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200 };

        }

        public static Response RegsiterAPIFlow(int client_id, int flow_id)
        {
            //create restClient
            RestClient client = new RestClient("http://localhost:5004/");

            //preparing request to register server 
            RestRequest request = new RestRequest("api/RegisterServer/registerAPIFlow", Method.Post);
            // Add query parameters
            request.AddQueryParameter("client_id", client_id);
            request.AddQueryParameter("flow_id", flow_id);

            //sending request to worker pod
            RestResponse rr = client.Execute(request);

            // marked for removal
            if (!rr.IsSuccessStatusCode)
            {
                return new Response() { Data = null, IsError = true, ErrorMessage = "Worker pod is not available", StatusCode = 500 };
            }
            return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200 };
        }
    }
}
