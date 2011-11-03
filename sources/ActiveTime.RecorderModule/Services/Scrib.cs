using System;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.RecorderModule.Services
{
    /// <summary>
    /// Keeps track of a current record and updates it in the database when requested.
    /// </summary>
    /// <remarks>
    /// The current record can be obtained in two ways: 1) from the database or 2) created new.
    /// </remarks>
    class Scrib : IScrib
    {
        private readonly ITimeRecordRepository repository;
        private readonly ITimeProvider timeProvider;
        private TimeRecord record;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scrib"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="timeProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Scrib(ITimeRecordRepository repository, ITimeProvider timeProvider)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            if (timeProvider == null)
                throw new ArgumentNullException("timeProvider");

            this.repository = repository;
            this.timeProvider = timeProvider;
        }

        public void StampNew()
        {
            CreateNewRecordInternal();
        }

        /// <summary>
        /// Updates the current record with the current time and saves it into the
        /// repository. If there is no record a new one is automatically created.
        /// </summary>
        public void Stamp()
        {
            if (record == null)
                CreateNewRecordInternal();
            else
                repository.Update(record);
        }

        /// <summary>
        /// Creates a new record with default values and saves it into the repository.
        /// </summary>
        private void CreateNewRecordInternal()
        {
            DateTime now = timeProvider.GetDateTime();
            TimeRecord newRecord = new TimeRecord
                                    {
                                        RecordType = TimeRecordType.Normal,
                                        Date = now.Date,
                                        StartTime = now.TimeOfDay,
                                        EndTime = now.TimeOfDay
                                    };
            repository.Add(newRecord);
            record = newRecord;
        }

        private bool IsNewDay()
        {
            DateTime now = timeProvider.GetDateTime();
            return record != null && record.Date != now.Date;
        }


        public void DeleteDatabaseRecord()
        {
            if (record != null)
                repository.Delete(record);
        }

        public TimeSpan? GetTimeFromLastStamp()
        {
            if (record == null) return null;

            DateTime now = timeProvider.GetDateTime();

            if (record.Date < now.Date)
            {
                TimeSpan a = TimeSpan.FromDays(1) - record.EndTime;
                TimeSpan b = now.Date - record.Date + TimeSpan.FromDays(1);
                TimeSpan c = now.TimeOfDay;

                return a + b + c;
            }
            else
            {
                return now - now.Date.Add(record.EndTime);
            }
        }
    }
}
