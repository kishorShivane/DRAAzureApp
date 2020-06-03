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
    public static class ERAUserProfileRead
    {
        [FunctionName("ERAUserProfileRead")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - User Profile Read Request ");
            HttpResponseMessage response = null;
            ERAUserProfileWorker worker = null;
            var message = "";
            try
            {
                var user = await req.Content.ReadAsAsync<ERAUserModel>();
                if (user != null)
                {
                    log.LogInformation("Processing User Profile Read for User: " + user.UserId);
                    worker = new ERAUserProfileWorker(log);
                    var result = await worker.GetUserProfile(user);

                    if (result != null)
                    {
                        var sendMe = new ResponseMessage<ERAUserModel>() { Message = result.Item1, Content = result.Item2 };
                        switch (result.Item3)
                        {
                            case 1:
                                response = req.CreateResponse(HttpStatusCode.OK, sendMe);
                                break;
                            case -1:
                                response = req.CreateResponse(HttpStatusCode.NotFound, sendMe);
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
                log.LogInformation("End - User Profile Read Request ");
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
