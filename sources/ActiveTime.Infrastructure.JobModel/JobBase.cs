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

public abstract class JobBase : IJob
{
    protected readonly object StateSynchronizer = new();

    public abstract string Id { get; }

    public JobState State { get; private set; }

    public async Task Start()
    {
        if (State == JobState.Stopped)
        {
            await OnStarting();

            lock (StateSynchronizer)
            {
                DoStart();
                State = JobState.Running;
            }

            await OnStarted();
        }
    }

    protected virtual Task OnStarting()
    {
        return Task.CompletedTask;
    }

    protected virtual void DoStart()
    {
    }

    protected virtual Task OnStarted()
    {
        return Task.CompletedTask;
    }

    public async Task Stop()
    {
        if (State == JobState.Running)
        {
            await OnStopping();

            lock (StateSynchronizer)
            {
                DoStop();
                State = JobState.Stopped;
            }

            await OnStopped();
        }
    }

    protected virtual Task OnStopped()
    {
        return Task.CompletedTask;
    }

    protected virtual void DoStop()
    {
    }

    protected virtual Task OnStopping()
    {
        return Task.CompletedTask;
    }
}