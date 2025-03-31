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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Module.Repositories;
using LiteDB;

namespace DustInTheWind.ActiveTime.Persistence.LiteDB.Module
{
    public class UnitOfWork : IUnitOfWork
    {
        public const string ConnectionString = Constants.DatabaseFileName;

        private bool isDisposed;

        private LiteDatabase database;

        private TimeRecordRepository timeRecordRepository;
        public ITimeRecordRepository TimeRecordRepository
        {
            get
            {
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(UnitOfWork));

                if (timeRecordRepository == null)
                    timeRecordRepository = new TimeRecordRepository(database);

                return timeRecordRepository;
            }
        }

        private DateRecordRepository dateRecordRepository;
        public IDateRecordRepository DateRecordRepository
        {
            get
            {
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(UnitOfWork));

                if (dateRecordRepository == null)
                    dateRecordRepository = new DateRecordRepository(database);

                return dateRecordRepository;
            }
        }

        public UnitOfWork()
        {
            database = new LiteDatabase(ConnectionString);
            database.BeginTrans();
        }

        public void Commit()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            if (database != null)
            {
                database.Commit();
                database.Dispose();
                database = null;
            }
        }

        public void Rollback()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            if (database != null)
            {
                database.Rollback();
                database.Dispose();
                database = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                if (database != null)
                {
                    database.Dispose();
                    database = null;
                }
            }

            isDisposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
