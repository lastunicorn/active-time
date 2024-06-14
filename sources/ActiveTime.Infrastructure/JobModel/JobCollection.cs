// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public class JobCollection
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

        public void StartAll()
        {
            foreach (IJob job in jobs) 
                job.Start();
        }
    }
}