using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAUserRiskResponse
    {
        public List<ERAUserRiskModel> UserRisks { get; set; }
        public int TotalRecords { get; set; }
    }
}
