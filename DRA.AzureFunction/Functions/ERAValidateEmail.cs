using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DRA.BusinessLogic.Workers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DRA.AzureFunction.Functions
{
    public static class ERAValidateEmail
    {
        [FunctionName("ERAValidateEmail")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Validate email Request ");
            HttpResponseMessage response = null;
            ERAResetPasswordWorker worker = null;
            var message = "";
            try
            {
                var email = await req.Content.ReadAsAsync<string>();
                if (string.IsNullOrEmpty(email))
                {
                    log.LogInformation("Processing validate email for User: " + email);
                    worker = new ERAResetPasswordWorker(log);
                    message = await worker.ValidateEmail(email);

                    if (message == "1")
                    {
                        message = "Email validated successfully for user: " + email;
                        log.LogInformation(message);
                        response = req.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else if (message == "-1")
                    {
                        message = "Email does not exist in the system!!";
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
                    message = "Failed to parse email";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                }

                log.LogInformation("End - Validate email Request ");
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
