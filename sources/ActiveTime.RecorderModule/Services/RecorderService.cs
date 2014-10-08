// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using System.Threading;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;

namespace DustInTheWind.ActiveTime.RecorderModule.Services
{
    /// <summary>
    /// Periodically calls the scribe to update the time of the current record in the database.
    /// </summary>
    class RecorderService : IRecorderService, IDisposable
    {
        private readonly IScribe scribe;

        /// <summary>
        /// Gets the state of the current recorder service.
        /// </summary>
        public RecorderState State { get; private set; }

        private readonly object stateSynchronizer = new object();

        private DateTime? lastStopTime;

        private readonly Timer timer;

        private TimeSpan stampingInterval;

        /// <summary>
        /// Gets or sets the interval of time to wait between two stamping actions.
        /// A stamp consists in updating the time record in the persistence layer (usually the database).
        /// </summary>
        public TimeSpan StampingInterval
        {
            get { return stampingInterval; }
            set
            {
                lock (stateSynchronizer)
                {
                    stampingInterval = value;

                    if (State == RecorderState.Running)
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
            EventHandler handler = Started;

            if (handler != null)
                handler(this, e);
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
            EventHandler handler = Stopped;

            if (handler != null)
                handler(this, e);
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
            EventHandler handler = Stamping;

            if (handler != null)
                handler(this, e);
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
            EventHandler handler = Stamped;

            if (handler != null)
                handler(this, e);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RecorderService"/> class.
        /// </summary>
        /// <param name="scribe"></param>
        /// <param name="applicationService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RecorderService(IScribe scribe, IApplicationService applicationService)
        {
            if (scribe == null)
                throw new ArgumentNullException("scribe");

            if (applicationService == null)
                throw new ArgumentNullException("applicationService");

            this.scribe = scribe;
            timer = new Timer(HandleTimerTick);
            stampingInterval = TimeSpan.FromMinutes(1);

            applicationService.Exiting += HandleApplicationExiting;
        }

        private void HandleApplicationExiting(object sender, EventArgs e)
        {
            DoStop(false);
        }

        private void HandleTimerTick(object o)
        {
            StampWithEvents();
        }

        #region Start

        /// <summary>
        /// Starts the current instance of the RecorderService. The action is performed
        /// only if the instance is stopped.
        /// </summary>
        /// <remarks>
        /// An internal timer is started that will elapse according to the <see cref="StampingInterval"/> value.
        /// </remarks>
        public void Start()
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    StartWithEvents();
                    break;

                case RecorderState.Running:
                    break;
            }
        }

        /// <summary>
        /// Private method that starts the current instance of the RecorderService.
        /// This method is thread safe and raises necessary events, but does not check the
        /// state of the service before it executes.
        /// </summary>
        private void StartWithEvents()
        {
            lock (stateSynchronizer)
            {
                DoStart();
            }

            OnStarted(EventArgs.Empty);
        }

        /// <summary>
        /// Private method that starts the current instance of the RecorderService.
        /// This method is not thread safe and does not raise any events.
        /// </summary>
        private void DoStart()
        {
            // Stamp
            scribe.StampNew();

            // Start timer
            timer.Change(stampingInterval, stampingInterval);

            // Change the state.
            State = RecorderState.Running;
        }

        #endregion

        #region Stamp

        /// <summary>
        /// Private method that stamps the scribe.
        /// This method is thread safe and raises necessary events, but does not check the
        /// state of the service before it executes.
        /// </summary>
        private void StampWithEvents()
        {
            OnStamping(EventArgs.Empty);

            lock (stateSynchronizer)
            {
                DoStamp();
            }

            OnStamped(EventArgs.Empty);
        }

        /// <summary>
        /// Private method that stamps the scribe.
        /// This method is not thread safe and does not raise any events.
        /// </summary>
        private void DoStamp()
        {
            scribe.Stamp();
        }

        #endregion

        #region Stop

        /// <summary>
        /// Stops the current instance of the RecorderService. The action is performed
        /// only if the service is started.
        /// </summary>
        /// <param name="deleteLastRecord">If <c>true</c>, the current record is deleted from the repository, after the service is stopped.</param>
        /// <remarks>
        /// The internal timer is stopped.
        /// </remarks>
        public void Stop(bool deleteLastRecord = false)
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    break;

                case RecorderState.Running:
                    StopWithEvents(deleteLastRecord);
                    break;
            }
        }

        /// <summary>
        /// Private method that stops the current instance of the RecorderService.
        /// This method is thread safe and raises necessary events, but does not check the
        /// state of the service before it executes.
        /// </summary>
        /// <param name="deleteLastRecord">If <c>true</c>, the current record is deleted from the scribe, after the service is stopped.</param>
        private void StopWithEvents(bool deleteLastRecord)
        {
            lock (stateSynchronizer)
            {
                DoStop(deleteLastRecord);
            }

            OnStopped(EventArgs.Empty);
        }

        /// <summary>
        /// Private method that stops the current instance of the RecorderService.
        /// This method is not thread safe and does not raise any events.
        /// </summary>
        /// <param name="deleteLastRecord">If <c>true</c>, the current record is deleted from the scribe, after the service is stopped.</param>
        private void DoStop(bool deleteLastRecord)
        {
            // Stop timer
            timer.Change(-1, -1);

            if (deleteLastRecord)
                scribe.DeleteCurrentTimeRecord();
            else
                DoStamp();

            lastStopTime = DateTime.Now;
            State = RecorderState.Stopped;
        }

        #endregion

        public TimeSpan? CalculateTimeFromLastStop()
        {
            return lastStopTime == null
                ? null
                : DateTime.Now - lastStopTime;
        }

        private bool isDisposed;

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

        ~RecorderService()
        {
            Dispose(false);
        }
    }
}
