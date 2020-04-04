using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAQuestion
    {
        public int QuestionId { get; set; }
        public int RiskId { get; set; }
        public string Question { get; set; }
        public string Comment { get; set; }
    }
}
