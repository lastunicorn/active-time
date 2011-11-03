using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Events;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using Microsoft.Practices.Prism.Events;

namespace DustInTheWind.ActiveTime.RecorderModule.Services
{
    /// <summary>
    /// Periodically calls the scrib to update the time of the current record in the database.
    /// </summary>
    class RecorderService : IRecorderService
    {
        private readonly IScrib scrib;
        private readonly Timer timer;
        private TimeSpan stampingInterval;

        /// <summary>
        /// Specifies the state of the current recorder service.
        /// </summary>
        public RecorderState State { get; private set; }
        private readonly object stateSynchronizer = new object();


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
            if (Started != null)
            {
                Started(this, e);
            }
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
            if (Stopped != null)
            {
                Stopped(this, e);
            }
        }

        #endregion

        #region Event Stamping

        /// <summary>
        /// Event raised when ... Well, is raised when it should be raised. Ok?
        /// </summary>
        public event EventHandler Stamping;
        /// <summary>
        /// Raises the <see cref="Stamping"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStamping(EventArgs e)
        {
            if (Stamping != null)
            {
                Stamping(this, e);
            }
        }

        #endregion

        #region Event Stamped

        /// <summary>
        /// Event raised when ... Well, is raised when it should be raised. Ok?
        /// </summary>
        public event EventHandler Stamped;

        /// <summary>
        /// Raises the <see cref="Stamped"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStamped(EventArgs e)
        {
            if (Stamped != null)
            {
                Stamped(this, e);
            }
        }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="RecorderService"/> class.
        /// </summary>
        /// <param name="scrib"></param>
        /// <param name="eventAggregator"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RecorderService(IScrib scrib, IEventAggregator eventAggregator)
        {
            if (scrib == null)
                throw new ArgumentNullException("scrib");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.scrib = scrib;
            timer = new Timer(timer_tick);
            stampingInterval = TimeSpan.FromMinutes(1);

            ApplicationExitEvent applicationExitEvent = eventAggregator.GetEvent<ApplicationExitEvent>();
            if (applicationExitEvent != null)
                applicationExitEvent.Subscribe(new Action<object>(OnApplicationExitEvent));
        }

        private void OnApplicationExitEvent(object o)
        {
            DoStop(false);
        }

        private void timer_tick(object o)
        {
            DoStamp();
        }

        public void Start()
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    lock (stateSynchronizer)
                    {
                        DoStart();
                    }
                    OnStarted(EventArgs.Empty);
                    break;

                case RecorderState.Running:
                    break;

                default:
                    break;
            }
        }

        private void Stamp()
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    lock (stateSynchronizer)
                    {
                        DoStart();
                    }
                    OnStarted(EventArgs.Empty);

                    OnStamping(EventArgs.Empty);
                    lock (stateSynchronizer)
                    {
                        DoStamp();
                    }
                    OnStamped(EventArgs.Empty);
                    break;

                case RecorderState.Running:
                    OnStamping(EventArgs.Empty);
                    lock (stateSynchronizer)
                    {
                        DoStamp();
                    }
                    OnStamped(EventArgs.Empty);
                    break;

                default:
                    break;
            }
        }

        public void Stop(bool deleteLastRecord = false)
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    break;

                case RecorderState.Running:
                    lock (stateSynchronizer)
                    {
                        DoStop(deleteLastRecord);
                    }
                    OnStopped(EventArgs.Empty);
                    break;

                default:
                    break;
            }
        }

        private void DoStart()
        {
            // Stamp
            scrib.StampNew();
            
            // Start timer
            timer.Change(stampingInterval, stampingInterval);
           
            // Change the state.
            State = RecorderState.Running;
        }

        private void DoStamp()
        {
            scrib.Stamp();
        }

        private void DoStop(bool deleteLastRecord)
        {
            // Stop timer
            timer.Change(-1, -1);

            if (deleteLastRecord)
            {
                // Delete database record from the db.
                scrib.DeleteDatabaseRecord();
            }
            else
            {
                DoStamp();
            }

            // Change the state.
            State = RecorderState.Stopped;
        }

        public TimeSpan? GetTimeFromLastStop()
        {
            return State == RecorderState.Running ? null : scrib.GetTimeFromLastStamp();
        }
    }
}
