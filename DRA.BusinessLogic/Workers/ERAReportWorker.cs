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
            var users = await Task.Run(() => userRepo.Get(x => (string.IsNullOrEmpty(request.LastName) || x.LastName.Contains(request.LastName)) &&
            (string.IsNullOrEmpty(request.Email) || x.Email.Contains(request.Email)) &&
            (string.IsNullOrEmpty(request.CompanyName) || x.CompanyName.Contains(request.CompanyName))).ToList());

            if (users.Any())
            {
                var userTakenTest = userRiskRepo.Get().ToList();
                if (userTakenTest != null)
                {
                    response = new List<ERAUserModel>();
                    var userTakenTestIDs = request.TestIdentifier.HasValue ? userTakenTest.Select(x => new { x.UserId, x.TestIdentifier, x.AssesmentDate }).Where(x => x.TestIdentifier == request.TestIdentifier).Distinct().ToList() :
                        userTakenTest.Select(x => new { x.UserId, x.TestIdentifier, x.AssesmentDate }).Distinct().ToList();

                    userTakenTestIDs.ForEach(x =>
                    {
                        var user = users.FirstOrDefault(z => z.UserId == x.UserId);
                        if (user != null)
                        {
                            response.Add(DataToDomain.MapUserToERAUserModel(user, true, x.AssesmentDate, x.TestIdentifier));
                        }
                    });
                    message = "Found " + response.Count() + " records";
                }
            }

            return new Tuple<string, List<ERAUserModel>>(message, response);
        }
    }
}
