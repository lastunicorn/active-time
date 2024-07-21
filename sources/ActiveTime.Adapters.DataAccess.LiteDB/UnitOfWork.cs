// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using System.Threading;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Repositories;
using DustInTheWind.ActiveTime.Ports.Persistence;
using LiteDB;

namespace DustInTheWind.ActiveTime.Persistence.LiteDB;

public class UnitOfWork : IUnitOfWork
{
    public const string ConnectionString = Constants.DatabaseFileName;

    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    private bool isDisposed;

    private readonly LiteDatabase database;
    private readonly DataCache dataCache;

    private ITimeRecordRepository timeRecordRepository;
    private IDateRecordRepository dateRecordRepository;

    public ITimeRecordRepository TimeRecordRepository
    {
        get
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            return timeRecordRepository ??= new CacheableTimeRecordRepository(dataCache, new TimeRecordRepository(database));
        }
    }

    public IDateRecordRepository DateRecordRepository
    {
        get
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            return dateRecordRepository ??= new CacheableDateRecordRepository(dataCache, new DateRecordRepository(database));
        }
    }

    public UnitOfWork()
    {
        Semaphore.Wait();

        try
        {
            dataCache = new DataCache();

            database = new LiteDatabase(ConnectionString);
            database.BeginTrans();
        }
        catch
        {
            Semaphore.Release();
            throw;
        }
    }

    public void Commit()
    {
        if (isDisposed)
            throw new ObjectDisposedException(nameof(UnitOfWork));

        database.Commit();
    }

    public void Rollback()
    {
        if (isDisposed)
            throw new ObjectDisposedException(nameof(UnitOfWork));

        database.Rollback();
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
            database.Dispose();
            Semaphore.Release();
        }

        isDisposed = true;
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}