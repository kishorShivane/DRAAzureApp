using DRA.DataProvider.Models;
using DRA.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class ResetPasswordWorker : BaseWorker
    {
        public ResetPasswordWorker(ILogger log) : base(log)
        {
        }

        public async Task<string> ResetPassword(ResetPassword resetPwd)
        {
            string result = "0";
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await Task.Run(()=> userRepo.Get(x => x.UserEmail.Trim() == resetPwd.Email.Trim()).FirstOrDefault());
            if (user != null)
            {
                user.UserPassword = resetPwd.Password;
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
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await Task.Run(() => userRepo.Get(x => x.UserEmail == email).FirstOrDefault());
            if (user != null)
            {
                result = "1";
                user.UserActive = 1;
                userRepo.Update(user);
                unitOfWork.Save();
            }
            else
            {
                result = "-1";
            }
            return result;
        }
    }
}
