using System.Data;
using wps_master_pod_1.Repositories;
using wps_master_pod_1.Modal;
using wps_master_pod_1.Modal.POCOS;
using ServiceStack.OrmLite;
using Microsoft.AspNetCore.Mvc;
using wps_master_pod_1.BL.Interface;
using wps_master_pod_1.Modal.Enums;

namespace wps_master_pod_1.BL.Services
{
    public class DbMPD02
    {
        /// <summary>
        /// Database connection
        /// </summary>
        private IDbConnection _dbConnection;

        /// <summary>
        /// Constructor for the DbUptimeWorker class
        /// </summary>
        /// <param name="dbService"></param>
        public DbMPD02([FromServices] IDataBaseService dbService)
        {
            _dbConnection = dbService.db;
        }


        /// <summary>
        /// Method to register server heartbit
        /// </summary>
        /// <param name="worker_id">worker id</param>
        /// <returns>Response Object</returns>
        public Response AddServer(string worker_id)
        {
            try
            {
                //create POCO object
                Response preSaveResponse = PreSave(worker_id);

                // if error occured
                if (preSaveResponse.IsError)
                {
                    return preSaveResponse;
                }

                //Getting POCO object from PreSave method
                MPD02 MPD02 = preSaveResponse.Other;


                // validate POCO object
                Response validateResponse = ValidateOnSave(MPD02);

                // if error occured
                if (validateResponse.IsError)
                {
                    return validateResponse;
                }

                //save to database
                return Save(MPD02);



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = ex.Message, StatusCode = 500 };
            }
        }

        /// <summary>
        /// Method to remove worker pod from database
        /// </summary>
        /// <param name="worker_id">worker_id</param>
        /// <returns>Response Object</returns>
        public Response RemoveServer(string worker_id)
        {
            try
            {
                //create POCO object
                Response preDeleteResponse = PreDelete(worker_id);

                // if error occured
                if (preDeleteResponse.IsError)
                {
                    return preDeleteResponse;
                }

                //Getting POCO object from PreSave method
                MPD02 mpd02 = preDeleteResponse.Other;

                // validate database before final deletion
                Response validationRespones = ValidateOnDelete(mpd02);

                // if error occured
                if (validationRespones.IsError)
                {
                    return validationRespones;
                }

                //delete from database
                return Delete(mpd02);
            }
            catch (Exception ex)
            {
                //if error occured
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = "Internal server error", StatusCode = 500 };
            }
        }


        public Response AcknowledgeServer(string worker_id)
        {
            try
            {
                //create POCO object
                //_dbConnection.CreateTableIfNotExists<MPD03>();
                Response preSaveResponse = PreSave(worker_id);

                // if error occured
                if (preSaveResponse.IsError)
                {
                    return preSaveResponse;
                }

                //Getting POCO object from PreSave method
                MPD02 MPD02 = preSaveResponse.Other;

                // validate POCO object
                Response validateResponse = ValidateOnSave(MPD02);

                // if error occured
                if (validateResponse.IsError)
                {
                    return validateResponse;
                }

                // update to database
                int updateResult = _dbConnection.UpdateOnly(() => new MPD02 { D02F03 = WorkerPodStatus.R }, where: x => x.D02F01 == MPD02.D02F01);

                // if error occured
                if (updateResult != 1)
                {
                    return new Response() { Data = null, IsError = true, ErrorMessage = "Error while updating database", StatusCode = 500 };
                }

                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = MPD02 };

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = ex.Message, StatusCode = 500 };
            }
        }

        /// <summary>
        /// Method to create POCO object
        /// </summary>
        /// <param name="worker_id">worker pod id (string)</param>
        /// <returns>Response Object</returns>
        private Response PreSave(string worker_id)
        {
            try
            {
                //check for invalid data
                if (string.IsNullOrEmpty(worker_id))
                {
                    return new Response() { Data = null, IsError = true, ErrorMessage = "Invalid data", StatusCode = 400 };
                }

                //create POCO object
                MPD02 mpd02 = new MPD02()
                {
                    D02F01 = worker_id,
                    D02F03 = WorkerPodStatus.R
                };

                // return response
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = mpd02 };
            }
            catch (Exception ex)
            {
                //if error occured
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = "Error while creating POCO", StatusCode = 500 };
            }
        }

        /// <summary>
        /// Method to validate POCO object
        /// </summary>
        /// <param name="MPD02">POCO object</param>
        /// <returns></returns>
        private Response ValidateOnSave(MPD02 MPD02)
        {
            try
            {
                //check wheather client have entry for this server before 
                //if yes then return error
                //if no then return success
                List<MPD02>? oldEntry = _dbConnection.Select<MPD02>(x => x.D02F01 == MPD02.D02F01 );

                // if entry found then return error
                if (oldEntry.Count == 0)
                {
                    return new Response() { Data = null, IsError = true, ErrorMessage = "Entry for worker exists in the table", StatusCode = 400 };
                }


                //return success
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                //if error occured
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = "Error while validating POCO", StatusCode = 500 };
            }
        }

        /// <summary>
        /// Method to save POCO object
        /// </summary>
        /// <param name="MPD02">POCO Object</param>
        /// <returns></returns>
        private Response Save(MPD02 MPD02)
        {
            try
            {
                //Insert into database 
                long result = _dbConnection.Insert(MPD02, true);

                // if error occured
                // result infers that how many rows are affected
                if (result != 1)
                {
                    return new Response()
                    {
                        Data = null,
                        IsError = true,
                        ErrorMessage = "Error while saving POCO",
                        StatusCode = 500
                    };
                }

                //return success
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = MPD02 };
            }
            catch (Exception ex)
            {
                //if error occured
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = "Error while saving POCO", StatusCode = 500 };
            }
        }


        /// <summary>
        /// Method to create POCO object used in deletion process
        /// </summary>
        /// <param name="worker_id">worker id</param>
        /// <returns>Response object</returns>
        private Response PreDelete(string worker_id)
        {
            try
            {
                //check for invalid data
                if (string.IsNullOrEmpty(worker_id))
                {
                    return new Response() { Data = null, IsError = true, ErrorMessage = "Invalid data", StatusCode = 400 };
                }

                //create POCO object
                MPD02 MPD02 = new MPD02()
                {
                    D02F01 = worker_id,
                };

                // return response
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = MPD02 };
            }
            catch (Exception ex)
            {
                //if error occured
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = "Error while creating POCO", StatusCode = 500 };
            }
        }

        /// <summary>
        /// Method to validate Database constraints before processing final deletion
        /// </summary>
        /// <param name="MPD02">poco object</param>
        /// <returns>Response Object</returns>
        private Response ValidateOnDelete(MPD02 MPD02)
        {
            try
            {
                //check wheather client have entry for this server before 
                //if yes then return success
                //if no then return error
                MPD02? oldEntry = _dbConnection.Single<MPD02>(x => x.D02F01 == MPD02.D02F01);

                // if entry found then return success
                if (oldEntry == null)
                {
                    return new Response() { Data = null, IsError = true, ErrorMessage = "Client does not have entry for this server", StatusCode = 400 };
                }

                //return success
                return new Response()
                {
                    Data = null,
                    IsError = false,
                    ErrorMessage = "successfull",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                //if error occured
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = "Error while validation on delete", StatusCode = 500 };
            }
        }

        /// <summary>
        /// Method to delete server from client
        /// </summary>
        /// <param name="MPD02">POCO object</param>
        /// <returns>Response Object</returns>
        private Response Delete(MPD02 MPD02)
        {
            try
            {
                //Delete from database 
                long result = _dbConnection.Delete<MPD02>(x => x.D02F01 == MPD02.D02F01);

                // if error occured
                // result infers that how many rows are affected
                if (result != 1)
                {
                    return new Response()
                    {
                        Data = null,
                        IsError = true,
                        ErrorMessage = "Database error while deleting POCO",
                        StatusCode = 500
                    };
                }

                //return success
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = MPD02 };
            }
            catch (Exception ex)
            {
                //if error occured
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = "Error while deleting POCO", StatusCode = 500 };
            }
        }
    }
}
