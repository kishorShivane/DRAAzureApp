using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAUserRiskReadRequest
    {
        public int RiskID { get; set; } = 0;
        public int UserID { get; set; } = 0;
        public Guid? TestIdentifier { get; set; }
    }
}
