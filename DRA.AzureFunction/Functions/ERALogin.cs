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
    public static class ERALogin
    {
        [FunctionName("ERALogin")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - ERA Login Request ");
            HttpResponseMessage response = null;
            ERALoginWorker worker = null;
            var message = "";
            try
            {
                var user = await req.Content.ReadAsAsync<ERAUserModel>();
                if (user != null)
                {
                    log.LogInformation("Processing ERA Login Request for User: " + user.Email);
                    worker = new ERALoginWorker(log);
                    var userExist = await worker.ValidateUserCredential(user);

                    if (userExist != null)
                    {
                        message = "User validated successfully!!";
                        log.LogInformation(message);
                    }
                    else
                    {
                        message = "User does not exist!!";
                        log.LogInformation(message);
                    }
                    response = req.CreateResponse(HttpStatusCode.OK, new ResponseMessage<ERAUserModel>() { Message = message, Content = userExist });
                }
                else
                {
                    message = "Failed to parse user";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest, new ResponseMessage<UserModel>() { Message = message, Content = null });
                }

                log.LogInformation("End - Login Request ");
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
