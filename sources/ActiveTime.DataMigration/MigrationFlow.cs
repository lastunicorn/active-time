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
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ActiveTime.Common.Persistence;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.PersistenceModule.SQLite.AdoNet.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.PersistenceModule.LiteDB.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration
{
    internal class MigrationFlow : IFlow
    {
        private Dictionary<DateTime, bool> destinationExitentDays;
        private IUnitOfWork sourceUnitOfWork;
        private IUnitOfWork destinationUnitOfWork;
        private int migratedRecordsCount;
        private int ignoredRecordsCount;
        private Dictionary<DateTime, string> warnings;

        public void Run()
        {
            PrepareMigration();

            try
            {
                MigrateAllRecords();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                DisplayError(ex);
            }
            finally
            {
                ConcludeMigration();
            }
        }

        private void PrepareMigration()
        {
            Console.WriteLine("Migrating TimeRecords");

            destinationExitentDays = new Dictionary<DateTime, bool>();
            warnings = new Dictionary<DateTime, string>();
            migratedRecordsCount = 0;
            ignoredRecordsCount = 0;

            sourceUnitOfWork = new SQLiteUnitOfWork();
            destinationUnitOfWork = new LiteDBUnitOfWork();
        }

        private void ConcludeMigration()
        {
            Console.WriteLine();
            DisplaySuccess(string.Format("Successfully migrated records: {0}", migratedRecordsCount));
            DisplaySuccess(string.Format("Already present records: {0}", ignoredRecordsCount));

            if (warnings.Count > 0)
            {
                Console.WriteLine();
                DisplayWarning("Warnings:");

                foreach (string warningText in warnings.Values)
                    DisplayWarning(string.Format(" {0}", warningText));
            }

            sourceUnitOfWork?.Dispose();
            destinationUnitOfWork?.Dispose();
        }

        private void MigrateAllRecords()
        {
            IEnumerable<TimeRecord> timeRecords = sourceUnitOfWork.TimeRecordRepository.GetAll();

            foreach (TimeRecord timeRecord in timeRecords)
                MigrateRecord(timeRecord);

            Console.WriteLine();

            destinationUnitOfWork.Commit();
        }

        private void MigrateRecord(TimeRecord timeRecord)
        {
            DateTime date = timeRecord.Date;

            IEnumerable<TimeRecord> destinationRecords = destinationUnitOfWork.TimeRecordRepository.GetByDate(date);

            bool existsDestinationRecords = CheckIfDateExistsInDestination(date, destinationRecords);

            if (existsDestinationRecords)
            {
                bool existsIdenticalRecord = destinationRecords
                    .Any(x => x.Date == timeRecord.Date &&
                              x.StartTime == timeRecord.StartTime &&
                              x.EndTime == timeRecord.EndTime &&
                              x.RecordType == timeRecord.RecordType);

                if (existsIdenticalRecord)
                {
                    ignoredRecordsCount++;
                }
                else
                {
                    if (!warnings.ContainsKey(date))
                    {
                        string message = string.Format("Date {0:d} conflict: Some records already exists in the destination database. No record will be imported.", date);
                        warnings.Add(date, message);
                    }
                }
            }
            else
            {
                InsertRecordInDestination(timeRecord);
                migratedRecordsCount++;
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
            Console.Write(".");
        }

        private bool CheckIfDateExistsInDestination(DateTime date, IEnumerable<TimeRecord> destinationRecords)
        {
            if (destinationExitentDays.ContainsKey(date))
                return destinationExitentDays[date];

            bool existsRecords = destinationRecords.Any();
            destinationExitentDays.Add(date, existsRecords);

            return existsRecords;
        }

        private void DisplayError(Exception ex)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ForegroundColor = old;
        }

        private void DisplayWarning(string text)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = old;
        }

        private void DisplaySuccess(string text)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = old;
        }
    }
}