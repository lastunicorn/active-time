// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using DustInTheWind.ActiveTime.Domain;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.SQLite.AdoNet.Helpers;

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
        using SQLiteConnection connection = new(DbTestHelper.ConnectionString);
        connection.Open();

        using SQLiteCommand command = new();
        command.CommandText = sql;
        command.Connection = connection;

        long recordCount = (long)command.ExecuteScalar();

        return recordCount;
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
        string sql = string.Format("select count(*) from records where id={0}", id);

        long recordCount = ReadTimeRecordCount(sql);

        if (recordCount != 0)
        {
            string errorMessage = string.Format("  Expected to not find record {0} in the database.", id);
            throw new AssertionException(errorMessage);
        }
    }

    private static TimeRecord GetTimeRecordById(int id)
    {
        string sql = string.Format("select * from records where id='{0}'", id);

        using SQLiteConnection connection = new(DbTestHelper.ConnectionString);
        connection.Open();

        using SQLiteCommand command = new(connection);
        command.CommandText = string.Format("select * from records where id={0}", id);
        command.Connection = connection;

        using (SQLiteDataReader dataReader = command.ExecuteReader())
        {
            if (!dataReader.Read())
                return null;

            object idAsObject = dataReader["id"];
            object dateAsObject = dataReader["date"];
            object startTimeAsObject = dataReader["start_time"];
            object endTimeAsObject = dataReader["end_time"];
            object recordTypeAsObject = dataReader["type"];

            return new TimeRecord
            {
                Id = int.Parse(idAsObject.ToString()),
                Date = DateTime.Parse(dateAsObject.ToString()),
                StartTime = DateTime.Parse(startTimeAsObject.ToString()).TimeOfDay,
                EndTime = DateTime.Parse(endTimeAsObject.ToString()).TimeOfDay,
                RecordType = (TimeRecordType)Enum.Parse(typeof(TimeRecordType), recordTypeAsObject.ToString())
            };
        }
    }
}