using DRA.DataProvider.Models;
using DRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Mapper
{
    public class DataToDomain
    {
        public static UserModel MapUserToUserModel(User user)
        {
            return new UserModel()
            {
                BusinessFunction = user.BusinessFunction,
                ID = user.ID,
                Industry = user.Industry,
                JobID = user.JobID,
                Organization = user.Organization,
                UserActive = user.UserActive,
                UserEmail = user.UserEmail,
                UserID = user.UserID,
                UserName = user.UserName,
                UserPassword = user.UserPassword,
                UserSurname = user.UserSurname
            };
        }

        public static User MapUserModelToUser(UserModel user)
        {
            return new User()
            {
                BusinessFunction = user.BusinessFunction,
                ID = user.ID,
                Industry = user.Industry,
                JobID = user.JobID,
                Organization = user.Organization,
                UserActive = user.UserActive,
                UserEmail = user.UserEmail,
                UserID = user.UserID,
                UserName = user.UserName,
                UserPassword = user.UserPassword,
                UserSurname = user.UserSurname
            };
        }

    }
}
