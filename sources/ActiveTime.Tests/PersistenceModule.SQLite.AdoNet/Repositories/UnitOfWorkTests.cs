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
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.PersistenceModule.SQLite.AdoNet;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.SQLite.AdoNet.Repositories
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
            InsertOneTimeRecord(unitOfWork);

            DbAssert.AssertDoesNotExistAnyTimeRecord();
        }

        [Test]
        public void no_record_exists_if_disposed_without_commit()
        {
            InsertOneTimeRecord(unitOfWork);
            unitOfWork.Dispose();

            DbAssert.AssertDoesNotExistAnyTimeRecord();
        }

        [Test]
        public void no_record_exists_if_rolledback()
        {
            InsertOneTimeRecord(unitOfWork);
            unitOfWork.Rollback();

            DbAssert.AssertDoesNotExistAnyTimeRecord();
        }

        [Test]
        public void one_record_exists_if_commited()
        {
            InsertOneTimeRecord(unitOfWork);
            unitOfWork.Commit();

            DbAssert.AssertExistsAnyTimeRecord();
        }

        [Test]
        public void two_records_exists_if_commited()
        {
            InsertOneTimeRecord(unitOfWork);
            InsertSecondTimeRecord(unitOfWork);
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

                DbConnection connection = unitOfWork.Connection;
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

        private static void InsertOneTimeRecord(UnitOfWork unitOfWork)
        {
            string sql = string.Format("insert into records(date,start_time,end_time,type) values('{0}', '{1}', '{2}', {3})",
                new DateTime(2014, 04, 30).ToString("yyyy-MM-dd"),
                new TimeSpan(1, 1, 1).ToString(@"hh\:mm\:ss"),
                new TimeSpan(2, 2, 2).ToString(@"hh\:mm\:ss"),
                (int)TimeRecordType.Normal);

            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        private static void InsertSecondTimeRecord(UnitOfWork unitOfWork)
        {
            string sql = string.Format("insert into records(date,start_time,end_time,type) values('{0}', '{1}', '{2}', {3})",
                new DateTime(2014, 05, 01).ToString("yyyy-MM-dd"),
                new TimeSpan(1, 1, 1).ToString(@"hh\:mm\:ss"),
                new TimeSpan(2, 2, 2).ToString(@"hh\:mm\:ss"),
                (int)TimeRecordType.Normal);

            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
    }
}
