using System;
using System.Threading.Tasks;

namespace DustInTheWind.ActiveTime.Infrastructure.JobModel
{
    public abstract class JobBase : IJob, IDisposable
    {
        private bool isDisposed;
        private readonly object stateSynchronizer = new object();
        private readonly ITimer timer;

        public abstract string Id { get; }

        /// <summary>
        /// Gets the state of the current recorder job.
        /// </summary>
        public JobState State { get; private set; }

        /// <summary>
        /// Gets or sets the interval of time to wait between two stamping actions.
        /// A stamp consists in updating the time record in the persistence layer (usually the database).
        /// </summary>
        public TimeSpan RunInterval
        {
            get => timer.Interval;
            set
            {
                lock (stateSynchronizer)
                {
                    timer.Interval = value;
                }
            }
        }

        protected JobBase(ITimer timer)
        {
            this.timer = timer ?? throw new ArgumentNullException(nameof(timer));

            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += HandleTimerTick;
        }

        private void HandleTimerTick(object sender, EventArgs e)
        {
            _ = Execute();
        }

        public void Start()
        {
            switch (State)
            {
                case JobState.Stopped:
                    _ = DoStart();
                    break;

                case JobState.Running:
                    break;
            }
        }

        private async Task DoStart()
        {
            lock (stateSynchronizer)
            {
                timer.Start();
                State = JobState.Running;
            }

            await Execute();
        }

        public void Stop()
        {
            switch (State)
            {
                case JobState.Stopped:
                    break;

                case JobState.Running:
                    DoStop();
                    break;
            }
        }

        private void DoStop()
        {
            lock (stateSynchronizer)
            {
                timer.Stop();
                State = JobState.Stopped;
            }
        }

        protected abstract Task Execute();

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
                timer.Dispose();
            }

            isDisposed = true;
        }

        ~JobBase()
        {
            Dispose(false);
        }
    }
}