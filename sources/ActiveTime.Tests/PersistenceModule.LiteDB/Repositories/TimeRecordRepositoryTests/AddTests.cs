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
using System.Diagnostics.CodeAnalysis;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Module.Repositories;
using DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Helpers;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Repositories.TimeRecordRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class AddTests
    {
        private TimeRecordRepository timeRecordRepository;
        private LiteDatabase database;

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
        public void sets_the_id_of_the_timeRecord_entity()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();

            timeRecordRepository.Add(timeRecord);

            Assert.That(timeRecord.Id, Is.Not.EqualTo(0));
        }

        [Test]
        public void saves_the_timeRecord_in_the_database()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();

            timeRecordRepository.Add(timeRecord);

            DbAssert.AssertTimeRecordCount(1, x => x.Id == timeRecord.Id);
        }

        [Test]
        public void saves_two_timeRecords_in_the_database()
        {
            TimeRecord timeRecord1 = CreateTimeRecordEntity();
            timeRecord1.Date = new DateTime(2014, 06, 13);
            TimeRecord timeRecord2 = CreateTimeRecordEntity();
            timeRecord2.Date = new DateTime(2014, 03, 05);

            timeRecordRepository.Add(timeRecord1);
            timeRecordRepository.Add(timeRecord2);

            DbAssert.AssertTimeRecordCount(1, x => x.Id == timeRecord1.Id);
            DbAssert.AssertTimeRecordCount(1, x => x.Id == timeRecord2.Id);
        }

        [Test]
        public void throws_if_two_identical_timeRecords_are_saved()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                TimeRecord timeRecord1 = CreateTimeRecordEntity();
                TimeRecord timeRecord2 = CreateTimeRecordEntity();

                timeRecordRepository.Add(timeRecord1);
                timeRecordRepository.Add(timeRecord2);
            });
        }

        [Test]
        public void correctly_adds_all_the_fields()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();

            timeRecordRepository.Add(timeRecord);

            DbAssert.AssertExistsTimeRecordEqualTo(timeRecord);
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
