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
    public class ERARiskWorker : BaseWorker
    {
        public ERARiskWorker(ILogger log) : base(log)
        { }

        public async Task<ERARiskResponse> GetRisks(ERARiskRequest request)
        {
            ERARiskResponse response = null;
            List<ERARiskModel> risksModel = null;
            var riskRepo = unitOfWork.GetRepository<Risk>();
            var risks = await Task.Run(() => riskRepo.Get(x => (x.RiskId == request.RiskID || request.RiskID == 0)).ToList());
            if (risks != null && risks.Any())
            {
                risksModel = DataToDomain.MapRisksToERARisks(risks);
                response = new ERARiskResponse() { Risks = risksModel, TotalRecords = risksModel.Count() };
            }
            return response;
        }
    }
}
