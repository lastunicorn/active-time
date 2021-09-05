using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.UseCases.Stamp;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Jobs
{
    /// <summary>
    /// Periodically calls the scribe to update the time of the current record in the database.
    /// </summary>
    public class RecorderJob : IJob, IDisposable
    {
        private readonly IMediator mediator;

        private bool isDisposed;
        private readonly object stateSynchronizer = new object();
        private readonly Timer timer;
        private TimeSpan stampingInterval;

        public string Id { get; } = "Recorder";

        /// <summary>
        /// Gets the state of the current recorder job.
        /// </summary>
        public JobState State { get; private set; }

        /// <summary>
        /// Gets or sets the interval of time to wait between two stamping actions.
        /// A stamp consists in updating the time record in the persistence layer (usually the database).
        /// </summary>
        public TimeSpan StampingInterval
        {
            get => stampingInterval;
            set
            {
                lock (stateSynchronizer)
                {
                    stampingInterval = value;

                    if (State == JobState.Running)
                        timer.Change(stampingInterval, stampingInterval);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecorderJob"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public RecorderJob(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            timer = new Timer(HandleTimerTick);
            stampingInterval = TimeSpan.FromMinutes(1);
        }

        private void HandleTimerTick(object o)
        {
            _ = Execute();
        }

        /// <summary>
        /// Starts the current instance of the <see cref="RecorderJob"/>. The action is performed
        /// only if the instance is stopped.
        /// </summary>
        /// <remarks>
        /// An internal timer is started that will elapse according to the <see cref="StampingInterval"/> value.
        /// </remarks>
        public void Start()
        {
            switch (State)
            {
                case JobState.Stopped:
                    DoStart();
                    break;

                case JobState.Running:
                    break;
            }
        }

        private void DoStart()
        {
            lock (stateSynchronizer)
            {
                timer.Change(stampingInterval, stampingInterval);
                State = JobState.Running;
            }
        }

        /// <summary>
        /// Stops the current instance of the <see cref="RecorderJob"/>. The action is performed
        /// only if the service is started.
        /// </summary>
        /// <remarks>
        /// The internal timer is stopped.
        /// </remarks>
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
                timer.Change(-1, -1);
                State = JobState.Stopped;
            }
        }

        private async Task Execute()
        {
            StampRequest stampRequest = new StampRequest();
            await mediator.Send(stampRequest);
        }

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

        ~RecorderJob()
        {
            Dispose(false);
        }
    }
}