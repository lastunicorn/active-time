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
using System.Linq;
using System.Linq.Expressions;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.PersistenceModule.LiteDB;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Repositories
{
    public class DbAssert
    {
        private const string ConnectionString = Constants.DatabaseFileName;

        public static void AssertExistsTimeRecord(int id)
        {
            long recordCount = ReadTimeRecordCount(x => x.Id == id);

            if (recordCount != 1)
            {
                string errorMessage = string.Format("  Expected to find in database one record with id: {0}.\r\n  But found {1} records.", id, recordCount);
                throw new AssertionException(errorMessage);
            }
        }
        public static void AssertTimeRecordCount(int expectedCount)
        {
            long actualCount = ReadTimeRecordCount(x => true);

            if (actualCount != expectedCount)
            {
                string errorMessage = string.Format("  Expected to find in database {0} record(s).\r\n  But found {1} records.", expectedCount, actualCount);
                throw new AssertionException(errorMessage);
            }
        }

        public static void AssertExistsAnyTimeRecord()
        {
            long recordCount = ReadTimeRecordCount(x => true);

            if (recordCount <= 0)
                throw new AssertionException("  Expected to find in database at least one record.");
        }

        public static void AssertDoesNotExistAnyTimeRecord()
        {
            long recordCount = ReadTimeRecordCount(x => true);

            if (recordCount > 0)
            {
                string errorMessage = string.Format("  Expected to find no records in the database.\r\n  But found: {0}", recordCount);
                throw new AssertionException(errorMessage);
            }
        }

        private static long ReadTimeRecordCount(Expression<Func<TimeRecord, bool>> predicate)
        {
            using (LiteDatabase database = new LiteDatabase(ConnectionString))
            {
                return database.GetCollection<TimeRecord>("TimeRecord")
                    .Find(predicate)
                    .Count();
            }
        }

        public static void AssertExistsTimeRecordEqualTo(TimeRecord expectedTimeRecord)
        {
            TimeRecord actualTimeRecord = GetTimeRecordById(expectedTimeRecord.Id);

            if (actualTimeRecord == null)
            {
                string errorMessage = string.Format("There is no TimeRecord in the database with the id {0}", expectedTimeRecord.Id);
                throw new AssertionException(errorMessage);
            }

            if (!actualTimeRecord.Equals(expectedTimeRecord))
            {
                string errorMessage = string.Format("The TimeRecord from the database is different from the expected one.\r\n  Actual TimeRecord: {0}\r\n  Expected TimeRecord: {1}", actualTimeRecord, expectedTimeRecord);
                throw new AssertionException(errorMessage);
            }
        }

        public static void AssertDoesNotExistTimeRecord(int id)
        {
            long recordCount = ReadTimeRecordCount(x => x.Id == id);

            if (recordCount != 0)
            {
                string errorMessage = string.Format("  Expected to not find record {0} in the database.", id);
                throw new AssertionException(errorMessage);
            }
        }

        private static TimeRecord GetTimeRecordById(int id)
        {
            using (LiteDatabase database = new LiteDatabase(ConnectionString))
            {
                return database.GetCollection<TimeRecord>("TimeRecord")
                    .Find(x => x.Id == id)
                    .FirstOrDefault();
            }
        }
    }
}