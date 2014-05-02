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

using System.Data.SQLite;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.AdoNet.Repositories
{
    public class DbAssert
    {
        public static void AssertExistsTimeRecord(int id)
        {
            long recordCount = ReadTimeRecordCount(string.Format("select count(*) from records where id = {0}", id));

            if (recordCount != 1)
            {
                string errorMessage = string.Format("  Expected to find in database one record with id: {0}.\r\n  But found {1} records.", id, recordCount);
                throw new AssertionException(errorMessage);
            }
        }
        public static void AssertTimeRecordCount(int expectedCount)
        {
            long actualCount = ReadTimeRecordCount("select count(*) from records");

            if (actualCount != expectedCount)
            {
                string errorMessage = string.Format("  Expected to find in database {0} record(s).\r\n  But found {1} records.", expectedCount, actualCount);
                throw new AssertionException(errorMessage);
            }
        }

        public static void AssertExistsAnyTimeRecord()
        {
            long recordCount = ReadTimeRecordCount("select count(*) from records");

            if (recordCount <= 0)
                throw new AssertionException("  Expected to find in database at least one record.");
        }

        public static void AssertDoesNotExistAnyTimeRecord()
        {
            long recordCount = ReadTimeRecordCount("select count(*) from records");

            if (recordCount > 0)
            {
                string errorMessage = string.Format("  Expected to find no records in the database.\r\n  But found: {0}", recordCount);
                throw new AssertionException(errorMessage);
            }
        }

        private static long ReadTimeRecordCount(string sql)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=db.s3db"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.CommandText = sql;
                    command.Connection = connection;

                    long recordCount = (long)command.ExecuteScalar();

                    return recordCount;
                }
            }
        }
    }
}