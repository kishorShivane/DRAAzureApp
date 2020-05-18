using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class QuestionnaireRequest
    {
        public int UserID { get; set; }
        public string Competency { get; set; }
        public string Points { get; set; }
    }
}
