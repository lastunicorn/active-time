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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.PersistenceModule.LiteDB.Repositories;
using LiteDB;

namespace DustInTheWind.ActiveTime.PersistenceModule.LiteDB
{
    public class UnitOfWork : IUnitOfWork
    {
        private const string ConnectionString = Constants.DatabaseFileName;

        private LiteDatabase database;
        private LiteTransaction transaction;

        private LiteDatabase OpenDatabaseAndTransaction()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            if (database == null)
                database = new LiteDatabase(ConnectionString);

            if (transaction == null)
                transaction = database.BeginTrans();

            return database;
        }

        private TimeRecordRepository timeRecordRepository;
        public ITimeRecordRepository TimeRecordRepository
        {
            get
            {
                if (timeRecordRepository == null)
                {
                    OpenDatabaseAndTransaction();
                    timeRecordRepository = new TimeRecordRepository(database);
                }

                return timeRecordRepository;
            }
        }

        public void Commit()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            if (transaction == null)
                return;

            transaction.Commit();

            transaction.Dispose();
            transaction = null;
        }

        public void Rollback()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

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

                if (database != null)
                {
                    database.Dispose();
                    database = null;
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
