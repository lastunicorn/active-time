using System;
using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.Recording
{
    public class Recorder
    {
        private ITimeRecordRepository timeRecordRepository;

        private object stateSynchronizer = new object();

        /// <summary>
        /// Specifies the state of the current instance.
        /// </summary>
        public RecorderState State { get; private set; }


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
        public Recorder(ITimeRecordRepository timeRecordRepository)
        {
            if (timeRecordRepository == null)
                throw new ArgumentNullException("timeRecordRepository");

            this.timeRecordRepository = timeRecordRepository;
            State = RecorderState.Stopped;
        }

        #endregion


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

        public void Stamp()
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
            DateTime now = DateTime.Now;

            // Create a new current record.
            CreateNewCurrentRecord(now, false);
            // Create a new database record from current record.
            CreateNewDatabaseRecord();
            // Save database record to db.
            timeRecordRepository.Add(databaseRecord);
            // Change the state.
            State = RecorderState.Running;
        }

        private void DoStamp()
        {
            DateTime now = DateTime.Now;

            if (IsNewDay(now))
            {
                // Update current record.
                FillCurrentRecordToEndDay();
                // Update database record.
                databaseRecord.EndTime = currentRecord.EndTime;
                // Save database record to db.
                timeRecordRepository.Update(databaseRecord);

                // Create a new current record.
                CreateNewCurrentRecord(now, true);
                // Create a new database record from current record.
                CreateNewDatabaseRecord();
                // Save database record to db.
                timeRecordRepository.Add(databaseRecord);
            }

            // Update the current record's end time.
            UpdateCurrentRecordEndTime(now);
            // Update the database record's end time.
            databaseRecord.EndTime = currentRecord.EndTime;
            // Save database record in db.
            timeRecordRepository.Update(databaseRecord);
        }

        private void DoStop(bool deleteLastRecord)
        {
            if (deleteLastRecord)
            {
                // Delete database record from the db.
                DeleteDatabaseRecordFromDb();
            }
            else
            {
                DoStamp();
            }

            // Delete database record.
            databaseRecord = null;
            // Delete current record.
            currentRecord = null;
            // Change the state.
            State = RecorderState.Stopped;
        }


        private bool IsNewDay(DateTime now)
        {
            return ExistsCurrentRecord && currentRecord.Date != now.Date;
        }

        #region Previous Record

        private Record previousRecord;

        private bool IsNeverStarted
        {
            get { return previousRecord == null; }
        }

        #endregion

        #region Current Record

        private Record currentRecord;

        private bool ExistsCurrentRecord
        {
            get { return currentRecord != null; }
        }

        private void CreateNewCurrentRecord(DateTime now, bool startsFromBeginningOfDay)
        {
            if (startsFromBeginningOfDay)
                currentRecord = new Record(now.Date, TimeSpan.Zero, now.TimeOfDay);
            else
                currentRecord = new Record(now.Date, now.TimeOfDay, now.TimeOfDay);

            databaseRecord = null;
        }

        private void UpdateCurrentRecordEndTime(DateTime now)
        {
            currentRecord.EndTime = now.TimeOfDay;
        }

        private void FillCurrentRecordToEndDay()
        {
            currentRecord.EndTime = TimeSpan.FromDays(1).Subtract(TimeSpan.FromTicks(1));
        }

        private void SaveCurrentRecordToDb()
        {
            if (!ExistsCurrentRecord)
                return;

            if (ExistsDatabaseRecord)
            {
                databaseRecord.EndTime = currentRecord.EndTime;
                timeRecordRepository.Update(databaseRecord);
            }
            else
            {
                CreateNewDatabaseRecord();
                timeRecordRepository.Add(databaseRecord);
            }
        }

        #endregion

        #region Database Record

        private TimeRecord databaseRecord = null;

        private bool ExistsDatabaseRecord
        {
            get { return databaseRecord != null; }
        }

        private void CreateNewDatabaseRecord()
        {
            databaseRecord = new TimeRecord
            {
                Date = currentRecord.Date,
                StartTime = currentRecord.StartTime,
                EndTime = currentRecord.EndTime,
                RecordType = TimeRecordType.Normal
            };
        }

        private void DeleteDatabaseRecordFromDb()
        {
            if (!ExistsDatabaseRecord)
                return;

            timeRecordRepository.Delete(databaseRecord);
        }

        #endregion

        public TimeSpan? GetTimeFromLastStop()
        {
            if (State == RecorderState.Running)
                return null;

            if (IsNeverStarted)
                return null; // Recorder has never been started.

            DateTime now = DateTime.Now;

            if (previousRecord.Date < now.Date)
            {
                TimeSpan a = TimeSpan.FromDays(1) - previousRecord.EndTime;
                TimeSpan b = now.Date - previousRecord.Date + TimeSpan.FromDays(1);
                TimeSpan c = now.TimeOfDay;

                return a + b + c;
            }

            return now - now.Date.Add(previousRecord.EndTime);
        }
    }
}
