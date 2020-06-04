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
            var isMissMatch = false;
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
                        userCompetency = new UserCompetencyMatrix();
                        var comp = comps[i];
                        var jobMetric = await Task.Run(() => jobMetricsRepo.Get(x => x.JobID == user.JobID && x.Competency.Equals(comp)).FirstOrDefault());
                        if (jobMetric != null)
                        {
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
                            if (Convert.ToDouble(points[i]) <= 0)
                            {
                                log.LogError("Current Level in the request is 0 for: " + request.UserID + " with competency :" + comp + " below is the data." + "\n" +
                                " Competency Object:\n" +
                                " UserID = " + request.UserID + "\n" +
                                " Competency = " + comp + "\n" +
                                " Points = " + points[i] + "\n");
                            }
                        }
                        else
                        {
                            status = -1;
                            log.LogError("Job Metric does not exist for UserID: " + request.UserID + " with competency :" + comp + " below is the data." + "\n" +
                                " Competency Object:\n" +
                                " UserID = " + request.UserID + "\n" +
                                " Competency = " + comp + "\n" +
                                " Points = " + points[i] + "\n");
                        }
                    }
                }
                else
                {
                    isMissMatch = true;
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
                    if (isMissMatch)
                        message = "Error in the Request: Number of Competencies does not match number of points in the request data passed.";
                    else
                        message = "No Competencies exist for UserID: " + request.UserID;
                }
            }
            else
            {
                status = -2;
                message = "User does not exist with UserID: " + request.UserID;
            }
            log.LogDebug(message);
            return new Tuple<string, bool, int>(message, response, status);
        }
    }
}
