using DRA.DataProvider.Helpers;
using DRA.DataProvider.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Workers
{
    public class BaseWorker
    {
        public IUnitOfWork unitOfWork = null;
        public ILogger log = null;
        public BaseWorker(ILogger logger)
        {
            unitOfWork = new UnitOfWork();
            log = logger;
        }
    }
}
