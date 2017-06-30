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
using DustInTheWind.ActiveTime.Common.Persistence;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.PersistenceModule.SQLite.AdoNet.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.PersistenceModule.LiteDB.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration
{
    internal class MigrationFlow : IFlow
    {
        public void Run()
        {
            Migrate();
        }

        private static void Migrate()
        {
            Console.WriteLine("Migrating TimeRecords");

            try
            {
                using (IUnitOfWork sourceUnitOfWork = new SQLiteUnitOfWork())
                using (IUnitOfWork destinationUnitOfWork = new LiteDBUnitOfWork())
                {
                    IList<TimeRecord> timeRecords = sourceUnitOfWork.TimeRecordRepository.GetAll();

                    int count = 0;

                    foreach (TimeRecord timeRecord in timeRecords)
                    {
                        TimeRecord timeRecordCopy = new TimeRecord
                        {
                            Date = timeRecord.Date,
                            StartTime = timeRecord.StartTime,
                            EndTime = timeRecord.EndTime,
                            RecordType = timeRecord.RecordType
                        };

                        destinationUnitOfWork.TimeRecordRepository.AddIfNotExist(timeRecordCopy);
                        Console.Write(".");
                        count++;
                    }

                    Console.WriteLine();
                    Console.WriteLine("Migrated {0} records", count);

                    destinationUnitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex);
            }
        }
    }
}