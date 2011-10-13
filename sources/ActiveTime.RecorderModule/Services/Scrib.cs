using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.RecorderModule.Services
{
    class Scrib
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

        public void CreateNewRecord()
        {
            CreateNewRecordInternal();
        }

        public void Stamp()
        {
            if (record == null)
                CreateNewRecordInternal();

            repository.Update(record);
        }

        private void CreateNewRecordInternal()
        {
            DateTime now = timeProvider.GetDateTime();
            TimeRecord record = new TimeRecord
                                    {
                                        RecordType = TimeRecordType.Normal,
                                        Date = now.Date,
                                        StartTime = now.TimeOfDay,
                                        EndTime = now.TimeOfDay
                                    };
            repository.Add(record);

            this.record = record;
        }
    }
}
