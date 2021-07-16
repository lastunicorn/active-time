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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Module.Repositories;
using DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Helpers;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Repositories.DayCommentRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class UpdateTests
    {
        private LiteDatabase database;
        private DayCommentRepository dayCommentRepository;

        [SetUp]
        public void SetUp()
        {
            DbTestHelper.ClearDatabase();

            database = new LiteDatabase(DbTestHelper.ConnectionString);
            dayCommentRepository = new DayCommentRepository(database);
        }

        [TearDown]
        public void TearDown()
        {
            database.Dispose();
        }

        [Test]
        public void throws_if_received_entity_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => dayCommentRepository.Update(null));
        }

        [Test]
        public void throws_if_id_is_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DayRecord dayRecord = CreateDayCommentEntity();
                dayRecord.Id = 0;

                dayCommentRepository.Update(dayRecord);
            });
        }

        [Test]
        public void throws_if_id_is_less_then_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DayRecord dayRecord = CreateDayCommentEntity();
                dayRecord.Id = -1;

                dayCommentRepository.Update(dayRecord);
            });
        }

        [Test]
        public void throws_if_id_does_not_exist()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DayRecord dayRecord = CreateDayCommentEntity();
                dayRecord.Id = 10000;

                dayCommentRepository.Update(dayRecord);
            });
        }

        [Test]
        public void Date_is_updated_correctly()
        {
            DayRecord dayRecord = CreateDayCommentEntity();
            dayCommentRepository.Add(dayRecord);
            dayRecord.Date = new DateTime(2018, 05, 02);

            dayCommentRepository.Update(dayRecord);

            DbAssert.AssertExistsDayCommentEqualTo(dayRecord);
        }

        [Test]
        public void Comment_is_updated_correctly()
        {
            DayRecord dayRecord = CreateDayCommentEntity();
            dayCommentRepository.Add(dayRecord);
            dayRecord.Comment = "this comment is changed";

            dayCommentRepository.Update(dayRecord);

            DbAssert.AssertExistsDayCommentEqualTo(dayRecord);
        }

        [Test]
        public void record_that_was_not_updated_is_not_changed()
        {
            DayRecord dayComment1 = CreateDayCommentEntity();
            DayRecord dayComment2 = CreateDayCommentEntity();
            dayComment1.Date = new DateTime(2018, 06, 13);
            dayComment1.Date = new DateTime(2020, 06, 13);
            dayCommentRepository.Add(dayComment1);
            dayCommentRepository.Add(dayComment2);
            dayComment1.Comment = "this comment is changed";

            dayCommentRepository.Update(dayComment1);

            DbAssert.AssertExistsDayCommentEqualTo(dayComment2);
        }

        private static DayRecord CreateDayCommentEntity()
        {
            return new DayRecord
            {
                Id = 0,
                Date = new DateTime(2014, 04, 30),
                Comment = "This is a big comment"
            };
        }
    }
}
