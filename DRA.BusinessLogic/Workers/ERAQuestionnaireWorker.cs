using DRA.BusinessLogic.Mapper;
using DRA.DataProvider.Models;
using DRA.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class ERAQuestionnaireWorker : BaseWorker
    {
        public ERAQuestionnaireWorker(ILogger log) : base(log)
        { }

        public async Task<ERAQuestionnaireResponse> GetQuestions(ERAQuestionnaireRequest request)
        {
            ERAQuestionnaireResponse response = null;
            List<ERAQuestionModel> questionsModel = null;
            var questionRepo = unitOfWork.GetRepository<Question>();
            var questions = await Task.Run(() => questionRepo.Get(x => (x.RiskId == request.RiskID || request.RiskID == 0) && (x.QuestionId == request.QuestionID || request.QuestionID == 0)).ToList());
            if (questions != null && questions.Any())
            {
                questionsModel = DataToDomain.MapQuestionToERAQuestions(questions);
                response = new ERAQuestionnaireResponse() { Questions = questionsModel, TotalRecords = questions.Count() };
            }
            return response;
        }
    }
}
