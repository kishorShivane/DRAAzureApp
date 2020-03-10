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
    public static class DRAActivateAccount
    {
        [FunctionName("DRAActivateAccount")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Activate account Request ");
            HttpResponseMessage response = null;
            ActivationWorker worker = null;
            var message = "";
            try
            {
                var userID = await req.Content.ReadAsAsync<int>();
                if (userID > 0)
                {
                    log.LogInformation("Processing Activate account Request for User: " + userID);
                    worker = new ActivationWorker(log);
                    message = await worker.ActivateAccount(userID);

                    if (message == "1")
                    {
                        message = "Account activated successfully for user ID: " + userID;
                        log.LogInformation(message);
                        response = req.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else if (message == "-1")
                    {
                        message = "User does not exist with User ID: " + userID;
                        log.LogInformation(message);
                        response = req.CreateResponse(HttpStatusCode.NotFound, message);
                    }
                    else
                    {
                        message = "Failed to execute the request!!";
                        log.LogInformation(message);
                        response = req.CreateResponse(HttpStatusCode.ExpectationFailed, message);
                    }
                }
                else
                {
                    message = "Failed to parse user";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                }

                log.LogInformation("End - Activate account Request ");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                response = req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return response;
        }
    }
}
