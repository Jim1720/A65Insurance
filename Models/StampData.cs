using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A65Insurance.Models
{
    public class StampData
    {
        public string AdjustedClaimId { get; set; }
        public string AdjustingClaimId { get; set; }
        public DateTime DateAdjusted { get; set; } 
        public string AppAdjusting { get; set; }
    }
}
