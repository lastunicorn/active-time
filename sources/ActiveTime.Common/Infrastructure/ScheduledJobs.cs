using System;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.ActiveTime.Common.Infrastructure
{
    public class ScheduledJobs
    {
        private readonly HashSet<IJob> jobs = new HashSet<IJob>();

        public void Add(IJob job)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));

            jobs.Add(job);
        }

        public IJob Get(string jobId)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));

            return jobs.FirstOrDefault(x => x.Id == jobId);
        }
    }
}