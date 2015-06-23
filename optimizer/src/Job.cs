using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optimizer
{
    public class Job
    {
        public string Id { get; set; }
        public int RepaymentMonth { get; set; }
        public decimal RepaymentAmount { get; set; }
        public decimal ReplacementRate { get; set; }
        public Mortgage MortgageDefinition { get; set; }
        public bool Taken { get; set; }
        public bool Done { get; set; }
        public decimal TotalMortgageCost { get; set; }
    }
}
