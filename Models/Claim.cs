using System;
using System.Collections.Generic;

#nullable disable

namespace A65Insurance.Models
{
    public partial class Claim
    {
        public int Id { get; set; }
        public string ClaimIdNumber { get; set; }
        public string ClaimDescription { get; set; }
        public string CustomerId { get; set; }
        public string PlanId { get; set; }
        public string PatientFirst { get; set; }
        public string PatientLast { get; set; }
        public string Diagnosis1 { get; set; }
        public string Diagnosis2 { get; set; }
        public string Procedure1 { get; set; }
        public string Procedure2 { get; set; }
        public string Procedure3 { get; set; }
        public string Physician { get; set; }
        public string Clinic { get; set; }
        public DateTime? DateService { get; set; }
        public string Service { get; set; }
        public string Location { get; set; }
        public decimal TotalCharge { get; set; }
        public decimal CoveredAmount { get; set; }
        public decimal BalanceOwed { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? DateAdded { get; set; }
        public string AdjustedClaimId { get; set; }
        public string AdjustingClaimId { get; set; }
        public DateTime? AdjustedDate { get; set; }
        public string AppAdjusting { get; set; }
        public string ClaimStatus { get; set; }
        public string Referral { get; set; }
        public string PaymentAction { get; set; }
        public string ClaimType { get; set; }
        public DateTime? DateConfine { get; set; }
        public DateTime? DateRelease { get; set; }
        public int ToothNumber { get; set; }
        public string DrugName { get; set; }
        public string Eyeware { get; set; }
    }
}
