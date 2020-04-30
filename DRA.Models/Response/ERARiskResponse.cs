using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERARiskResponse
    {
        public List<ERARiskModel> Risks { get; set; }
        public int TotalRecords { get; set; }
    }
}
