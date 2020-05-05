using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAUserRiskModel
    {
        public int UserRisksID { get; set; }
        public int UserID { get; set; }
        public System.DateTime AssesmentDate { get; set; }
        public int RiskID { get; set; }
        public double Score { get; set; }
        public int RiskValue { get; set; }
        public string Risk { get; set; }
        public Guid TestIdentifier { get; set; }
    }
}
