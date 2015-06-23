using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;

namespace Optimizer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();

//            JobsController.Jobs = new List<Job>
//            {
//                new Job { Id = "a", RepaymentAmount = 1000, Done = false, MortgageDefinition = new Mortgage { AmountBorrowed = 1000, FixRate = 0.05M, IsRateFixed = true, RepaymentLenghtInMonths = 240, StartMonth = 3 } },
//                new Job { Id = "b", RepaymentAmount = 300, Done = false, MortgageDefinition = new Mortgage { AmountBorrowed = 1000, IsRateFixed = false, RepaymentLenghtInMonths = 120, StartMonth = 10 } },
//            };
        }
    }
}
