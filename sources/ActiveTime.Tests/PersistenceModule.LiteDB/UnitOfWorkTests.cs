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
using DustInTheWind.ActiveTime.PersistenceModule.LiteDB;
using DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Helpers;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class UnitOfWorkTests
    {
        private UnitOfWork unitOfWork;

        [SetUp]
        public void SetUp()
        {
            DbTestHelper.ClearDatabase();

            unitOfWork = new UnitOfWork();
        }

        [TearDown]
        public void TearDown()
        {
            unitOfWork.Dispose();
        }

        [Test]
        public void no_record_exists_if_not_commited()
        {
            InsertOneRecord();

            DbAssert.AssertTimeRecordCount(0);
        }

        [Test]
        public void no_record_exists_if_disposed_without_commit()
        {
            InsertOneRecord();
            unitOfWork.Dispose();

            DbAssert.AssertTimeRecordCount(0);
        }

        [Test]
        public void no_record_exists_if_rolledback()
        {
            InsertOneRecord();
            unitOfWork.Rollback();

            DbAssert.AssertTimeRecordCount(0);
        }

        [Test]
        public void one_record_exists_if_commited()
        {
            TimeRecord timeRecord = InsertOneRecord();
            unitOfWork.Commit();

            DbAssert.AssertTimeRecordCount(1, x => x.Id == timeRecord.Id);
        }

        [Test]
        public void two_records_exists_if_commited()
        {
            InsertOneRecord();
            InsertSecondRecord();
            unitOfWork.Commit();

            DbAssert.AssertTimeRecordCount(2);
        }

        [Test]
        public void no_exception_is_thrown_if_Dispose_is_called_twice()
        {
            unitOfWork.Dispose();
            unitOfWork.Dispose();
        }

        [Test]
        public void throws_if_using_connection_after_dispose()
        {
            Assert.Throws<ObjectDisposedException>(() =>
            {
                unitOfWork.Dispose();

                ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;
            });
        }

        [Test]
        public void nothing_happens_if_Commit_is_called_without_using_the_connection_first()
        {
            unitOfWork.Commit();
        }

        [Test]
        public void nothing_happens_if_Rollback_is_called_without_using_the_connection_first()
        {
            unitOfWork.Rollback();
        }

        [Test]
        public void throws_if_Commit_is_called_after_dispose()
        {
            Assert.Throws<ObjectDisposedException>(() =>
            {
                unitOfWork.Dispose();

                unitOfWork.Commit();
            });
        }

        [Test]
        public void throws_if_Rollback_is_called_after_dispose()
        {
            Assert.Throws<ObjectDisposedException>(() =>
            {
                unitOfWork.Dispose();

                unitOfWork.Rollback();
            });
        }

        private TimeRecord InsertOneRecord()
        {
            ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;
            
            TimeRecord timeRecord = CreateTimeRecordEntity();
            timeRecordRepository.Add(timeRecord);

            return timeRecord;
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

        private void InsertSecondRecord()
        {
            ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;

            TimeRecord timeRecord = CreateSecondTimeRecordEntity();
            timeRecordRepository.Add(timeRecord);
        }

        private static TimeRecord CreateSecondTimeRecordEntity()
        {
            return new TimeRecord
            {
                Id = 0,
                Date = new DateTime(2014, 05, 01),
                StartTime = new TimeSpan(1, 1, 1),
                EndTime = new TimeSpan(2, 2, 2),
                RecordType = TimeRecordType.Normal
            };
        }
    }
}
