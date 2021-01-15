using System;
using System.Collections.Generic;

#nullable disable

namespace A65Insurance.Models
{
    public partial class Plan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public string PlanLiteral { get; set; }
        public string Percent { get; set; }
    }
}
