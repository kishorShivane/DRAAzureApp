using DRA.BusinessLogic.Mapper;
using DRA.DataProvider.Models;
using DRA.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class LoginWorker : BaseWorker
    {
        public LoginWorker(ILogger log) : base(log)
        {
        }

        public async Task<UserModel> ValidateUserCredential(UserModel userModel)
        {
            UserModel result = null;
            var userRepo = unitOfWork.GetRepository<User>();
            var userCompRepo = unitOfWork.GetRepository<UserCompetencyMatrix>();
            var user = await Task.Run(() => userRepo.Get(x => x.UserEmail == userModel.UserEmail && x.UserPassword == userModel.UserPassword).FirstOrDefault());
            if (user != null)
            {
                result = DataToDomain.MapUserToUserModel(user);
                var lastCompetency = userCompRepo.Get(x=>x.UserID == user.UserID).OrderByDescending(x=>x.RatingDate).FirstOrDefault();
                if (lastCompetency != null)
                {
                    result.IsTestTaken = true;
                    result.LastTestTakenOn = lastCompetency.RatingDate;
                }
            }
            return result;
        }

    }
}
