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

using System;
using System.Linq;
using System.Linq.Expressions;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Repositories;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.LiteDB.Helpers
{
    public partial class DbAssert
    {
        public static void AssertTimeRecordCount(int expectedCount, Expression<Func<TimeRecord, bool>> predicate = null)
        {
            long actualCount = ReadTimeRecordCount(predicate);

            if (actualCount != expectedCount)
            {
                string errorMessage = string.Format("Expected to find in database {0} record(s).\r\n  But found {1} records.", expectedCount, actualCount);
                throw new AssertionException(errorMessage);
            }
        }

        private static long ReadTimeRecordCount(Expression<Func<TimeRecord, bool>> predicate)
        {
            using (LiteDatabase database = new LiteDatabase(DbTestHelper.ConnectionString))
            {
                if (predicate == null)
                    predicate = x => true;

                return database.GetCollection<TimeRecord>(TimeRecordRepository.CollectionName)
                    .Find(predicate)
                    .Count();
            }
        }

        public static void AssertExistsTimeRecordEqualTo(TimeRecord expectedRecord)
        {
            TimeRecord actualRecord = GetTimeRecordById(expectedRecord.Id);

            if (actualRecord == null)
            {
                string errorMessage = string.Format("There is no record in the database with the id {0}", expectedRecord.Id);
                throw new AssertionException(errorMessage);
            }

            if (!actualRecord.Equals(expectedRecord))
            {
                string errorMessage = string.Format("The record from the database is different from the expected one.\r\n  Actual record: {0}\r\n  Expected record: {1}", actualRecord, expectedRecord);
                throw new AssertionException(errorMessage);
            }
        }

        private static TimeRecord GetTimeRecordById(int id)
        {
            using (LiteDatabase database = new LiteDatabase(DbTestHelper.ConnectionString))
            {
                return database.GetCollection<TimeRecord>(TimeRecordRepository.CollectionName)
                    .Find(x => x.Id == id)
                    .FirstOrDefault();
            }
        }
    }
}