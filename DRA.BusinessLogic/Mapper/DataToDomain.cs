﻿using DRA.DataProvider.Models;
using DRA.Models;
using DRA.Models.Model;
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

        public static List<ERAQuestionModel> MapQuestionToERAQuestions(List<Question> questions)
        {
            List<ERAQuestionModel> questionsModel = null;
            if (questions.Any())
            {
                questionsModel = new List<ERAQuestionModel>();
                questions.ForEach(x =>
                {
                    questionsModel.Add(new ERAQuestionModel() { RiskID = x.RiskId, Comment = x.Comment, Question = x.Question1, QuestionID = x.QuestionId });
                });
            }

            return questionsModel;
        }

        public static List<ERARiskModel> MapRisksToERARisks(List<Risk> risks)
        {
            List<ERARiskModel> risksModel = null;
            if (risks.Any())
            {
                risksModel = new List<ERARiskModel>();
                risks.ForEach(x =>
                {
                    risksModel.Add(new ERARiskModel() { RiskID = x.RiskId, Category = x.Category, Domain = x.Domain, Risk = x.Risk1 });
                });
            }

            return risksModel;
        }

        public static List<ERAUserRiskModel> MapUserRisksToERAUserRisks(List<UserRisk> userRisks)
        {
            List<ERAUserRiskModel> userRisksModel = null;
            if (userRisks.Any())
            {
                userRisksModel = new List<ERAUserRiskModel>();
                userRisks.ForEach(x =>
                {
                    userRisksModel.Add(new ERAUserRiskModel() { RiskID = x.RiskId, UserRisksID = x.UserRisksId, Risk = x.Risk, RiskValue = x.RiskValue, AssesmentDate = Convert.ToDateTime(x.AssesmentDate.ToShortDateString()), Score = x.Score, UserID = x.UserId, TestIdentifier = x.TestIdentifier });
                });
            }

            return userRisksModel;
        }

        public static List<UserRisk> MapERAUserRisksToUserRisks(List<ERAUserRiskModel> userRisksModel)
        {
            List<UserRisk> userRisks = null;
            if (userRisksModel.Any())
            {
                userRisks = new List<UserRisk>();
                userRisksModel.ForEach(x =>
                {
                    userRisks.Add(new UserRisk() { RiskId = x.RiskID, UserRisksId = x.UserRisksID, Risk = x.Risk, RiskValue = x.RiskValue, AssesmentDate = Convert.ToDateTime(x.AssesmentDate.ToShortDateString()), Score = x.Score, UserId = x.UserID, TestIdentifier = x.TestIdentifier });
                });
            }
            return userRisks;
        }


        public static List<ERAUserAnswerModel> MapUserAnswersToERAUserAnswers(List<UserAnswer> answers)
        {
            List<ERAUserAnswerModel> answersModel = null;
            if (answers.Any())
            {
                answersModel = new List<ERAUserAnswerModel>();
                answers.ForEach(x =>
                {
                    answersModel.Add(new ERAUserAnswerModel() { RiskID = x.RiskId, QuestionID = x.QuestionId, UserAnswerID = x.UserAnswerId, Answer = x.Answer, AssesmentDate = x.AssesmentDate, Score = x.Score, UserID = x.UserId, TestIdentifier = x.TestIdentifier ,UserImages = MapUserImagesToERAUserImages(x.UserImages.ToList())});
                });
            }

            return answersModel;
        }


        public static List<UserAnswer> MapERAUserAnswersToUserAnswers(List<ERAUserAnswerModel> userAnswers)
        {
            List<UserAnswer> answers = null;
            if (userAnswers.Any())
            {
                answers = new List<UserAnswer>();
                userAnswers.ForEach(x =>
                {
                    answers.Add(new UserAnswer() { RiskId = x.RiskID, QuestionId = x.QuestionID, UserAnswerId = x.UserAnswerID, Answer = x.Answer, AssesmentDate = x.AssesmentDate, Score = x.Score, UserId = x.UserID, TestIdentifier = x.TestIdentifier });
                });
            }

            return answers;
        }

        public static List<ERAUserImageModel> MapUserImagesToERAUserImages(List<UserImage> images)
        {
            List<ERAUserImageModel> imagesModel = null;
            if (images.Any())
            {
                imagesModel = new List<ERAUserImageModel>();
                images.ForEach(x =>
                {
                    imagesModel.Add(new ERAUserImageModel() { UserImageID = x.UserImageID, UserAnswerID = x.UserAnswerID, ImageFileName = x.ImageFileName });
                });
            }
            return imagesModel;
        }


        public static List<UserImage> MapERAUserImagesToUserImages(List<ERAUserImageModel> userImages)
        {
            List<UserImage> images = null;
            if (userImages.Any())
            {
                images = new List<UserImage>();
                userImages.ForEach(x =>
                {
                    images.Add(new UserImage() { UserImageID = x.UserImageID, UserAnswerID = x.UserAnswerID, ImageFileName = x.ImageFileName });
                });
            }
            return images;
        }

        public static ERAUserModel MapUserToERAUserModel(OnlineAssessmentUser user, bool isTestTaken = false, DateTime? assessmentDate = null, Guid? testIdentifier = null)
        {
            return new ERAUserModel()
            {
                Email = user.Email,
                EmployeeNumber = user.EmployeeNumber,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RegisteredDate = user.RegisteredDate,
                CompanyName = user.CompanyName,
                UserId = user.UserId,
                UserTypeId = user.UserTypeId,
                IsTestTaken = isTestTaken,
                LastAssessmentDate = assessmentDate,
                LatestTestIdentifier = testIdentifier
            };
        }

        public static OnlineAssessmentUser MapERAUserModelToUser(ERAUserModel user)
        {
            return new OnlineAssessmentUser()
            {
                Email = user.Email,
                EmployeeNumber = user.EmployeeNumber,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RegisteredDate = user.RegisteredDate,
                CompanyName = user.CompanyName,
                UserId = user.UserId,
                UserTypeId = user.UserTypeId
            };
        }

    }
}
