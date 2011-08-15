using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.Recording
{
    public class Recorder2
    {
        private ITimeRecordRepository recordRepository;

        /// <summary>
        /// Specifies the state of the current instance.
        /// </summary>
        public RecorderState State { get; set; }

        private object stateSynchronizer = new object();

        private TimeRecord previousRecord;
        private TimeRecord currentRecord;

        public TimeRecord CurrentRecord
        {
            get { return currentRecord; }
        }


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

            this.recordRepository = recordRepository;
            State = RecorderState.Stopped;
        }

        #endregion


        public TimeSpan? GetTimeFromLastStop()
        {
            if (State == RecorderState.Running)
                return null;
            
            if (previousRecord == null)
                return null; // Recorder hes been never started.

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
