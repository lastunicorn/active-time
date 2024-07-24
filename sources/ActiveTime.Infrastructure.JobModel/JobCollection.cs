// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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

namespace DustInTheWind.ActiveTime.Infrastructure.JobEngine;

public class JobCollection
{
    private readonly HashSet<IJob> items = new();

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

        items.Add(job);
    }

    public void AddRange(IEnumerable<IJob> jobs)
    {
        if (jobs == null) throw new ArgumentNullException(nameof(jobs));

        IEnumerable<IJob> jobsToAdd = jobs
            .Where(x => x != null);

        foreach (IJob job in jobsToAdd)
            items.Add(job);
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
            return items.First(x => x.Id == jobId);
        }
        catch (Exception ex)
        {
            throw new Exception($"There is no job with the id {jobId}.", ex);
        }
    }

    public void StartAll()
    {
        foreach (IJob job in items)
            job.Start();
    }
}