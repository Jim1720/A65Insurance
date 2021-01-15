using System;
using System.Collections.Generic;

#nullable disable

namespace A65Insurance.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string CustId { get; set; }
        public string CustPassword { get; set; }
        public string Encrypted { get; set; }
        public string CustFirst { get; set; }
        public string CustMiddle { get; set; }
        public string CustLast { get; set; }
        public string CustEmail { get; set; }
        public DateTime? CustBirthDate { get; set; }
        public string CustGender { get; set; }
        public string CustPhone { get; set; }
        public string CustAddr1 { get; set; }
        public string CustAddr2 { get; set; }
        public string CustCity { get; set; }
        public string CustState { get; set; }
        public string CustZip { get; set; }
        public string CustPlan { get; set; }
        public string PromotionCode { get; set; }
        public string AppId { get; set; }
        public string ExtendColors { get; set; }
        public string ClaimCount { get; set; }
    }
}
