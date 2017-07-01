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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.PersistenceModule.LiteDB.Repositories;
using DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Helpers;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Repositories.DayCommentRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class DeleteTests
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
        public void throws_if_DayComment_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => dayCommentRepository.Delete(null));
        }

        [Test]
        public void throws_if_id_is_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DayComment dayComment = CreateDayCommentEntity();
                dayComment.Id = 0;

                dayCommentRepository.Delete(dayComment);
            });
        }

        [Test]
        public void throws_if_id_is_less_then_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DayComment dayComment = CreateDayCommentEntity();
                dayComment.Id = -1;

                dayCommentRepository.Delete(dayComment);
            });
        }

        [Test]
        public void deletes_the_sigle_record_from_database()
        {
            DayComment dayComment = CreateDayCommentEntity();
            dayCommentRepository.Add(dayComment);

            dayCommentRepository.Delete(dayComment);

            DbAssert.AssertDayCommentCount(0);
            //DbAssert.AssertDoesNotExistAnyDayComment();
        }

        [Test]
        public void if_two_records_in_db_the_deleted_one_does_not_exist()
        {
            DayComment dayComment1 = CreateDayCommentEntity();
            dayComment1.Date = new DateTime(2011, 06, 13);
            DayComment dayComment2 = CreateDayCommentEntity();
            dayComment2.Date = new DateTime(2013, 06, 13);
            dayCommentRepository.Add(dayComment1);
            dayCommentRepository.Add(dayComment2);

            dayCommentRepository.Delete(dayComment1);

            DbAssert.AssertDayCommentCount(0, x=>x.Id == dayComment1.Id);
            //DbAssert.AssertDoesNotExistDayComment(dayComment1.Id);
        }

        [Test]
        public void if_two_records_in_db_the_not_deleted_one_remains()
        {
            DayComment dayComment1 = CreateDayCommentEntity();
            dayComment1.Date = new DateTime(2011, 06, 13);
            DayComment dayComment2 = CreateDayCommentEntity();
            dayComment2.Date = new DateTime(2013, 06, 13);
            dayCommentRepository.Add(dayComment1);
            dayCommentRepository.Add(dayComment2);

            dayCommentRepository.Delete(dayComment1);

            DbAssert.AssertExistsDayCommentEqualTo(dayComment2);
        }

        [Test]
        public void throws_if_id_does_not_exist()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DayComment dayComment = CreateDayCommentEntity();
                dayComment.Id = 10000;

                dayCommentRepository.Delete(dayComment);
            });
        }

        private static DayComment CreateDayCommentEntity()
        {
            return new DayComment
            {
                Id = 0,
                Date = new DateTime(2014, 04, 30),
                Comment = "This is a nice comment"
            };
        }
    }
}
