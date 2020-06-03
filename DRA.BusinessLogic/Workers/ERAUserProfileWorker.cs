using DRA.BusinessLogic.Mapper;
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
    public class ERAUserProfileWorker : BaseWorker
    {
        public ERAUserProfileWorker(ILogger log) : base(log)
        {
        }

        public async Task<Tuple<string, ERAUserModel, int>> GetUserProfile(ERAUserModel userModel)
        {
            ERAUserModel result = null;
            int status = 0;
            var message = "";
            var userRepo = unitOfWork.GetRepository<OnlineAssessmentUser>();
            if (userModel != null && userModel.UserId > 0)
            {
                var user = await Task.Run(() => userRepo.Get(x => x.UserId == userModel.UserId).FirstOrDefault());
                if (user == null)
                {
                    status = -1;
                    message = "User does not exist!!";
                    result = userModel;
                }
                else
                {
                    result = DataToDomain.MapUserToERAUserModel(user);
                    if (result != null)
                    {
                        status = 1;
                        message = "User profile found!!";
                    }
                }
            }
            return new Tuple<string, ERAUserModel, int>(message, result, status);
        }

        public async Task<Tuple<string, ERAUserModel, int>> UpdateUserProfile(ERAUserModel userModel)
        {
            ERAUserModel result = null;
            int status = 0;
            var message = "";
            var userRepo = unitOfWork.GetRepository<OnlineAssessmentUser>();
            if (userModel != null && userModel.UserId > 0)
            {
                var user = await Task.Run(() => userRepo.Get(x => x.UserId == userModel.UserId).FirstOrDefault());
                if (user == null)
                {
                    status = -1;
                    message = "User does not exist!!";
                    result = userModel;
                }
                else
                {
                    var userEmail = await Task.Run(() => userRepo.Get(x => x.UserId != userModel.UserId && x.Email == userModel.Email).FirstOrDefault());
                    if (userEmail == null)
                    {
                        user.CompanyName = userModel.CompanyName;
                        user.Email = userModel.Email;
                        user.EmployeeNumber = userModel.EmployeeNumber;
                        user.FirstName = userModel.FirstName;
                        user.LastName = userModel.LastName;
                        user.Password = userModel.Password;

                        userRepo.Update(user);
                        unitOfWork.Save();
                        result = DataToDomain.MapUserToERAUserModel(user);
                        if (result != null)
                        {
                            status = 1;
                            message = "User profile updated successfully!!";
                        }
                    }
                    else
                    {
                        status = -2;
                        message = "Email trying to update already exist.!!";
                    }
                }
            }
            log.LogDebug(message);
            return new Tuple<string, ERAUserModel, int>(message, result, status);
        }
    }
}
