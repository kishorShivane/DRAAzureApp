using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAUserAnswerReadRequest
    {
        public int RiskID { get; set; } = 0;
        public int QuestionID { get; set; } = 0;
        public int UserID { get; set; } = 0;
        public Guid? TestIdentifier { get; set; }
    }
}
