using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ResponseMessage<T>
    {
        public string Message { get; set; }
        public T Content { get; set; }
    }
}
