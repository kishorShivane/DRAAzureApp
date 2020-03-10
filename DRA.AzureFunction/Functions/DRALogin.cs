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
    public static class DRALogin
    {
        [FunctionName("DRALogin")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Login Request ");
            HttpResponseMessage response = null;
            LoginWorker worker = null;
            var message = "";
            try
            {
                var user = await req.Content.ReadAsAsync<UserModel>();
                if (user != null)
                {
                    log.LogInformation("Processing Login Request for User: " + user.UserName);
                    worker = new LoginWorker(log);
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
                    response = req.CreateResponse(HttpStatusCode.OK, new ResponseMessage<UserModel>() { Message = message, Content = userExist });
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
