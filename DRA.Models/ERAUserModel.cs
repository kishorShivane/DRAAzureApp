using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.Models
{
    public class ERAUserModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public System.DateTime RegisteredDate { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public bool IsTestTaken { get; set; }
    }
}
