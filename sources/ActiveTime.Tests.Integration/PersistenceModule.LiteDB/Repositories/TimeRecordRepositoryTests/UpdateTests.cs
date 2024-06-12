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
using System.Diagnostics.CodeAnalysis;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Repositories;
using DustInTheWind.ActiveTime.Ports.Persistence;
using DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.LiteDB.Helpers;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.LiteDB.Repositories.TimeRecordRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class UpdateTests
    {
        private LiteDatabase database;
        private TimeRecordRepository timeRecordRepository;

        [SetUp]
        public void SetUp()
        {
            DbTestHelper.ClearDatabase();

            database = new LiteDatabase(DbTestHelper.ConnectionString);
            timeRecordRepository = new TimeRecordRepository(database);
        }

        [TearDown]
        public void TearDown()
        {
            database.Dispose();
        }

        [Test]
        public void throws_if_received_entity_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => timeRecordRepository.Update(null));
        }

        [Test]
        public void throws_if_id_is_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                TimeRecord timeRecord = CreateTimeRecordEntity();
                timeRecord.Id = 0;

                timeRecordRepository.Update(timeRecord);
            });
        }

        [Test]
        public void throws_if_id_is_less_then_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                TimeRecord timeRecord = CreateTimeRecordEntity();
                timeRecord.Id = -1;

                timeRecordRepository.Update(timeRecord);
            });
        }

        [Test]
        public void throws_if_id_does_not_exist()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                TimeRecord timeRecord = CreateTimeRecordEntity();
                timeRecord.Id = 10000;

                timeRecordRepository.Update(timeRecord);
            });
        }

        [Test]
        public void Date_is_updated_correctly()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecordRepository.Add(timeRecord);
            timeRecord.Date = new DateTime(2018, 05, 02);

            timeRecordRepository.Update(timeRecord);

            DbAssert.AssertExistsTimeRecordEqualTo(timeRecord);
        }

        [Test]
        public void StartTime_is_updated_correctly()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecordRepository.Add(timeRecord);
            timeRecord.StartTime = new TimeSpan(10, 10, 10);

            timeRecordRepository.Update(timeRecord);

            DbAssert.AssertExistsTimeRecordEqualTo(timeRecord);
        }

        [Test]
        public void EndTime_is_updated_correctly()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecordRepository.Add(timeRecord);
            timeRecord.EndTime = new TimeSpan(10, 10, 10);

            timeRecordRepository.Update(timeRecord);

            DbAssert.AssertExistsTimeRecordEqualTo(timeRecord);
        }

        [Test]
        public void RecordType_is_updated_correctly()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecordRepository.Add(timeRecord);
            timeRecord.RecordType = TimeRecordType.Fake;

            timeRecordRepository.Update(timeRecord);

            DbAssert.AssertExistsTimeRecordEqualTo(timeRecord);
        }

        [Test]
        public void record_that_was_not_updated_is_not_changed()
        {
            TimeRecord timeRecord1 = CreateTimeRecordEntity();
            timeRecord1.Date = new DateTime(2018, 06, 13);
            TimeRecord timeRecord2 = CreateTimeRecordEntity();
            timeRecord1.Date = new DateTime(2020, 06, 13);
            timeRecordRepository.Add(timeRecord1);
            timeRecordRepository.Add(timeRecord2);
            timeRecord1.RecordType = TimeRecordType.Fake;

            timeRecordRepository.Update(timeRecord1);

            DbAssert.AssertExistsTimeRecordEqualTo(timeRecord2);
        }

        private static TimeRecord CreateTimeRecordEntity()
        {
            return new TimeRecord
            {
                Id = 0,
                Date = new DateTime(2014, 04, 30),
                StartTime = new TimeSpan(1, 1, 1),
                EndTime = new TimeSpan(2, 2, 2),
                RecordType = TimeRecordType.Normal
            };
        }
    }
}
