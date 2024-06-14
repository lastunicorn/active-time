﻿using System;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.Persistence;
using DustInTheWind.ActiveTime.Ports.SystemAccess;

namespace DustInTheWind.ActiveTime.Application.Recording2
{
    /// <summary>
    /// Keeps track of a current record and updates it in the database when requested.
    /// </summary>
    /// <remarks>
    /// The current record can be obtained in two ways: 1) from the database or 2) by creating a new one.
    /// </remarks>
    public class Scribe
    {
        private readonly ISystemClock systemClock;
        private readonly IUnitOfWork unitOfWork;
        private readonly CurrentDay currentDay;

        public Scribe(ISystemClock systemClock, IUnitOfWork unitOfWork, CurrentDay currentDay)
        {
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        }

        /// <summary>
        /// Creates a new time record and saves it in the repository.
        /// </summary>
        public void StampNew()
        {
            DateTime now = systemClock.GetCurrentTime();
            CreateNewTimeRecord(now);
        }

        private void CreateNewTimeRecord(DateTime now)
        {
            TimeRecord newTimeRecord = new TimeRecord(now);
            unitOfWork.TimeRecordRepository.Add(newTimeRecord);
            currentDay.TimeRecordId = newTimeRecord.Id;
        }

        /// <summary>
        /// Updates the current time record with the current time.
        /// If there is no record a new one is automatically created.
        /// </summary>
        public void Stamp()
        {
            TimeRecord currentTimeRecord = RetrieveCurrentTimeRecord();

            DateTime now = systemClock.GetCurrentTime();

            if (currentTimeRecord == null)
            {
                CreateNewTimeRecord(now);
            }
            else
            {
                bool isSameDay = currentTimeRecord.Date == now.Date;

                if (isSameDay)
                {
                    currentTimeRecord.EndTime = now.TimeOfDay;
                }
                else
                {
                    currentTimeRecord.EndAtMidnight();

                    TimeRecord newTimeRecord = TimeRecord.NewFromMidnight(now);
                    unitOfWork.TimeRecordRepository.Add(newTimeRecord);
                    currentDay.TimeRecordId = newTimeRecord.Id;
                }
            }
        }

        /// <summary>
        /// Deletes from the database the current time record.
        /// If no time record was created, nothing happens.
        /// </summary>
        public void DeleteCurrentTimeRecord()
        {
            TimeRecord currentTimeRecord = RetrieveCurrentTimeRecord();

            if (currentTimeRecord == null)
                return;

            unitOfWork.TimeRecordRepository.Delete(currentTimeRecord);

            currentDay.TimeRecordId = null;
        }

        private TimeRecord RetrieveCurrentTimeRecord()
        {
            if (currentDay.TimeRecordId == null)
                return null;

            int currentTimeRecordId = currentDay.TimeRecordId.Value;
            return unitOfWork.TimeRecordRepository.GetById(currentTimeRecordId);
        }
    }
}