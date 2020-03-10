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
    public class QuestionnaireWorker : BaseWorker
    {
        public QuestionnaireWorker(ILogger log) : base(log)
        { }

        public async Task<Tuple<string, QuestionnaireResponse, int>> InsertUserCompetencyMatrices(QuestionnaireRequest request)
        {
            QuestionnaireResponse response = null;
            int status = 0;
            var message = "";
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await Task.Run(() => userRepo.Get(x=>x.UserID == request.UserID).FirstOrDefault());
            if (user != null)
            {
                var jobMetricsRepo = unitOfWork.GetRepository<JobCompetencyMatrix>();
                var jobRepo = unitOfWork.GetRepository<Job>();
                var jobMetric = await Task.Run(() => jobMetricsRepo.Get(x => x.JobID == user.JobID).FirstOrDefault());
                var job = await Task.Run(() => jobRepo.Get(x => x.JobID == user.JobID).FirstOrDefault());
                if (jobMetric != null)
                {
                    var userMetricsRepo = unitOfWork.GetRepository<UserCompetencyMatrix>();
                    UserCompetencyMatrix userCompetency = new UserCompetencyMatrix();
                    userCompetency.UserID = Convert.ToByte(request.UserID);
                    userCompetency.Type = jobMetric.Type;
                    userCompetency.MainGroup = jobMetric.Maingroup;
                    userCompetency.SubGroup = jobMetric.Subgroup;
                    userCompetency.Competency = request.Competency;
                    userCompetency.LoW = jobMetric.LoW;
                    userCompetency.RequiredLevel = jobMetric.RequiredLevel;
                    userCompetency.CurrentLevel = request.Points / request.NumberOfQuestion;
                    userCompetency.RatingDate = DateTime.Now;
                    userCompetency.Gap = jobMetric.RequiredLevel - userCompetency.CurrentLevel;

                    userMetricsRepo.Insert(userCompetency);
                    unitOfWork.Save();

                    response = new QuestionnaireResponse();
                    response.UserFullName = user.UserName + ", " + user.UserSurname;
                    response.UserEmail = user.UserEmail;
                    response.Industry = user.Industry;
                    response.Organization = user.Organization;
                    response.BusinessFunction = user.BusinessFunction;
                    response.JobTitle = job.JobTitle;
                    response.Type = userCompetency.Type;
                    response.MainGroup = userCompetency.MainGroup;
                    response.SubGroup = userCompetency.SubGroup;
                    response.Competency = userCompetency.Competency;
                    response.LoW = userCompetency.LoW;
                    response.RequiredLevel = userCompetency.RequiredLevel;
                    response.CurrentLevel = userCompetency.CurrentLevel;
                    response.Gap = userCompetency.Gap;

                    status = 1;
                    message = "Data inserted successfully for UserID: " + request.UserID;
                }
                else
                {
                    status = -1;
                    message = "Job Metric does not exist for UserID: " + request.UserID;
                }
            }
            else
            {
                status = -2;
                message = "User does not exist with UserID: "+ request.UserID;
            }
            return new Tuple<string, QuestionnaireResponse, int>(message, response, status);
        }
    }
}
