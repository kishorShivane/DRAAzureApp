//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DRA.DataProvider.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserCompetencyMatrix
    {
        public int ID { get; set; }
        public byte UserID { get; set; }
        public string Type { get; set; }
        public string MainGroup { get; set; }
        public string SubGroup { get; set; }
        public string Competency { get; set; }
        public byte LoW { get; set; }
        public int RequiredLevel { get; set; }
        public int CurrentLevel { get; set; }
        public System.DateTime RatingDate { get; set; }
        public int Gap { get; set; }
    }
}
