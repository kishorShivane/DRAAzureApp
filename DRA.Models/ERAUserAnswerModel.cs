using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAUserAnswerModel
    {
        public int UserAnswerId { get; set; }
        public int UserId { get; set; }
        public System.DateTime AssesmentDate { get; set; }
        public int RiskId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }
}
