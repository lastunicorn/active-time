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
using System.Threading.Tasks;

namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public abstract class JobBase : IJob, IDisposable
    {
        private bool isDisposed;
        protected readonly object StateSynchronizer = new object();
        protected readonly ITimer Timer;

        public abstract string Id { get; }

        /// <summary>
        /// Gets the state of the current recorder job.
        /// </summary>
        public JobState State { get; private set; }

        protected JobBase(ITimer timer)
        {
            Timer = timer ?? throw new ArgumentNullException(nameof(timer));

            timer.Tick += HandleTimerTick;
        }

        private void HandleTimerTick(object sender, EventArgs e)
        {
            _ = Execute();
        }

        public async Task Start()
        {
            if (State == JobState.Stopped)
            {
                DoStart();
                await OnStarted();
            }
        }

        protected virtual Task OnStarted()
        {
            return Task.CompletedTask;
        }

        private void DoStart()
        {
            lock (StateSynchronizer)
            {
                Timer.Start();
                State = JobState.Running;
            }
        }

        public Task Stop()
        {
            if (State == JobState.Running)
                DoStop();

            return Task.CompletedTask;
        }

        private void DoStop()
        {
            lock (StateSynchronizer)
            {
                Timer.Stop();
                State = JobState.Stopped;
            }
        }

        protected async Task Execute()
        {
            await OnExecuting();
            await DoExecute();
            await OnExecuted();
        }

        protected virtual Task OnExecuting()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnExecuted()
        {
            return Task.CompletedTask;
        }

        protected abstract Task DoExecute();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                Timer.Dispose();
            }

            isDisposed = true;
        }

        ~JobBase()
        {
            Dispose(false);
        }
    }
}