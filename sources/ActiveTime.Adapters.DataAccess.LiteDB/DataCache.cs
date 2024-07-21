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
using DustInTheWind.ActiveTime.Domain;

namespace DustInTheWind.ActiveTime.Persistence.LiteDB;

internal class DataCache
{
    private Dictionary<int, TimeRecord> TimeRecords { get; } = new();

    private Dictionary<int, DateRecord> DateRecords { get; } = new();

    public void Add(TimeRecord timeRecord)
    {
        if (timeRecord == null) throw new ArgumentNullException(nameof(timeRecord));

        TimeRecords.Add(timeRecord.Id, timeRecord);
    }

    public void Add<T>(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        switch (entity)
        {
            case TimeRecord timeRecord:
                TimeRecords.Add(timeRecord.Id, timeRecord);
                break;

            case DateRecord dateRecord:
                DateRecords.Add(dateRecord.Id, dateRecord);
                break;
        }
    }

    public TimeRecord GetTimeRecord(int id)
    {
        bool exists = TimeRecords.TryGetValue(id, out TimeRecord timeRecord);

        return exists
            ? timeRecord
            : null;
    }

    public T Get<T>(int id)
    {
        if (typeof(T) == typeof(TimeRecord))
        {
            bool exists = TimeRecords.TryGetValue(id, out TimeRecord timeRecord);

            return exists
                ? (T)(object)timeRecord
                : default;
        }

        if (typeof(T) == typeof(DateRecord))
        {
            bool exists = DateRecords.TryGetValue(id, out DateRecord dateRecord);

            return exists
                ? (T)(object)dateRecord
                : default;
        }

        return default;
    }

    public void Delete<T>(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        switch (entity)
        {
            case TimeRecord timeRecord:
                TimeRecords.Remove(timeRecord.Id);
                break;

            case DateRecord dateRecord:
                DateRecords.Remove(dateRecord.Id);
                break;
        }
    }
}