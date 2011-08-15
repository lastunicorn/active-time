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
using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.Recording
{
    /// <summary>
    /// The main purpose of this class is to keep the last active time record and
    /// to updates it into the database when requested.
    /// </summary>
    public class Recorder : IDisposable
    {
        private ITimeRecordRepository timeRecordRepository;

        /// <summary>
        /// Specifies the state of the current instance.
        /// </summary>
        public RecorderState State { get; set; }

        private object stateSynchronizer = new object();

        private Record previousRecord;
        private Record currentRecord;
        private TimeRecord databaseRecord = null;
        
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
        /// <param name="timeRecordRepository">Dal class used to access the persistent layer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Recorder(ITimeRecordRepository timeRecordRepository)
        {
            if (timeRecordRepository == null)
                throw new ArgumentNullException("timeRecordRepository");

            this.timeRecordRepository = timeRecordRepository;
            State = RecorderState.Stopped;
        }

        #endregion

        private void SaveCurrentRecordToDb()
        {
            if (currentRecord == null)
                return;

            if (databaseRecord == null)
            {
                databaseRecord = new TimeRecord
                {
                    Date = currentRecord.Date,
                    StartTime = currentRecord.StartTime,
                    RecordType = TimeRecordType.Normal
                };

                databaseRecord.EndTime = currentRecord.EndTime;

                timeRecordRepository.Add(databaseRecord);
            }
            else
            {
                databaseRecord.EndTime = currentRecord.EndTime;
                timeRecordRepository.Update(databaseRecord);
            }
        }

        private void NewRecordIfNeeded()
        {
            DateTime now = DateTime.Now;

            if (IsNewDay(now) || currentRecord == null)
            {
                if (currentRecord != null)
                {
                    // day was changed. update the last record.
                    currentRecord.EndTime = TimeSpan.FromDays(1).Subtract(TimeSpan.FromTicks(1));
                    SaveCurrentRecordToDb();

                    // create a new record starting from the beggining of the day.
                    currentRecord = CreateNewRecord(now.Date);
                    currentRecord.EndTime = now.TimeOfDay;
                }
                else
                {
                    // create a new record.
                    currentRecord = CreateNewRecord(now);
                }

                databaseRecord = null;
                SaveCurrentRecordToDb();
            }
        }

        private Record CreateNewRecord(DateTime now)
        {
            return new Record(now.Date, now.TimeOfDay);
        }

        private bool IsNewDay(DateTime now)
        {
            return currentRecord != null && currentRecord.Date != now.Date;
        }

        public TimeSpan? TimeFromLastStop()
        {
            if (currentRecord != null) return null;
            if (previousRecord == null) return null;

            DateTime now = DateTime.Now;
            return now - now.Date.Add(previousRecord.EndTime);
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

        public void Stop()
        {
            switch (State)
            {
                case RecorderState.Stopped:
                    break;

                case RecorderState.Running:
                    lock (stateSynchronizer)
                    {
                        DoStop();
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
                        DoStamp();
                    }

                    OnStamped(EventArgs.Empty);

                    break;

                default:
                    break;
            }
        }

        private void DoStart()
        {
            NewRecordIfNeeded();
            State = RecorderState.Running;
        }

        private void DoStop()
        {
            NewRecordIfNeeded();
            UpdateTime();

            previousRecord = currentRecord;
            currentRecord = null;

            State = RecorderState.Stopped;
        }

        private void DoStamp()
        {
            NewRecordIfNeeded();
            UpdateTime();
        }

        private void UpdateTime()
        {
            currentRecord.EndTime = DateTime.Now.TimeOfDay;

            SaveCurrentRecordToDb();
            //timeRecordRepository.Update(currentRecord);
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
                        //UpdateTime();

                        DeleteCurrentRecordFromDb();
                        //timeRecordRepository.Delete(currentRecord);
                        currentRecord = null;

                        State = RecorderState.Stopped;
                    }

                    OnStopped(EventArgs.Empty);

                    break;

                default:
                    break;
            }
        }

        private void DeleteCurrentRecordFromDb()
        {
            if (databaseRecord == null)
                return;

            timeRecordRepository.Delete(databaseRecord);
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
