// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.Repositories;
using DustInTheWind.ActiveTime.UnitTests.PersistenceModule.SQLite.AdoNet.Helpers;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.SQLite.AdoNet.Repositories.TimeRecordRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class DeleteTests
    {
        private TimeRecordRepository timeRecordRepository;
        private SQLiteConnection connection;

        [SetUp]
        public void SetUp()
        {
            DbTestHelper.ClearDatabase();

            connection = new SQLiteConnection(DbTestHelper.ConnectionString);
            connection.Open();
            timeRecordRepository = new TimeRecordRepository(connection);
        }

        [TearDown]
        public void TearDown()
        {
            connection.Dispose();
        }

        [Test]
        public void throws_if_timeRecord_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => timeRecordRepository.Delete(null));
        }

        [Test]
        public void throws_if_id_is_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                TimeRecord timeRecord = CreateTimeRecordEntity();
                timeRecord.Id = 0;

                timeRecordRepository.Delete(timeRecord);
            });
        }

        [Test]
        public void throws_if_id_is_less_then_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                TimeRecord timeRecord = CreateTimeRecordEntity();
                timeRecord.Id = -1;

                timeRecordRepository.Delete(timeRecord);
            });
        }

        [Test]
        public void deletes_the_sigle_record_from_database()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecordRepository.Add(timeRecord);

            timeRecordRepository.Delete(timeRecord);

            DbAssert.AssertDoesNotExistAnyTimeRecord();
        }

        [Test]
        public void if_two_records_in_db_the_deleted_one_does_not_exist()
        {
            TimeRecord timeRecord1 = CreateTimeRecordEntity();
            timeRecord1.Date = new DateTime(2011, 06, 13);
            TimeRecord timeRecord2 = CreateTimeRecordEntity();
            timeRecord2.Date = new DateTime(2013, 06, 13);
            timeRecordRepository.Add(timeRecord1);
            timeRecordRepository.Add(timeRecord2);

            timeRecordRepository.Delete(timeRecord1);

            DbAssert.AssertDoesNotExistTimeRecord(timeRecord1.Id);
        }

        [Test]
        public void if_two_records_in_db_the_not_deleted_one_remains()
        {
            TimeRecord timeRecord1 = CreateTimeRecordEntity();
            timeRecord1.Date = new DateTime(2011, 06, 13);
            TimeRecord timeRecord2 = CreateTimeRecordEntity();
            timeRecord2.Date = new DateTime(2013, 06, 13);
            timeRecordRepository.Add(timeRecord1);
            timeRecordRepository.Add(timeRecord2);

            timeRecordRepository.Delete(timeRecord1);

            DbAssert.AssertExistsTimeRecordEqualTo(timeRecord2);
        }

        [Test]
        public void throws_if_id_does_not_exist()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                TimeRecord timeRecord = CreateTimeRecordEntity();
                timeRecord.Id = 10000;

                timeRecordRepository.Delete(timeRecord);
            });
        }

        private static TimeRecord CreateTimeRecordEntity()
        {
            return new TimeRecord
            {
                Id = 0,
                Date = new DateTime(2014, 04, 30),
                StartTime = new TimeSpan(1, 1, 1),
                EndTime = new TimeSpan(2, 2, 2),
                RecordType = TimeRecordType.Fake
            };
        }
    }
}
