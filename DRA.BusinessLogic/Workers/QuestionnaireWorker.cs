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

        public async Task<Tuple<string, bool, int>> InsertUserCompetencyMatrices(QuestionnaireRequest request)
        {
            bool response = false;
            int status = 0;
            var message = "";
            List<UserCompetencyMatrix> userCompetencies = new List<UserCompetencyMatrix>();
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await Task.Run(() => userRepo.Get(x => x.UserID == request.UserID).FirstOrDefault());
            if (user != null)
            {
                var jobMetricsRepo = unitOfWork.GetRepository<JobCompetencyMatrix>();
                var jobRepo = unitOfWork.GetRepository<Job>();
                var job = await Task.Run(() => jobRepo.Get(x => x.JobID == user.JobID).FirstOrDefault());
                var userMetricsRepo = unitOfWork.GetRepository<UserCompetencyMatrix>();

                var comps = request.Competency.Split(';'); var points = request.Points.Split(';');
                if (comps.Length == points.Length)
                {
                    UserCompetencyMatrix userCompetency;

                    for (int i = 0; i < comps.Length; i++)
                    {
                        var comp = comps[i];
                        var jobMetric = await Task.Run(() => jobMetricsRepo.Get(x => x.JobID == user.JobID && x.Competency.Equals(comp)).FirstOrDefault());
                        if (jobMetric != null)
                        {
                            userCompetency = new UserCompetencyMatrix();
                            userCompetency.UserID = Convert.ToByte(request.UserID);
                            userCompetency.Type = jobMetric.Type;
                            userCompetency.MainGroup = jobMetric.Maingroup;
                            userCompetency.SubGroup = jobMetric.Subgroup;
                            userCompetency.Competency = comp;
                            userCompetency.LoW = jobMetric.LoW;
                            userCompetency.RequiredLevel = jobMetric.RequiredLevel;
                            userCompetency.CurrentLevel = Convert.ToInt32(Math.Round(Convert.ToDouble(points[i])));
                            userCompetency.RatingDate = DateTime.Now;

                            var gap = jobMetric.RequiredLevel - userCompetency.CurrentLevel;
                            userCompetency.Gap = gap > 0 ? gap : 0;

                            userCompetencies.Add(userCompetency);
                        }
                        else
                        {
                            status = -1;
                            message = "Job Metric does not exist for UserID: " + request.UserID;
                            break;
                        }
                    }
                }

                if (userCompetencies.Any())
                {
                    userMetricsRepo.InsertAll(userCompetencies);
                    unitOfWork.Save();

                    status = 1;
                    message = "Data inserted successfully for UserID: " + request.UserID;
                    response = true;
                }
                else
                {
                    status = -1;
                    message = "No Competencies exist for UserID: " + request.UserID;
                }
            }
            else
            {
                status = -2;
                message = "User does not exist with UserID: " + request.UserID;
            }
            return new Tuple<string, bool, int>(message, response, status);
        }
    }
}
