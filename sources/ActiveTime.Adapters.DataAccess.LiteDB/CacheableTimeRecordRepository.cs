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

using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;

namespace DustInTheWind.ActiveTime.Adapters.DataAccess.LiteDB;

internal class CacheableTimeRecordRepository : ITimeRecordRepository
{
    private readonly DataCache dataCache;
    private readonly ITimeRecordRepository underlyingRepository;

    public CacheableTimeRecordRepository(DataCache dataCache, ITimeRecordRepository underlyingRepository)
    {
        this.dataCache = dataCache ?? throw new ArgumentNullException(nameof(dataCache));
        this.underlyingRepository = underlyingRepository ?? throw new ArgumentNullException(nameof(underlyingRepository));
    }

    public void Add(TimeRecord timeRecord)
    {
        underlyingRepository.Add(timeRecord);
        dataCache.Add(timeRecord);
    }

    public void Update(TimeRecord timeRecord)
    {
        underlyingRepository.Update(timeRecord);
    }

    public void Delete(TimeRecord timeRecord)
    {
        underlyingRepository.Delete(timeRecord);
        dataCache.Delete(timeRecord);
    }

    public TimeRecord GetById(int id)
    {
        TimeRecord timeRecord = dataCache.Get<TimeRecord>(id);

        if (timeRecord == null)
        {
            timeRecord = underlyingRepository.GetById(id);
            dataCache.Add(timeRecord);
        }

        return timeRecord;
    }

    public IEnumerable<TimeRecord> GetByDate(DateTime date)
    {
        IEnumerable<TimeRecord> timeRecords = underlyingRepository.GetByDate(date);
        return new CacheableEnumerable<TimeRecord>(dataCache, timeRecords);
    }

    public IEnumerable<TimeRecord> GetAll()
    {
        IEnumerable<TimeRecord> timeRecords = underlyingRepository.GetAll();
        return new CacheableEnumerable<TimeRecord>(dataCache, timeRecords);
    }
}