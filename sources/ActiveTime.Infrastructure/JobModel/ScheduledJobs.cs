using System;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public class ScheduledJobs
    {
        private readonly HashSet<IJob> jobs = new HashSet<IJob>();

        public IJob this[string jobId]
        {
            get
            {
                if (jobId == null) throw new ArgumentNullException(nameof(jobId));

                return GetInternal(jobId);
            }
        }

        public void Add(IJob job)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));

            jobs.Add(job);
        }

        public IJob Get(string jobId)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));

            return GetInternal(jobId);
        }

        public JobState GetState(string jobId)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));

            IJob job = GetInternal(jobId);
            return job.State;
        }

        public void Start(string jobId)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));

            IJob job = GetInternal(jobId);
            job.Start();
        }

        public void Stop(string jobId)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));

            IJob job = GetInternal(jobId);
            job.Stop();
        }

        private IJob GetInternal(string jobId)
        {
            try
            {
                return jobs.First(x => x.Id == jobId);
            }
            catch (Exception ex)
            {
                throw new Exception($"There is no job with the id {jobId}.", ex);
            }
        }
    }
}