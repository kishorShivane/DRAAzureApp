using System;
using System.Collections.Generic;
using System.Text;

namespace DRA.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string ID { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public byte UserActive { get; set; }
        public string Industry { get; set; }
        public string Organization { get; set; }
        public string BusinessFunction { get; set; }
        public string JobTitle { get; set; }
        public byte JobID { get; set; }
        public bool IsTestTaken { get; set; }
        public DateTime LastTestTakenOn { get; set; }
    }
}
