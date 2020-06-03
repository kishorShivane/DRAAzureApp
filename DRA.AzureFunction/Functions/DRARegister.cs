using DRA.BusinessLogic.Workers;
using DRA.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DRA.AzureFunction.Functions
{
    public static class DRARegister
    {
        [FunctionName("DRARegister")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Registration Request ");
            HttpResponseMessage response = null;
            RegistrationWorker worker = null;
            var message = "";
            try
            {
                var user = await req.Content.ReadAsAsync<UserModel>();
                if (user != null)
                {
                    log.LogInformation("Processing Login Request for User: " + user.UserID);
                    worker = new RegistrationWorker(log);
                    var result = await worker.ValidateUserAndRegister(user);

                    if (result != null)
                    {
                        var sendMe = new ResponseMessage<UserModel>() { Message= result.Item1, Content = result.Item2};
                        switch (result.Item3)
                        {
                            case 1:
                                response = req.CreateResponse(HttpStatusCode.Created, sendMe);
                                break;
                            case -1:
                                response = req.CreateResponse(HttpStatusCode.Conflict, sendMe);
                                break;
                            case -2:
                                response = req.CreateResponse(HttpStatusCode.BadRequest, sendMe);
                                break;
                            default:
                                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                                break;
                        }
                    }
                    else
                    {
                        message = "Failed to execute the request!!";
                        log.LogInformation(message);
                        response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<UserModel>() { Message = message, Content = null });
                    }
                }
                else
                {
                    message = "Failed to parse user";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest, new ResponseMessage<UserModel>() { Message = message, Content = null });
                }
                log.LogInformation("End - Registration Request ");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<UserModel>() { Message = ex.Message, Content = null });
            }           
            return response;
        }
    }
}
