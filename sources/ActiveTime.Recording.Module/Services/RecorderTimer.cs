using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.UseCases.Stamp;
using DustInTheWind.ActiveTime.Common.Jobs;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Recording.Module.Services
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
        private DateTime? lastStopTime;
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

        #region Event Started

        /// <summary>
        /// Event raised when the Recorder is started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStarted(EventArgs e)
        {
            Started?.Invoke(this, e);
        }

        #endregion

        #region Event Stopped

        /// <summary>
        /// Event raised when the Recorder is stopped.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Raises the <see cref="Stopped"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStopped(EventArgs e)
        {
            Stopped?.Invoke(this, e);
        }

        #endregion

        #region Event Stamping

        /// <summary>
        /// Event raised immediately before a new stamping is initiated.
        /// </summary>
        public event EventHandler Stamping;

        /// <summary>
        /// Raises the <see cref="Stamping"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStamping(EventArgs e)
        {
            Stamping?.Invoke(this, e);
        }

        #endregion

        #region Event Stamped

        /// <summary>
        /// Event raised after a stamping was finished.
        /// </summary>
        public event EventHandler Stamped;

        /// <summary>
        /// Raises the <see cref="Stamped"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStamped(EventArgs e)
        {
            Stamped?.Invoke(this, e);
        }

        #endregion

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
            _ = StampWithEvents();
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
                    StartWithEvents();
                    break;

                case JobState.Running:
                    break;
            }
        }

        private void StartWithEvents()
        {
            lock (stateSynchronizer)
            {
                timer.Change(stampingInterval, stampingInterval);
                State = JobState.Running;
            }

            OnStarted(EventArgs.Empty);
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
                    StopWithEvents();
                    break;
            }
        }

        private void StopWithEvents()
        {
            lock (stateSynchronizer)
            {
                // Stop timer
                timer.Change(-1, -1);

                lastStopTime = DateTime.Now;
                State = JobState.Stopped;
            }

            OnStopped(EventArgs.Empty);
        }

        private async Task StampWithEvents()
        {
            OnStamping(EventArgs.Empty);

            StampRequest stampRequest = new StampRequest();
            await mediator.Send(stampRequest);

            OnStamped(EventArgs.Empty);
        }

        public TimeSpan? CalculateTimeFromLastStop()
        {
            return lastStopTime == null
                ? null
                : DateTime.Now - lastStopTime;
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