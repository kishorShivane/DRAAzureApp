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
    public static class DRAResetPassword
    {
        [FunctionName("DRAResetPassword")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Reset Password Request ");
            HttpResponseMessage response = null;
            ResetPasswordWorker worker = null;
            var message = "";
            try
            {
                var user = await req.Content.ReadAsAsync<ResetPassword>();
                if (user != null)
                {
                    log.LogInformation("Processing Reset Password Request for User: " + user.Email);
                    worker = new ResetPasswordWorker(log);
                    message = await worker.ResetPassword(user);

                    if (message == "1")
                    {
                        message = "Password updated successfully for user: " + user.Email;
                        log.LogInformation(message);
                        response = req.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else if (message == "-1")
                    {
                        message = "User does not exist with email: " + user.Email;
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

                log.LogInformation("End - Reset Password Request ");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                response = req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }
    }
}
