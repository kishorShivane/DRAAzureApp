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
    public static class ERAUserAnswerRead
    {
        [FunctionName("ERAUserAnswerRead")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Get User Answer Request ");
            HttpResponseMessage response = null;
            ERAUserAnswerWorker worker = null;
            var message = "";
            try
            {
                var request = await req.Content.ReadAsAsync<ERAUserAnswerReadRequest>();
                if (request != null)
                {
                    log.LogInformation("Processing get answer Request ");
                    worker = new ERAUserAnswerWorker(log);
                    var reponse = await worker.GetUserAnswers(request);

                    if (reponse != null)
                    {
                        message = "Get Answer request successful!!";
                        log.LogInformation(message);
                    }
                    else
                    {
                        message = "No Answers match request!!";
                        log.LogInformation(message);
                    }
                    response = req.CreateResponse(HttpStatusCode.OK, new ResponseMessage<ERAUserAnswerResponse>() { Message = message, Content = reponse });
                }
                else
                {
                    message = "Failed to parse request";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest, new ResponseMessage<ERAUserAnswerResponse>() { Message = message, Content = null });
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<ERAUserAnswerResponse>() { Message = ex.Message, Content = null });
            }
            log.LogInformation("End - Get User Answer Request ");
            return response;
        }
    }
}
