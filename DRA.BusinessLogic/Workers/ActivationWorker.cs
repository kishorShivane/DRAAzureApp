using DRA.DataProvider.Models;
using DRA.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class ActivationWorker : BaseWorker
    {
        public ActivationWorker(ILogger log) : base(log)
        {
        }

        public async Task<string> ActivateAccount(int userID)
        {
            string result = "0";
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await Task.Run(()=> userRepo.Get(x => x.UserID == userID).FirstOrDefault());
            if (user != null)
            {
                user.UserActive = 1;
                userRepo.Update(user);
                unitOfWork.Save();
                result = "1";
            }
            else
            {
                result = "-1";
            }
            return result;
        }
    }
}
