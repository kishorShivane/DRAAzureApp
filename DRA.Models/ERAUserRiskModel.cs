using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAUserRiskModel
    {
        public int UserRisksId { get; set; }
        public int UserId { get; set; }
        public System.DateTime AssesmentDate { get; set; }
        public int RiskId { get; set; }
        public int Score { get; set; }
        public int RiskValue { get; set; }
        public string Risk { get; set; }
    }
}
