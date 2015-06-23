using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Threading;

namespace Optimizer
{
    [Route("api/[controller]")]
    public class MortgageController : Controller
    {
        [HttpPost("BestRepaymentDate")]
        public DateTime GetBestRepaymentDate(decimal AmountRepaid, decimal ReplacementFixRate, [FromBody] Mortgage m)
        {
            lock (JobsController.Verrou)
            {
                foreach (int IndexMonth in Enumerable.Range(
                    GetIndexCurrentMonth(DateTime.Now),
                    m.startMonth + m.repaymentLengthInMonths - GetIndexCurrentMonth(DateTime.Now) + 1))
                {
                    JobsController.Jobs.Add(new Job
                    {
                        Id = Guid.NewGuid().ToString(),
                        MortgageDefinition = m,
                        Done = false,
                        Taken = false,
                        RepaymentAmount = AmountRepaid,
                        RepaymentMonth = IndexMonth,
                        ReplacementRate = ReplacementFixRate,
                    });
                }
            }

            Console.WriteLine("{0} jobs created", JobsController.Jobs.Count);
            DateTime debut = DateTime.Now;
            while (DateTime.Now < debut.AddMinutes(1))
            {
                lock (JobsController.Verrou)
                {
                    if (JobsController.Jobs.TrueForAll(j => j.Done))
                        break;
                }
                Thread.Sleep(1000);
            }

            decimal TotalAmount = decimal.MaxValue;
            int BestMonth = 0;
            lock (JobsController.Verrou)
            {
                foreach (Job j in JobsController.Jobs)
                {
                    if (!j.Done) return DateTime.MinValue;
                    if (j.TotalMortgageCost < TotalAmount)
                    {
                        BestMonth = j.RepaymentMonth;
                        TotalAmount = j.TotalMortgageCost;
                    }
                }
            }

            Console.WriteLine("Index of best repayment month is {0}", BestMonth);
            return GetMonthFromIndex(BestMonth);
        }

        private int GetIndexCurrentMonth(DateTime t)
        {
            return 12 * (t.Year - 2000) + t.Month - 1;
        }

        private DateTime GetMonthFromIndex(int i)
        {
            return new DateTime(2000 + i / 12, (i % 12) + 1, 1);
        }
    }
}
