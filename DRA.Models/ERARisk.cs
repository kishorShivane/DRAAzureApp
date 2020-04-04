using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERARisk
    {
        public int RiskId { get; set; }
        public string Domain { get; set; }
        public string Category { get; set; }
        public string Risk { get; set; }
    }
}
