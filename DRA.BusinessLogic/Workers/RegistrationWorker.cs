using DRA.Models;
using DRA.DataProvider.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using DRA.BusinessLogic.Utility;
using System;
using DRA.BusinessLogic.Mapper;

namespace DRA.BusinessLogic.Workers
{
    public class RegistrationWorker : BaseWorker
    {
        public RegistrationWorker(ILogger log) : base(log)
        {
        }

        public async Task<Tuple<string, UserModel, int>> ValidateUserAndRegister(UserModel userModel)
        {
            UserModel result = null;
            int status = 0;
            var message = "";
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await Task.Run(() => userRepo.Get(x => x.ID == userModel.ID).FirstOrDefault());
            if (user != null)
            {
                var userEmail = await Task.Run(() => userRepo.Get(x => x.UserEmail == userModel.UserEmail).FirstOrDefault());
                if (userEmail != null)
                {
                    status = -1;
                    message = "User already exist!!";
                    result = DataToDomain.MapUserToUserModel(userEmail);
                    result.JobTitle = userModel.JobTitle;
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
                        message = "No job found with title:" + userModel.JobTitle;
                    }
                }
            }
            else
            {
                var userEmail = await Task.Run(() => userRepo.Get(x => x.UserEmail == userModel.UserEmail).FirstOrDefault());
                if (userEmail != null)
                {
                    status = -1;
                    message = "User already exist!!";
                    result = DataToDomain.MapUserToUserModel(userEmail);
                    result.JobTitle = userModel.JobTitle;
                }
                else
                {
                    if (userModel.ID.Length >= 13)
                    {
                        var identityHelper = new IdentityHelper(userModel.ID);
                        if (identityHelper.IsValid)
                        {
                            result = RegisterNewUser(userModel);
                            if (result != null)
                            {
                                status = 1;
                                message = "User registered successfully!!";
                            }
                            else
                            {
                                message = "No job found with title:" + userModel.JobTitle;
                            }
                        }
                        else
                        {
                            status = -2;
                            message = "ID is Invalid!!";
                            result = userModel;
                        }
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
                            message = "No job found with title:" + userModel.JobTitle;
                        }
                    }
                }

            }
            return new Tuple<string, UserModel, int>(message, result, status);
        }

        private UserModel RegisterNewUser(UserModel userModel)
        {
            UserModel user = null;
            var userRepo = unitOfWork.GetRepository<User>();
            var jobID = (int)userModel.JobID;
            if (jobID == 0 && !string.IsNullOrEmpty(userModel.JobTitle))
            {
                jobID = GetJobIDByTitle(userModel.JobTitle);
            }
            else
            {
                userModel.JobTitle = GetJobByID(jobID);
            }
                
            if (jobID != -1)
            {
                userModel.JobID = Convert.ToByte(jobID);
                var newUser = DataToDomain.MapUserModelToUser(userModel);
                //newUser.UserActive = 1;
                userRepo.Insert(newUser);
                unitOfWork.Save();
                user = DataToDomain.MapUserToUserModel(newUser);
                user.JobTitle = userModel.JobTitle;
            }
            return user;
        }

        private int GetJobIDByTitle(string jobTitle)
        {
            var jobRepo = unitOfWork.GetRepository<Job>();
            var job = jobRepo.Get(x => x.JobTitle == jobTitle).FirstOrDefault();
            if (job != null)
            {
                return job.JobID;
            }
            log.LogError("No job found with title:" + jobTitle);
            return -1;
        }

        private string GetJobByID(int jobID)
        {
            var jobRepo = unitOfWork.GetRepository<Job>();
            var job = jobRepo.Get(x => x.JobID == jobID).FirstOrDefault();
            if (job != null)
            {
                return job.JobTitle;
            }
            log.LogError("No job found with ID:" + jobID);
            return "";
        }
    }
}
