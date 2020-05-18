using DRA.BusinessLogic.Mapper;
using DRA.DataProvider.Models;
using DRA.Models;
using DRA.Models.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class ERAUserAnswerWorker : BaseWorker
    {
        public ERAUserAnswerWorker(ILogger log) : base(log)
        { }

        public async Task<ERAUserAnswerResponse> GetUserAnswers(ERAUserAnswerReadRequest request)
        {
            ERAUserAnswerResponse response = null;
            List<ERAUserAnswerModel> answersModel = null;
            var answersRepo = unitOfWork.GetRepository<UserAnswer>();
            var answers = await Task.Run(() => answersRepo.Get(x => (x.RiskId == request.RiskID || request.RiskID == 0) &&
                                                                (x.QuestionId == request.QuestionID || request.QuestionID == 0) &&
                                                                (x.UserId == request.UserID || request.UserID == 0) &&
                                                                (x.TestIdentifier == request.TestIdentifier || request.TestIdentifier == null), includeProperties: "UserImages").ToList());
            if (answers != null && answers.Any())
            {
                answersModel = DataToDomain.MapUserAnswersToERAUserAnswers(answers);
                response = new ERAUserAnswerResponse() { Answers = answersModel, TotalRecords = answersModel.Count() };
            }
            return response;
        }

        public async Task<ERAUserAnswerResponse> InsertUserAnswers(ERAUserAnswerWriteRequest request)
        {
            ERAUserAnswerResponse response = null;
            List<UserAnswer> userAnswers = null;
            List<ERAUserAnswerModel> answersModel = null;
            if (request != null && request.Answers.Any())
            {
                var answersRepo = unitOfWork.GetRepository<UserAnswer>();
                userAnswers = DataToDomain.MapERAUserAnswersToUserAnswers(request.Answers);
                await Task.Run(() =>
                {
                    try
                    {
                        answersRepo.InsertAll(userAnswers);
                        unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
            }

            if (userAnswers != null && userAnswers.Any())
            {
                foreach (var answer in request.Answers)
                {
                    if (answer.UserImages != null && answer.UserImages.Any())
                    {
                        answer.UserImages.ForEach(x => x.UserAnswerID = userAnswers.FirstOrDefault(z => z.QuestionId == answer.QuestionID).UserAnswerId);
                        var userImages = InsertUserImages(answer.UserImages);
                        userAnswers.Where(x => x.QuestionId == answer.QuestionID).Select(x => { x.UserImages = userImages; return x; }).ToList();
                    }
                }
                answersModel = DataToDomain.MapUserAnswersToERAUserAnswers(userAnswers);
                response = new ERAUserAnswerResponse() { Answers = answersModel, TotalRecords = answersModel.Count() };
            }
            return response;
        }

        private List<UserImage> InsertUserImages(List<ERAUserImageModel> userImages)
        {
            var insertMe = new List<UserImage>();
            if (userImages.Any())
            {
                var repo = unitOfWork.GetRepository<UserImage>();
                insertMe = DataToDomain.MapERAUserImagesToUserImages(userImages);
                repo.InsertAll(insertMe);
                unitOfWork.Save();
                userImages = DataToDomain.MapUserImagesToERAUserImages(insertMe);
            }
            return insertMe;
        }
    }
}
