using DRA.DataProvider.Models;
using DRA.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class ERAResetPasswordWorker : BaseWorker
    {
        public ERAResetPasswordWorker(ILogger log) : base(log)
        {
        }

        public async Task<string> ResetPassword(ResetPassword resetPwd)
        {
            string result = "0";
            var userRepo = unitOfWork.GetRepository<OnlineAssessmentUser>();
            var user = await Task.Run(()=> userRepo.Get(x => x.Email.Trim() == resetPwd.Email.Trim()).FirstOrDefault());
            if (user != null)
            {
                user.Password = resetPwd.Password;
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

        public async Task<string> ValidateEmail(string email)
        {
            string result = "0";
            var userRepo = unitOfWork.GetRepository<OnlineAssessmentUser>();
            var user = await Task.Run(() => userRepo.Get(x => x.Email == email).FirstOrDefault());
            if (user != null)
            {
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
