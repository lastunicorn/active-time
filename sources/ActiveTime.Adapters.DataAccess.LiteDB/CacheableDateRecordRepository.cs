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
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.Persistence;

namespace DustInTheWind.ActiveTime.Persistence.LiteDB;

internal class CacheableDateRecordRepository : IDateRecordRepository
{
    private readonly DataCache dataCache;
    private readonly IDateRecordRepository underlyingRepository;

    public CacheableDateRecordRepository(DataCache dataCache, IDateRecordRepository underlyingRepository)
    {
        this.dataCache = dataCache ?? throw new ArgumentNullException(nameof(dataCache));
        this.underlyingRepository = underlyingRepository ?? throw new ArgumentNullException(nameof(underlyingRepository));
    }

    public void Add(DateRecord dateRecord)
    {
        underlyingRepository.Add(dateRecord);
        dataCache.Add(dateRecord);
    }

    public void Update(DateRecord dateRecord)
    {
        underlyingRepository.Update(dateRecord);
    }

    public void AddOrUpdate(DateRecord dateRecord)
    {
        underlyingRepository.AddOrUpdate(dateRecord);
    }

    public void Delete(DateRecord dateRecord)
    {
        underlyingRepository.Delete(dateRecord);
        dataCache.Delete(dateRecord);
    }

    public DateRecord GetById(int id)
    {
        DateRecord dateRecord = dataCache.Get<DateRecord>(id);

        if (dateRecord == null)
        {
            dateRecord = underlyingRepository.GetById(id);
            dataCache.Add(dateRecord);
        }

        return dateRecord;
    }

    public DateRecord GetByDate(DateTime date)
    {
        DateRecord dateRecord = underlyingRepository.GetByDate(date);

        if (dateRecord == null)
            return null;

        DateRecord cachedDateRecord = dataCache.Get<DateRecord>(dateRecord.Id);

        if (cachedDateRecord == null)
            dataCache.Add(dateRecord);

        return dateRecord;
    }

    public List<DateRecord> GetByDate(DateTime startDate, DateTime endDate)
    {
        List<DateRecord> dateRecord = underlyingRepository.GetByDate(startDate, endDate);
        return new CacheableEnumerable<DateRecord>(dataCache, dateRecord).ToList();
    }

    public IList<DateRecord> GetAll()
    {
        IEnumerable<DateRecord> dateRecord = underlyingRepository.GetAll();
        return new CacheableEnumerable<DateRecord>(dataCache, dateRecord).ToList();
    }
}