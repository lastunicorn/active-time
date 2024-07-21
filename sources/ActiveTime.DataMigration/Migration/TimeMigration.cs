// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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

using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;

namespace DustInTheWind.ActiveTime.DataMigration.Migration
{
    internal class TimeMigration
    {
        private readonly IUnitOfWork sourceUnitOfWork;
        private readonly IUnitOfWork destinationUnitOfWork;

        private Dictionary<DateTime, bool> destinationExistentDays;

        public bool Simulate { get; set; }
        public int MigratedRecordsCount { get; private set; }
        public int IgnoredRecordsCount { get; private set; }
        public Dictionary<DateTime, string> Warnings { get; } = new Dictionary<DateTime, string>();

        public event EventHandler<TimeRecordMigratedEventArgs> TimeRecordMigrated;

        public TimeMigration(IUnitOfWork sourceUnitOfWork, IUnitOfWork destinationUnitOfWork)
        {
            this.sourceUnitOfWork = sourceUnitOfWork ?? throw new ArgumentNullException(nameof(sourceUnitOfWork));
            this.destinationUnitOfWork = destinationUnitOfWork ?? throw new ArgumentNullException(nameof(destinationUnitOfWork));
        }

        public void Migrate()
        {
            PrepareMigration();
            MigrateRecords();
        }

        private void PrepareMigration()
        {
            destinationExistentDays = new Dictionary<DateTime, bool>();
            Warnings.Clear();
            MigratedRecordsCount = 0;
            IgnoredRecordsCount = 0;
        }

        private void MigrateRecords()
        {
            IEnumerable<TimeRecord> timeRecords = sourceUnitOfWork.TimeRecordRepository.GetAll();

            foreach (TimeRecord timeRecord in timeRecords)
            {
                MigrateRecord(timeRecord);
                OnTimeRecordMigrated(new TimeRecordMigratedEventArgs(timeRecord));
            }
        }

        private void MigrateRecord(TimeRecord timeRecord)
        {
            DateTime date = timeRecord.Date;

            IEnumerable<TimeRecord> destinationRecords = destinationUnitOfWork.TimeRecordRepository.GetByDate(date);

            bool existsDestinationRecords = CheckIfDateExistsInDestination(date, destinationRecords);

            if (existsDestinationRecords)
            {
                bool existsIdenticalRecord = destinationRecords
                    .Any(x => x.Date.EqualsWithin(timeRecord.Date, TimeSpan.FromSeconds(1)) &&
                              x.StartTime.EqualsWithin(timeRecord.StartTime, TimeSpan.FromMinutes(1)) &&
                              x.EndTime.EqualsWithin(timeRecord.EndTime, TimeSpan.FromMinutes(1)) &&
                              x.RecordType == timeRecord.RecordType);

                if (existsIdenticalRecord)
                {
                    IgnoredRecordsCount++;
                }
                else
                {
                    if (!Warnings.ContainsKey(date))
                    {
                        string message = string.Format("Date {0:d} conflict: Some records already exists in the destination database. No record will be imported.", date);
                        Warnings.Add(date, message);
                    }
                }
            }
            else
            {
                if (!Simulate)
                    InsertRecordInDestination(timeRecord);

                MigratedRecordsCount++;
            }
        }

        private void InsertRecordInDestination(TimeRecord timeRecord)
        {
            TimeRecord timeRecordCopy = new TimeRecord
            {
                Date = timeRecord.Date,
                StartTime = timeRecord.StartTime,
                EndTime = timeRecord.EndTime,
                RecordType = timeRecord.RecordType
            };

            destinationUnitOfWork.TimeRecordRepository.Add(timeRecordCopy);
        }

        private bool CheckIfDateExistsInDestination(DateTime date, IEnumerable<TimeRecord> destinationRecords)
        {
            if (destinationExistentDays.ContainsKey(date))
                return destinationExistentDays[date];

            bool existsRecords = destinationRecords.Any();
            destinationExistentDays.Add(date, existsRecords);

            return existsRecords;
        }

        protected virtual void OnTimeRecordMigrated(TimeRecordMigratedEventArgs e)
        {
            TimeRecordMigrated?.Invoke(this, e);
        }
    }
}