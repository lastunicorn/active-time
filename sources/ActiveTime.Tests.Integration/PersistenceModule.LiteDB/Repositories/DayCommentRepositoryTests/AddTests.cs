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
using DustInTheWind.ActiveTime.Adapters.DataAccess.LiteDB.Repositories;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.LiteDB.Helpers;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.LiteDB.Repositories.DayCommentRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class AddTests
    {
        private DateRecordRepository dateRecordRepository;
        private LiteDatabase database;

        [SetUp]
        public void SetUp()
        {
            DbTestHelper.ClearDatabase();

            database = new LiteDatabase(DbTestHelper.ConnectionString);
            dateRecordRepository = new DateRecordRepository(database);
        }

        [TearDown]
        public void TearDown()
        {
            database.Dispose();
        }

        [Test]
        public void sets_the_id_of_the_DayComment_entity()
        {
            DateRecord dateRecord = CreateDayCommentEntity();

            dateRecordRepository.Add(dateRecord);

            Assert.That(dateRecord.Id, Is.Not.EqualTo(0));
        }

        [Test]
        public void saves_the_DayComment_in_the_database()
        {
            DateRecord dateRecord = CreateDayCommentEntity();

            dateRecordRepository.Add(dateRecord);

            DbAssert.AssertDayCommentCount(1, x => x.Id == dateRecord.Id);
        }

        [Test]
        public void saves_two_DayComments_in_the_database()
        {
            DateRecord dateComment1 = CreateDayCommentEntity();
            dateComment1.Date = new DateTime(2014, 06, 13);
            DateRecord dateComment2 = CreateDayCommentEntity();
            dateComment2.Date = new DateTime(2014, 03, 05);

            dateRecordRepository.Add(dateComment1);
            dateRecordRepository.Add(dateComment2);

            DbAssert.AssertDayCommentCount(1, x => x.Id == dateComment1.Id);
            DbAssert.AssertDayCommentCount(1, x => x.Id == dateComment2.Id);
        }

        [Test]
        public void throws_if_two_identical_DayComments_are_saved()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DateRecord dateComment1 = CreateDayCommentEntity();
                DateRecord dateComment2 = CreateDayCommentEntity();

                dateRecordRepository.Add(dateComment1);
                dateRecordRepository.Add(dateComment2);
            });
        }

        [Test]
        public void correctly_adds_all_the_fields()
        {
            DateRecord dateRecord = CreateDayCommentEntity();

            dateRecordRepository.Add(dateRecord);

            DbAssert.AssertExistsDayCommentEqualTo(dateRecord);
        }

        private static DateRecord CreateDayCommentEntity()
        {
            return new DateRecord
            {
                Id = 0,
                Date = new DateTime(2014, 04, 30),
                Comment = "This is a comment"
            };
        }
    }
}
