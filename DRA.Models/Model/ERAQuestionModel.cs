using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAQuestionModel
    {
        public int QuestionID { get; set; }
        public int RiskID { get; set; }
        public string Question { get; set; }
        public string Comment { get; set; }
    }
}
