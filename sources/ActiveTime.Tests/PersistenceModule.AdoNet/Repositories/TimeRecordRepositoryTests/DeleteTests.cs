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
using System.Diagnostics.CodeAnalysis;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.PersistenceModule.AdoNet.Repositories;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.AdoNet.Repositories.TimeRecordRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class DeleteTests
    {
        private UnitOfWork unitOfWork;
        private TimeRecordRepository timeRecordRepository;

        [SetUp]
        public void SetUp()
        {
            DbTestHelper.ClearDatabase();

            unitOfWork = new UnitOfWork();
            timeRecordRepository = new TimeRecordRepository(unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            unitOfWork.Dispose();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void throws_if_timeRecord_is_null()
        {
            timeRecordRepository.Delete(null);
        }

        [Test]
        [ExpectedException(typeof(PersistenceException))]
        public void throws_if_id_is_zero()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecord.Id = 0;

            timeRecordRepository.Delete(timeRecord);
        }

        [Test]
        [ExpectedException(typeof(PersistenceException))]
        public void throws_if_id_is_less_then_zero()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecord.Id = -1;

            timeRecordRepository.Delete(timeRecord);
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
        [ExpectedException(typeof(PersistenceException))]
        public void throws_if_id_does_not_exist()
        {
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecord.Id = 10000;

            timeRecordRepository.Delete(timeRecord);
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
