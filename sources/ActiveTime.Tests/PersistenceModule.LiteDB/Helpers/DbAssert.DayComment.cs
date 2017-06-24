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
using DustInTheWind.ActiveTime.PersistenceModule.LiteDB.Repositories;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Helpers
{
    public partial class DbAssert
    {
        public static void AssertDayCommentCount(int expectedCount, Expression<Func<DayComment, bool>> predicate = null)
        {
            long actualCount = ReadDayCommentCount(predicate);

            if (actualCount != expectedCount)
            {
                string errorMessage = string.Format("Expected to find in database {0} record(s).\r\n  But found {1} records.", expectedCount, actualCount);
                throw new AssertionException(errorMessage);
            }
        }

        private static long ReadDayCommentCount(Expression<Func<DayComment, bool>> predicate)
        {
            using (LiteDatabase database = new LiteDatabase(DbTestHelper.ConnectionString))
            {
                if (predicate == null)
                    predicate = x => true;

                return database.GetCollection<DayComment>(DayCommentRepository.CollectionName)
                    .Find(predicate)
                    .Count();
            }
        }

        public static void AssertExistsDayCommentEqualTo(DayComment expectedRecord)
        {
            DayComment actualRecord = GetDayCommentById(expectedRecord.Id);

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

        private static DayComment GetDayCommentById(int id)
        {
            using (LiteDatabase database = new LiteDatabase(DbTestHelper.ConnectionString))
            {
                return database.GetCollection<DayComment>(DayCommentRepository.CollectionName)
                    .Find(x => x.Id == id)
                    .FirstOrDefault();
            }
        }
    }
}