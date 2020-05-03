using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAReportRequest
    {
        public int UserID { get; set; }
        public string Email { get; set; }
    }
}
