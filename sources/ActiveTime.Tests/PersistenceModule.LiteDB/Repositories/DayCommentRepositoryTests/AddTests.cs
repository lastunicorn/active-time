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
using DustInTheWind.ActiveTime.PersistenceModule.LiteDB.Repositories;
using DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Helpers;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.LiteDB.Repositories.DayCommentRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class AddTests
    {
        private DayCommentRepository dayCommentRepository;
        private LiteDatabase database;

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
        public void sets_the_id_of_the_DayComment_entity()
        {
            DayComment dayComment = CreateDayCommentEntity();

            dayCommentRepository.Add(dayComment);

            Assert.That(dayComment.Id, Is.Not.EqualTo(0));
        }

        [Test]
        public void saves_the_DayComment_in_the_database()
        {
            DayComment dayComment = CreateDayCommentEntity();

            dayCommentRepository.Add(dayComment);

            DbAssert.AssertDayCommentCount(1, x => x.Id == dayComment.Id);
        }

        [Test]
        public void saves_two_DayComments_in_the_database()
        {
            DayComment dayComment1 = CreateDayCommentEntity();
            dayComment1.Date = new DateTime(2014, 06, 13);
            DayComment dayComment2 = CreateDayCommentEntity();
            dayComment2.Date = new DateTime(2014, 03, 05);

            dayCommentRepository.Add(dayComment1);
            dayCommentRepository.Add(dayComment2);

            DbAssert.AssertDayCommentCount(1, x => x.Id == dayComment1.Id);
            DbAssert.AssertDayCommentCount(1, x => x.Id == dayComment2.Id);
        }

        [Test]
        public void throws_if_two_identical_DayComments_are_saved()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DayComment dayComment1 = CreateDayCommentEntity();
                DayComment dayComment2 = CreateDayCommentEntity();

                dayCommentRepository.Add(dayComment1);
                dayCommentRepository.Add(dayComment2);
            });
        }

        [Test]
        public void correctly_adds_all_the_fields()
        {
            DayComment dayComment = CreateDayCommentEntity();

            dayCommentRepository.Add(dayComment);

            DbAssert.AssertExistsDayCommentEqualTo(dayComment);
        }

        private static DayComment CreateDayCommentEntity()
        {
            return new DayComment
            {
                Id = 0,
                Date = new DateTime(2014, 04, 30),
                Comment = "This is a comment"
            };
        }
    }
}
