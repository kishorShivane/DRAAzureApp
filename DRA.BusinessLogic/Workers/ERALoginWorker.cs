using DRA.BusinessLogic.Mapper;
using DRA.DataProvider.Models;
using DRA.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class ERALoginWorker : BaseWorker
    {
        public ERALoginWorker(ILogger log) : base(log)
        {
        }

        public async Task<ERAUserModel> ValidateUserCredential(ERAUserModel userModel)
        {
            ERAUserModel result = null;
            var userRepo = unitOfWork.GetRepository<OnlineAssessmentUser>();
            var userTypeRepo = unitOfWork.GetRepository<UserType>();
            var userRiskRepo = unitOfWork.GetRepository<UserRisk>();
            var user = await Task.Run(() => userRepo.Get(x => x.Email == userModel.Email && x.Password== userModel.Password).FirstOrDefault());
            if (user != null)
            {
                result = DataToDomain.MapUserToERAUserModel(user);
                var userType = await Task.Run(() => userTypeRepo.Get(x => x.UserTypeId == user.UserTypeId).FirstOrDefault());
                if (userType != null)
                {
                    result.UserType = userType.UserTypeName;
                }
                var userRisk = userRiskRepo.Get(x => x.UserId == user.UserId).ToList();
                if (userRisk != null)
                {
                    var latestTestEntry = userRisk.Select(x => new { x.AssesmentDate, x.TestIdentifier }).OrderByDescending(x => x.AssesmentDate).FirstOrDefault();
                    if (latestTestEntry != null)
                    {
                        result.IsTestTaken = true;
                        result.LastAssessmentDate = latestTestEntry.AssesmentDate;
                        result.LatestTestIdentifier = latestTestEntry.TestIdentifier;
                    }
                    //result.LastTestTakenOn = lastCompetency.RatingDate;
                }
            }
            return result;
        }

    }
}
