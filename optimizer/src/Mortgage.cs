using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optimizer
{
    public class Mortgage
    {
        public int startMonth { get; set; }
        public int repaymentLengthInMonths { get; set; }
        public decimal amountBorrowed { get; set; }
        public bool isRateFixed { get; set; }
        public decimal fixRate { get; set; }
    }
}
