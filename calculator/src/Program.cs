using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Framework.ConfigurationModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Runtime.Serialization.Json;

namespace Calculator
{
    // Pour commencer facile, on duplique les classes
    public class Mortgage
    {
        public int startMonth { get; set; }
        public int repaymentLengthInMonths { get; set; }
        public decimal amountBorrowed { get; set; }
        public bool isRateFixed { get; set; }
        public decimal fixRate { get; set; }
    }

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

    public class Program
    {
        public static void Main(string[] args)
        {
            //var config = new Configuration().AddEnvironmentVariables();
            //string urlOptimizer = config.Get("URLOptimizerJobs") ?? "http://localhost:5004/api/Jobs/";
            string urlOptimizer = Environment.GetEnvironmentVariable("URLOptimizerJobs") ?? "http://optimizer/api/Jobs/";
            Console.WriteLine("Mortgage calculation service listening to {0}", urlOptimizer);

            bool WarnNoMoreJobs = true;
            while (true)
            {
                try
                {
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    List<Job> jobs = httpClient.GetAsync(urlOptimizer).Result.Content.ReadAsAsync<List<Job>>().Result;

                    List<Job> remainingJobs = jobs.FindAll(j => !j.Taken);
                    if (remainingJobs.Count == 0)
                    {
                        if (WarnNoMoreJobs)
                        {
                            Console.WriteLine("No more jobs !");
                            WarnNoMoreJobs = false;
                        }
                        Task.Delay(1000).Wait();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("{0} job(s) remaining", remainingJobs.Count);
                        WarnNoMoreJobs = true;
                    }
                    Random engine = new Random(DateTime.Now.Millisecond);
                    Job Taken = remainingJobs[engine.Next(remainingJobs.Count)];
                    Taken.Taken = true;
                    httpClient.PutAsJsonAsync<Job>(urlOptimizer + Taken.Id, Taken).Result.EnsureSuccessStatusCode();

                    // The calculation is completely simulated, and does not correspond to any real financial computing
                    // We thus only wait for a given delay and send a random amount for the total cost
                    // Should one be interested in the kind of computation that can truly use scaling, one can take a look
                    // at the use of Genetic Algorithms as shown as in https://github.com/jp-gouigoux/PORCAGEN
                    Task.Delay(20).Wait();
                    Taken.Done = true;
                    Taken.TotalMortgageCost = Convert.ToDecimal(engine.Next(1000));

                    httpClient.PutAsJsonAsync<Job>(urlOptimizer + Taken.Id, Taken).Result.EnsureSuccessStatusCode();
                }
                catch
                {
                    Task.Delay(1000).Wait();
                }
            }
        }
    }
}
