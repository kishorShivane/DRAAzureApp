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
    public static class ERARisk
    {
        [FunctionName("ERARisk")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Risk Request ");
            HttpResponseMessage response = null;
            ERARiskWorker worker = null;
            var message = "";
            try
            {
                var request = await req.Content.ReadAsAsync<ERARiskRequest>();
                if (request != null)
                {
                    log.LogInformation("Processing risk Request ");
                    worker = new ERARiskWorker(log);
                    var reponse = await worker.GetRisks(request);

                    if (reponse != null)
                    {
                        message = "Risk request successful!!";
                        log.LogInformation(message);
                    }
                    else
                    {
                        message = "No Risk match request!!";
                        log.LogInformation(message);
                    }
                    response = req.CreateResponse(HttpStatusCode.OK, new ResponseMessage<ERARiskResponse>() { Message = message, Content = reponse });
                }
                else
                {
                    message = "Failed to parse request";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest, new ResponseMessage<ERARiskResponse>() { Message = message, Content = null });
                }

            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<ERARiskResponse>() { Message = ex.Message, Content = null });
            }
            log.LogInformation("End - Risk Request ");
            return response;
        }
    }
}
