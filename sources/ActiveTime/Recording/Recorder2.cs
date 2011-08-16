using System;
using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.Recording
{
    public class Recorder2
    {
        private ITimeRecordRepository timeRecordRepository;

        private object stateSynchronizer = new object();

        /// <summary>
        /// Specifies the state of the current instance.
        /// </summary>
        public RecorderState State { get; set; }


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
        public Recorder2(ITimeRecordRepository recordRepository)
        {
            if (recordRepository == null)
                throw new ArgumentNullException("recordRepository");

            this.timeRecordRepository = recordRepository;
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
        }

        public void Stop(bool deleteLastRecord)
        {
        }


        private void DoStart()
        {
            NewRecordIfNeeded();
            State = RecorderState.Running;
        }


        private void NewRecordIfNeeded()
        {
            DateTime now = DateTime.Now;

            if (!ExistsCurrentRecord)
            {
                CreateNewCurrentRecord(now, false);
                SaveCurrentRecordToDb();
            }
            else if (IsNewDay(now))
            {
                // day was changed. update the last record.
                FillCurrentRecordToEndDay();
                SaveCurrentRecordToDb();

                // create a new record starting from the beginning of the day.
                CreateNewCurrentRecord(now, true);

                SaveCurrentRecordToDb();
            }
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

        private void DeleteCurrentDatabaseRecordFromDb()
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
