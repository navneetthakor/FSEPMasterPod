using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using System.Configuration;
using wps_master_pod_1.BL.Services;
using wps_master_pod_1.Modal;
using wps_master_pod_1.Repositories;

namespace wps_master_pod_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerPodController : ControllerBase
    {
        private DbMPD02 _dbConnection;
        public WorkerPodController([FromServices] IDataBaseService dataBaseSevice)
        {
            _dbConnection = new DbMPD02(dataBaseSevice);
        }

        [HttpGet]
        [Route("HeartBeat")]
        public Response HeartBeat([FromServices] IRecurringJobManager recurringJobManager ,string worker_id)
        {
            try
            {
                //--- older code --- 
                //try to update the worker pod status in Database first 
                //Response response = _dbConnection.AcknowledgeServer(worker_id);

                //if(response.IsError)
                //{
                //    //if error occured then push this to stackStore 
                //    //so that it can be processed later
                //    stackStore.AddWorker(worker_id);
                //}
                //return response;


                //--- newer code --- 
                //Update hangfire entry 
                string? message = "Hello admin! None of the worker POD had reported in last time span";
                recurringJobManager.AddOrUpdate("Checking-health", () => MyKafkaProducer.NotifyAdmin(message), "*/3 * * * *");

                // return response
                return new Response { Data = null, ErrorMessage = "successfully reported", IsError = false };


            }
            catch (Exception ex)
            {
                return new Response { Data = null, ErrorMessage = ex.Message, IsError = true };
            }
        }

    }
}
