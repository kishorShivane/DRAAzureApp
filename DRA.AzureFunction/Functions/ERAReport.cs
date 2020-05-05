using DRA.BusinessLogic.Workers;
using DRA.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DRA.AzureFunction.Functions
{
    public static class ERAReport
    {
        [FunctionName("ERAReport")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - ERA Report Request ");
            HttpResponseMessage response = null;
            ERAReportWorker worker = null;
            var message = "";
            try
            {
                var request = await req.Content.ReadAsAsync<ERAReportRequest>();
                if (request != null)
                {
                    log.LogInformation("Processing ERA Report Request");
                    worker = new ERAReportWorker(log);
                    var result = await worker.GetERAUserReport(request);

                    if (result != null)
                    {
                        var sendMe = new ResponseMessage<List<ERAUserModel>>() { Message = result.Item1, Content = result.Item2 };
                        response = req.CreateResponse(HttpStatusCode.OK, sendMe);
                    }
                    else
                    {
                        message = "Failed to execute the request!!";
                        log.LogInformation(message);
                        response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<List<ERAUserModel>>() { Message = message, Content = null });
                    }
                }
                else
                {
                    message = "Failed to parse request";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest, new ResponseMessage<List<ERAUserModel>>() { Message = message, Content = null });
                }
                log.LogInformation("End - ERA Report Request ");
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
