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
    public class ERAUserRiskWorker : BaseWorker
    {
        public ERAUserRiskWorker(ILogger log) : base(log)
        { }

        public async Task<ERAUserRiskResponse> GetUserRisks(ERAUserRiskReadRequest request)
        {
            ERAUserRiskResponse response = null;
            List<ERAUserRiskModel> userRisksModel = null;
            var userRiskRepo = unitOfWork.GetRepository<UserRisk>();
            var userRisks = await Task.Run(() => userRiskRepo.Get(x => (x.RiskId == request.RiskID || request.RiskID == 0) &&
                                                                (x.UserId == request.UserID || request.UserID == 0)).ToList());
            if (userRisks != null && userRisks.Any())
            {
                userRisksModel = DataToDomain.MapUserRisksToERAUserRisks(userRisks);
                response = new ERAUserRiskResponse() { UserRisks = userRisksModel, TotalRecords = userRisksModel.Count() };
            }
            return response;
        }

        public async Task<ERAUserRiskResponse> InsertUserRisks(ERAUserRiskWriteRequest request)
        {
            ERAUserRiskResponse response = null;
            List<UserRisk> userRisks = null;
            List<ERAUserRiskModel> userRisksModel = null;
            if (request != null && request.UserRisks.Any())
            {
                var userRiskRepo = unitOfWork.GetRepository<UserRisk>();
                userRisks = DataToDomain.MapERAUserRisksToUserRisks(request.UserRisks);
                await Task.Run(() =>
                {
                    try
                    {
                        userRiskRepo.InsertAll(userRisks);
                        unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
            }

            if (userRisks != null && userRisks.Any())
            {
                userRisksModel = DataToDomain.MapUserRisksToERAUserRisks(userRisks);
                response = new ERAUserRiskResponse() { UserRisks = userRisksModel, TotalRecords = userRisksModel.Count() };
            }
            return response;
        }
    }
}
