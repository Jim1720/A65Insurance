using System;
using System.Collections.Generic;

#nullable disable

namespace A65Insurance.Models
{
    public partial class Service
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimTypeLiteral { get; set; }
        public decimal Cost { get; set; }
    }
}
