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
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Repositories;
using DustInTheWind.ActiveTime.Ports.Persistence;
using DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.LiteDB.Helpers;
using LiteDB;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.LiteDB.Repositories.DayCommentRepositoryTests
{
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class DeleteTests
    {
        private LiteDatabase database;
        private DateRecordRepository dateRecordRepository;

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
        public void throws_if_DayComment_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => dateRecordRepository.Delete(null));
        }

        [Test]
        public void throws_if_id_is_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DateRecord dateRecord = CreateDayCommentEntity();
                dateRecord.Id = 0;

                dateRecordRepository.Delete(dateRecord);
            });
        }

        [Test]
        public void throws_if_id_is_less_then_zero()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DateRecord dateRecord = CreateDayCommentEntity();
                dateRecord.Id = -1;

                dateRecordRepository.Delete(dateRecord);
            });
        }

        [Test]
        public void deletes_the_sigle_record_from_database()
        {
            DateRecord dateRecord = CreateDayCommentEntity();
            dateRecordRepository.Add(dateRecord);

            dateRecordRepository.Delete(dateRecord);

            DbAssert.AssertDayCommentCount(0);
            //DbAssert.AssertDoesNotExistAnyDayComment();
        }

        [Test]
        public void if_two_records_in_db_the_deleted_one_does_not_exist()
        {
            DateRecord dateComment1 = CreateDayCommentEntity();
            dateComment1.Date = new DateTime(2011, 06, 13);
            DateRecord dateComment2 = CreateDayCommentEntity();
            dateComment2.Date = new DateTime(2013, 06, 13);
            dateRecordRepository.Add(dateComment1);
            dateRecordRepository.Add(dateComment2);

            dateRecordRepository.Delete(dateComment1);

            DbAssert.AssertDayCommentCount(0, x=>x.Id == dateComment1.Id);
            //DbAssert.AssertDoesNotExistDayComment(dayComment1.Id);
        }

        [Test]
        public void if_two_records_in_db_the_not_deleted_one_remains()
        {
            DateRecord dateComment1 = CreateDayCommentEntity();
            dateComment1.Date = new DateTime(2011, 06, 13);
            DateRecord dateComment2 = CreateDayCommentEntity();
            dateComment2.Date = new DateTime(2013, 06, 13);
            dateRecordRepository.Add(dateComment1);
            dateRecordRepository.Add(dateComment2);

            dateRecordRepository.Delete(dateComment1);

            DbAssert.AssertExistsDayCommentEqualTo(dateComment2);
        }

        [Test]
        public void throws_if_id_does_not_exist()
        {
            Assert.Throws<PersistenceException>(() =>
            {
                DateRecord dateRecord = CreateDayCommentEntity();
                dateRecord.Id = 10000;

                dateRecordRepository.Delete(dateRecord);
            });
        }

        private static DateRecord CreateDayCommentEntity()
        {
            return new DateRecord
            {
                Id = 0,
                Date = new DateTime(2014, 04, 30),
                Comment = "This is a nice comment"
            };
        }
    }
}
