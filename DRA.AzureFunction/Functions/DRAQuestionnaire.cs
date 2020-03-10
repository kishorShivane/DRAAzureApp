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
    public static class DRAQuestionnaire
    {
        [FunctionName("DRAQuestionnaire")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Start - Questionnaire Request ");
            HttpResponseMessage response = null;
            QuestionnaireWorker worker = null;
            var message = "";
            try
            {
                var request= await req.Content.ReadAsAsync<QuestionnaireRequest>();
                if (request != null)
                {
                    log.LogInformation("Processing questionnaire Request for User");
                    worker = new QuestionnaireWorker(log);
                    var result = await worker.InsertUserCompetencyMatrices(request);

                    if (result != null)
                    {
                        switch (result.Item3)
                        {
                            case 1:
                                response = req.CreateResponse(HttpStatusCode.Created, new ResponseMessage<QuestionnaireResponse>() { Message = result.Item1, Content = result.Item2});
                                break;
                            case -1:
                            case -2:
                                response = req.CreateResponse(HttpStatusCode.BadRequest,result.Item1);
                                break;
                            default:
                                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                                break;
                        }
                    }
                    else
                    {
                        message = "Failed to execure the request!!";
                        log.LogInformation(message);
                        response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<QuestionnaireResponse>() { Message = message, Content = null});
                    }
                }
                else
                {
                    message = "Failed to parse request";
                    log.LogError(message);
                    response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest, new ResponseMessage<QuestionnaireResponse>() { Message = message, Content = null });
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.InternalServerError, new ResponseMessage<UserModel>() { Message = ex.Message, Content = null });
            }
            log.LogInformation("End - Questionnaire Request ");
            return response;
        }
    }
}
