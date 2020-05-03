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
    public static class ERAUserRiskRead
    {
        [FunctionName("ERAUserRiskRead")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Get User Risk Request ");
            HttpResponseMessage response = null;
            ERAUserRiskWorker worker = null;
            var message = "";
            try
            {
                var request = await req.Content.ReadAsAsync<ERAUserRiskReadRequest>();
                if (request != null)
                {
                    log.LogInformation("Processing get user risk Request ");
                    worker = new ERAUserRiskWorker(log);
                    var reponse = await worker.GetUserRisks(request);

                    if (reponse != null)
                    {
                        message = "Get user risk request successful!!";
                        log.LogInformation(message);
                    }
                    else
                    {
                        message = "No user risk match request!!";
                        log.LogInformation(message);
                    }
                    response = req.CreateResponse(HttpStatusCode.OK, new ResponseMessage<ERAUserRiskResponse>() { Message = message, Content = reponse });
                }
                else
                {
                    message = "Failed to parse request";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest, new ResponseMessage<ERAUserRiskResponse>() { Message = message, Content = null });
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<ERAUserRiskResponse>() { Message = ex.Message, Content = null });
            }
            log.LogInformation("End - Get User Risk Request ");
            return response;
        }
    }
}
