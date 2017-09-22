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
using System.Data.Common;
using System.Data.SQLite;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.PersistenceModule.SQLite.AdoNet.Repositories;

namespace DustInTheWind.ActiveTime.PersistenceModule.SQLite.AdoNet
{
    public class UnitOfWork : IUnitOfWork
    {
        public static string ConnectionString { get; set; } = "Data Source=db.s3db";

        private DbConnection connection;
        private DbTransaction transaction;

        private TimeRecordRepository timeRecordRepository;
        public ITimeRecordRepository TimeRecordRepository
        {
            get
            {
                if (timeRecordRepository == null)
                {
                    OpenDatabaseAndTransaction();
                    timeRecordRepository = new TimeRecordRepository(connection);
                }

                return timeRecordRepository;
            }
        }

        private DayCommentRepository dayCommentRepository;
        public IDayCommentRepository DayCommentRepository
        {
            get
            {
                if (dayCommentRepository == null)
                {
                    OpenDatabaseAndTransaction();
                    dayCommentRepository = new DayCommentRepository(connection);
                }

                return dayCommentRepository;
            }
        }
        
        private void OpenDatabaseAndTransaction()
        {
            if (disposed)
                throw new ObjectDisposedException("UnitOfWork");

            if (connection == null)
            {
                connection = new SQLiteConnection(ConnectionString);
                connection.Open();
            }

            if (transaction == null)
                transaction = connection.BeginTransaction();
        }

        public void Commit()
        {
            if (disposed)
                throw new ObjectDisposedException("UnitOfWork");

            if (transaction == null)
                return;

            transaction.Commit();

            transaction.Dispose();
            transaction = null;
        }

        public void Rollback()
        {
            if (disposed)
                throw new ObjectDisposedException("UnitOfWork");

            if (transaction == null)
                return;

            transaction.Rollback();

            transaction.Dispose();
            transaction = null;
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                    transaction = null;
                }

                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }

            disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
