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
using DustInTheWind.ActiveTime.Persistence;

namespace DustInTheWind.ActiveTime.Recording
{
    /// <summary>
    /// This class periodically updates the current opened record with the latest time.
    /// </summary>
    public class Recorder : IDisposable
    {
        /// <summary>
        /// Used to access the persistent layer.
        /// </summary>
        private Dal dal;

        /// <summary>
        /// Specifies the state of the current instance.
        /// </summary>
        public RecorderState State { get; set; }

        private DateTime currentDate;
        private object stateSynchronizer = new object();

        private Record currentRecord;

        public Record CurrentRecord
        {
            get { return currentRecord; }
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
        /// <param name="e">An <see cref="StoppedEventArgs"/> that contains the event data.</param>
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


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Recorder"/> class.
        /// </summary>
        /// <param name="dal">Dal class used to access the persistent layer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Recorder(Dal dal)
        {
            if (dal == null)
                throw new ArgumentNullException("dal");

            this.dal = dal;
            State = RecorderState.Stopped;
        }

        #endregion


        private void NewRecordIfNeeded()
        {
            DateTime now = DateTime.Now;
            DateTime today = now.Date;

            if (currentDate != today || currentRecord == null)
            {
                if (currentRecord != null)
                {
                    // day was changed. update db.
                    //currentRecord.EndTime = TimeSpan.FromHours(24);
                    currentRecord.EndTime = TimeSpan.FromDays(1).Subtract(TimeSpan.FromSeconds(1));
                    dal.UpdateEndTime(currentRecord);

                    // create a new record.
                    currentDate = today;
                    currentRecord = new Record(today, TimeSpan.Zero);
                }
                else
                {
                    // create a new record.
                    currentDate = today;
                    currentRecord = new Record(today, now.TimeOfDay);
                }

                dal.AddRecord(currentRecord);
            }
        }

        public void Start()
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    lock (stateSynchronizer)
                    {
                        NewRecordIfNeeded();

                        State = RecorderState.Running;
                    }

                    OnStarted(EventArgs.Empty);

                    break;

                case RecorderState.Running:
                    break;

                default:
                    break;
            }
        }

        public void Stop()
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    break;

                case RecorderState.Running:
                    lock (stateSynchronizer)
                    {
                        NewRecordIfNeeded();
                        UpdateTime();

                        currentRecord = null;

                        State = RecorderState.Stopped;
                    }

                    OnStopped(EventArgs.Empty);

                    break;

                default:
                    break;
            }
        }

        public void Stamp()
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    break;

                case RecorderState.Running:

                    OnStamping(EventArgs.Empty);

                    lock (stateSynchronizer)
                    {
                        NewRecordIfNeeded();
                        UpdateTime();
                    }

                    OnStamped(EventArgs.Empty);

                    break;

                default:
                    break;
            }
        }

        private void UpdateTime()
        {
            currentRecord.EndTime = DateTime.Now.TimeOfDay;
            dal.UpdateEndTime(currentRecord);
        }

        public void StopAndDeleteLastRecord()
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    break;

                case RecorderState.Running:
                    lock (stateSynchronizer)
                    {
                        UpdateTime();

                        dal.DeleteRecord(currentRecord);
                        currentRecord = null;

                        State = RecorderState.Stopped;
                    }

                    OnStopped(EventArgs.Empty);

                    break;

                default:
                    break;
            }
        }


        #region IDisposable Members

        private bool disposed = false;

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        /// <remarks>
        /// <para>Dispose(bool disposing) executes in two distinct scenarios.</para>
        /// <para>If the method has been called directly or indirectly by a user's code managed and unmanaged resources can be disposed.</para>
        /// <para>If the method has been called by the runtime from inside the finalizer you should not reference other objects. Only unmanaged resources can be disposed.</para>
        /// </remarks>
        /// <param name="disposing">Specifies if the method has been called by a user's code (true) or by the runtime from inside the finalizer (false).</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up unmanaged resources here.
                // ...

                disposed = true;
            }
        }

        ~Recorder()
        {
            Dispose(false);
        }

        #endregion
    }
}
