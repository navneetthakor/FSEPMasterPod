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
        [Route("WorkerHeartBeat")]
        public Response WorkerHeartBeat([FromServices] IRecurringJobManager recurringJobManager ,string worker_id)
        {
            try
            {
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
        [HttpGet]
        [Route("UserMngHeartBeat")]
        public Response UserMngHeartBeat([FromServices] IRecurringJobManager recurringJobManager ,string worker_id)
        {
            try
            {
                //Update hangfire entry 
                string? message = "Hello admin! None of the User-Management POD had reported in last time span";
                recurringJobManager.AddOrUpdate("Checking-health", () => MyKafkaProducer.NotifyAdmin(message), "*/3 * * * *");

                // return response
                return new Response { Data = null, ErrorMessage = "successfully reported", IsError = false };


            }
            catch (Exception ex)
            {
                return new Response { Data = null, ErrorMessage = ex.Message, IsError = true };
            }
        }
        [HttpGet]
        [Route("AlertingSysHeartBeat")]
        public Response AlertingSysHeartBeat([FromServices] IRecurringJobManager recurringJobManager ,string worker_id)
        {
            try
            {
                //Update hangfire entry 
                string? message = "Hello admin! None of the Alerting-System POD had reported in last time span";
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
