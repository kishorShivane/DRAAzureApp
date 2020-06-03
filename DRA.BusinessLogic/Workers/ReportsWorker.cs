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
    public class ReportsWorker : BaseWorker
    {
        public ReportsWorker(ILogger log) : base(log)
        {

        }

        public async Task<Tuple<string, List<UserCompetencyMatrixModel>>> GetUserCompetencyMatrix(CompetenciesReportRequest request)
        {
            List<UserCompetencyMatrixModel> response = null;
            var message = "";
            var competencyRepo = unitOfWork.GetRepository<UserCompetencyMatrix>();
            var competencies = await Task.Run(() => competencyRepo.Get(x => x.UserID == request.UserID &&
            (string.IsNullOrEmpty(request.Type) || x.Type == request.Type) &&
            (string.IsNullOrEmpty(request.MainGroup)|| x.Type == request.MainGroup) &&
            (string.IsNullOrEmpty(request.SubGroup) || x.Type == request.SubGroup) &&
            (string.IsNullOrEmpty(request.Competency)|| x.Type == request.Competency)).ToList());

            if (competencies.Any())
            {
                response = new List<UserCompetencyMatrixModel>();
                competencies.ForEach(x =>
                {
                    response.Add(new UserCompetencyMatrixModel()
                    {
                        Competency = x.Competency,
                        CurrentLevel = x.CurrentLevel,
                        Gap = x.Gap,
                        ID = x.ID,
                        LoW = x.LoW,
                        MainGroup = x.MainGroup,
                        RatingDate = x.RatingDate,
                        RequiredLevel = x.RequiredLevel,
                        SubGroup = x.SubGroup,
                        Type = x.Type,
                        UserID = x.UserID
                    });
                });
                message = "Found " + response.Count() + " records";
            }
            log.LogDebug(message);
            return new Tuple<string, List<UserCompetencyMatrixModel>>(message, response);
        }
    }
}
