using System.Data;
using wps_master_pod_1.Repositories;
using wps_master_pod_1.Modal;
using wps_master_pod_1.Modal.POCOS;
using ServiceStack.OrmLite;
using Microsoft.AspNetCore.Mvc;
using wps_master_pod_1.BL.Interface;

namespace wps_master_pod_1.BL.Services
{
    public class DbMPD01 : IDbUptimeWorker
    {
        /// <summary>
        /// Database connection
        /// </summary>
        private IDbConnection _dbConnection;

        /// <summary>
        /// Constructor for the DbUptimeWorker class
        /// </summary>
        /// <param name="dbService"></param>
        public DbMPD01([FromServices] IDataBaseService dbService)
        {
            _dbConnection = dbService.db;
        }


        /// <summary>
        /// Method to add server to client
        /// </summary>
        /// <param name="clinet_id">client id (who owns server)</param>
        /// <param name="server_id">server id (unique id of server)</param>
        /// <param name="worker_id">worker pod id (string)</param>
        /// <returns>Response Object</returns>
        public Response AddServer(int clinet_id, int server_id, string worker_id)
        {
            try
            {
                //create POCO object
                Response preSaveResponse = PreSave(clinet_id, server_id, worker_id);

                // if error occured
                if (preSaveResponse.IsError)
                {
                    return preSaveResponse;
                }

                //Getting POCO object from PreSave method
                MPD01 mpd01 = preSaveResponse.Other;


                // validate POCO object
                Response validateResponse = ValidateOnSave(mpd01);

                // if error occured
                if (validateResponse.IsError)
                {
                    return validateResponse;
                }

                //save to database
                return Save(mpd01);



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = ex.Message, StatusCode = 500 };
            }
        }

        /// <summary>
        /// Method to remove server from client
        /// </summary>
        /// <param name="clinet_id">Clinet id (who owns the server)</param>
        /// <param name="server_id">server id (unique id for server)</param>
        /// <returns>Response Object</returns>
        public Response RemoveServer(int clinet_id, int server_id)
        {
            try
            {
                //create POCO object
                Response preDeleteResponse = PreDelete(clinet_id, server_id);

                // if error occured
                if (preDeleteResponse.IsError)
                {
                    return preDeleteResponse;
                }

                //Getting POCO object from PreSave method
                MPD01 mpd01 = preDeleteResponse.Other;

                // validate database before final deletion
                Response validationRespones = ValidateOnDelete(preDeleteResponse.Other);

                // if error occured
                if (validationRespones.IsError)
                {
                    return validationRespones;
                }

                //delete from database
                return Delete(mpd01);
            }
            catch (Exception ex)
            {
                //if error occured
                Console.WriteLine("Error : " + ex.Message);
                return new Response() { Data = null, IsError = true, ErrorMessage = "Internal server error", StatusCode = 500 };
            }
        }

        /// <summary>
        /// Method to create POCO object
        /// </summary>
        /// <param name="clinet_id">client id (who owns server)</param>
        /// <param name="server_id">server id (unique id of server)</param>
        /// <param name="worker_id">worker pod id (string)</param>
        /// <returns>Response Object</returns>
        private Response PreSave(int clinet_id, int server_id, string worker_id)
        {
            try
            {
                //check for invalid data
                if (!(clinet_id > 0)  || !(clinet_id > 0) || string.IsNullOrEmpty(worker_id))
                {
                    return new Response() { Data = null, IsError = true, ErrorMessage = "Invalid data", StatusCode = 400 };
                }

                //create POCO object
                MPD01 mpd01 = new MPD01()
                {
                    D01F01 = clinet_id,
                    D01F02 = server_id,
                    D01F03 = worker_id
                };

                // return response
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = mpd01 };
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
        /// <param name="mpd01">POCO object</param>
        /// <returns></returns>
        private Response ValidateOnSave(MPD01 mpd01)
        {
            try
            {
                //check wheather client have entry for this server before 
                //if yes then return error
                //if no then return success
                MPD01? oldEntry = _dbConnection.Single<MPD01>(x => x.D01F01 == mpd01.D01F01 && x.D01F02 == mpd01.D01F02);

                // if entry found then return error
                if (oldEntry != null)
                {
                    return new Response() { Data = null, IsError = true, ErrorMessage = "Client already have entry for this server", StatusCode = 400 };
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
        /// <param name="mpd01">POCO Object</param>
        /// <returns></returns>
        private Response Save(MPD01 mpd01)
        {
            try
            {
                //Insert into database 
                long result = _dbConnection.Insert(mpd01, true);

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
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = mpd01 };
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
        /// <param name="clinet_id">clinet id (who owns server)</param>
        /// <param name="server_id">server id (unique id for server)</param>
        /// <returns>Response object</returns>
        private Response PreDelete(int clinet_id, int server_id)
        {
            try
            {
                //check for invalid data
                if (!(clinet_id > 0) || !(server_id > 0))
                {
                    return new Response() { Data = null, IsError = true, ErrorMessage = "Invalid data", StatusCode = 400 };
                }

                //create POCO object
                MPD01 mpd01 = new MPD01()
                {
                    D01F01 = clinet_id,
                    D01F02 = server_id
                };

                // return response
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = mpd01 };
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
        /// <param name="mpd01">poco object</param>
        /// <returns>Response Object</returns>
        private Response ValidateOnDelete(MPD01 mpd01)
        {
            try
            {
                //check wheather client have entry for this server before 
                //if yes then return success
                //if no then return error
                MPD01? oldEntry = _dbConnection.Single<MPD01>(x => x.D01F01 == mpd01.D01F01 && x.D01F02 == mpd01.D01F02);

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
        /// <param name="mpd01">POCO object</param>
        /// <returns>Response Object</returns>
        private Response Delete(MPD01 mpd01)
        {
            try
            {
                //Delete from database 
                long result = _dbConnection.Delete<MPD01>(x => x.D01F01 == mpd01.D01F01 && x.D01F02 == mpd01.D01F02);

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
                return new Response() { Data = null, IsError = false, ErrorMessage = "successfull", StatusCode = 200, Other = mpd01 };
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
