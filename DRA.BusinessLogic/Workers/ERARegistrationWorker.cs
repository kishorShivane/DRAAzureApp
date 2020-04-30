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
    public class ERARegistrationWorker : BaseWorker
    {
        public ERARegistrationWorker(ILogger log) : base(log)
        {
        }

        public async Task<Tuple<string, ERAUserModel, int>> ValidateUserAndRegister(ERAUserModel userModel)
        {
            ERAUserModel result = null;
            int status = 0;
            var message = "";
            var userRepo = unitOfWork.GetRepository<OnlineAssessmentUser>();
            var user = await Task.Run(() => userRepo.Get(x => x.UserId == userModel.UserId).FirstOrDefault());
            if (user != null)
            {
                var userEmail = await Task.Run(() => userRepo.Get(x => x.Email == userModel.Email).FirstOrDefault());
                if (userEmail != null)
                {
                    status = -1;
                    message = "User already exist!!";
                    result = DataToDomain.MapUserToERAUserModel(userEmail);
                    result.UserType = userModel.UserType;
                }
                else
                {
                    result = RegisterNewUser(userModel);
                    if (result != null)
                    {
                        status = 1;
                        message = "User registered successfully!!";
                    }
                    else
                    {
                        message = "No user type found with user type name:" + userModel.UserType;
                    }
                }
            }
            else
            {
                var userEmail = await Task.Run(() => userRepo.Get(x => x.Email == userModel.Email).FirstOrDefault());
                if (userEmail != null)
                {
                    status = -1;
                    message = "User already exist!!";
                    result = DataToDomain.MapUserToERAUserModel(userEmail);
                    result.UserType = userModel.UserType;
                }
                else
                {
                    result = RegisterNewUser(userModel);
                    if (result != null)
                    {
                        status = 1;
                        message = "User registered successfully!!";
                    }
                    else
                    {
                        message = "No user type found with user type name:" + userModel.UserType;
                    }
                }

            }
            return new Tuple<string, ERAUserModel, int>(message, result, status);
        }

        private ERAUserModel RegisterNewUser(ERAUserModel userModel)
        {
            ERAUserModel user = null;
            var userRepo = unitOfWork.GetRepository<OnlineAssessmentUser>();
            var userTypeID = (int)userModel.UserTypeId;
            if (userTypeID == 0 && !string.IsNullOrEmpty(userModel.UserType))
            {
                userTypeID = GetUserTypeIDByType(userModel.UserType);
            }
            else
            {
                userModel.UserType = GetUserTypeByID(userTypeID);
            }

            if (userTypeID != -1)
            {
                userModel.UserTypeId = Convert.ToByte(userTypeID);
                var newUser = DataToDomain.MapERAUserModelToUser(userModel);
                userRepo.Insert(newUser);
                unitOfWork.Save();
                user = DataToDomain.MapUserToERAUserModel(newUser);
                user.UserType = userModel.UserType;
            }
            return user;
        }

        private int GetUserTypeIDByType(string userTypeName)
        {
            var userTypeRepo = unitOfWork.GetRepository<UserType>();
            var userType = userTypeRepo.Get(x => x.UserTypeName == userTypeName).FirstOrDefault();
            if (userType != null)
            {
                return userType.UserTypeId;
            }
            log.LogError("No user type found with user type name:" + userTypeName);
            return -1;
        }

        private string GetUserTypeByID(int userTypeID)
        {
            var userTypeRepo = unitOfWork.GetRepository<UserType>();
            var userType = userTypeRepo.Get(x => x.UserTypeId == userTypeID).FirstOrDefault();
            if (userType != null)
            {
                return userType.UserTypeName;
            }
            log.LogError("No user type found with ID:" + userTypeID);
            return "";
        }
    }
}
