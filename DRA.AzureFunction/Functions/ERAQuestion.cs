using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DRA.BusinessLogic.Workers;
using DRA.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DRA.AzureFunction.Functions
{
    public static class ERAQuestion
    {
        [FunctionName("ERAQuestion")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Question Request ");
            HttpResponseMessage response = null;
            ERAQuestionWorker worker = null;
            var message = "";
            try
            {
                var request = await req.Content.ReadAsAsync<ERAQuestionRequest>();
                if (request != null)
                {
                    log.LogInformation("Processing question Request ");
                    worker = new ERAQuestionWorker(log);
                    var reponse = await worker.GetQuestions(request);

                    if (reponse != null)
                    {
                        message = "Questions request successful!!";
                        log.LogInformation(message);
                    }
                    else
                    {
                        message = "No Questions match request!!";
                        log.LogInformation(message);
                    }
                    response = req.CreateResponse(HttpStatusCode.OK, new ResponseMessage<ERAQuestionResponse>() { Message = message, Content = reponse }) ;
                }
                else
                {
                    message = "Failed to parse request";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest, new ResponseMessage<ERAQuestionResponse>() { Message = message, Content = null });
                }

                log.LogInformation("End - Question Request ");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<ERAQuestionResponse>() { Message = ex.Message, Content = null });
            }

            return response;
        }
    }
}
