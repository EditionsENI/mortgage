using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Optimizer
{
    [Route("api/[controller]")]
    public class JobsController : Controller
    {
        // Faire mieux que des static partagés
        static internal object Verrou = new object();

        static internal List<Job> Jobs = new List<Job>();

        // GET: api/jobs
        [HttpGet]
        public IEnumerable<Job> GetAll()
        {
            return Jobs;
        }

        // PUT: api/jobs/id
        [HttpPut("{id}")]
        public void Put(string Id, [FromBody] Job value)
        {
            lock (Verrou)
            {
                Jobs.RemoveAll(j => j.Id == Id);
                Jobs.Add(value);
            }
        }
    }
}
