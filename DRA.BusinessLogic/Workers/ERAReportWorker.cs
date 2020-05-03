using DRA.BusinessLogic.Mapper;
using DRA.DataProvider.Models;
using DRA.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class ERAReportWorker : BaseWorker
    {
        public ERAReportWorker(ILogger log) : base(log)
        {

        }

        public async Task<Tuple<string, List<ERAUserModel>>> GetERAUserReport(ERAReportRequest request)
        {
            List<ERAUserModel> response = null;
            var message = "";
            var userRepo = unitOfWork.GetRepository<OnlineAssessmentUser>();
            var userRiskRepo = unitOfWork.GetRepository<UserRisk>();
            var users = await Task.Run(() => userRepo.Get(x => (request.UserID == 0 || x.UserId == request.UserID) &&
            (string.IsNullOrEmpty(request.Email) || x.Email.Contains(request.Email))).ToList());

            if (users.Any())
            {
                var userTakenTest = userRiskRepo.Get().ToList();
                if (userTakenTest != null)
                {
                    response = new List<ERAUserModel>();
                    var userTakenTestIDs = userTakenTest.Select(x => x.UserId).Distinct().ToList();
                    userTakenTestIDs.ForEach(x =>
                    {
                        var user = users.FirstOrDefault(z => z.UserId == x);
                        if(user != null)
                        {
                            response.Add(DataToDomain.MapUserToERAUserModel(user));
                        }
                    });
                    message = "Found " + response.Count() + " records";
                }
            }

            return new Tuple<string, List<ERAUserModel>>(message, response);
        }
    }
}
