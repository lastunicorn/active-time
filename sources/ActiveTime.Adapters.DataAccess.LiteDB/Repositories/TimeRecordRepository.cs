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
using LiteDB;

namespace DustInTheWind.ActiveTime.Persistence.LiteDB.Repositories;

internal class TimeRecordRepository : ITimeRecordRepository
{
    public const string CollectionName = "TimeRecord";

    private readonly LiteDatabase database;

    public TimeRecordRepository(LiteDatabase database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public void Add(TimeRecord timeRecord)
    {
        ILiteCollection<TimeRecord> timeRecordCollection = database.GetCollection<TimeRecord>(CollectionName);

        bool exists = timeRecordCollection
            .Find(x =>
                x.Date == timeRecord.Date &&
                x.StartTime == timeRecord.StartTime &&
                x.EndTime == timeRecord.EndTime &&
                x.RecordType == timeRecord.RecordType)
            .Any();

        if (exists)
            throw new TimeRecordExistsException(timeRecord);

        timeRecordCollection.Insert(timeRecord);
    }

    public void Update(TimeRecord timeRecord)
    {
        if (timeRecord == null) throw new ArgumentNullException(nameof(timeRecord));

        if (timeRecord.Id <= 0)
            throw new PersistenceException("The id of the time record should be a positive integer.");

        ILiteCollection<TimeRecord> timeRecordCollection = database.GetCollection<TimeRecord>(CollectionName);

        bool exists = timeRecordCollection
            .Find(x => x.Id == timeRecord.Id)
            .Any();

        if (!exists)
            throw new PersistenceException("There is no record with the specified id to update.");

        timeRecordCollection.Update(timeRecord);
    }

    public void Delete(TimeRecord timeRecord)
    {
        if (timeRecord == null) throw new ArgumentNullException(nameof(timeRecord));

        if (timeRecord.Id <= 0)
            throw new NegativeEntityIdException(timeRecord.Id);

        ILiteCollection<TimeRecord> timeRecordCollection = database.GetCollection<TimeRecord>(CollectionName);

        timeRecordCollection.Delete(timeRecord.Id);
    }

    public TimeRecord GetById(int id)
    {
        ILiteCollection<TimeRecord> timeRecordCollection = database.GetCollection<TimeRecord>(CollectionName);
        return timeRecordCollection
            .Find(x => x.Id == id)
            .FirstOrDefault();
    }

    public IEnumerable<TimeRecord> GetByDate(DateTime date)
    {
        ILiteCollection<TimeRecord> timeRecordCollection = database.GetCollection<TimeRecord>(CollectionName);
        return timeRecordCollection
            .Find(x => x.Date == date);
    }

    public IEnumerable<TimeRecord> GetAll()
    {
        ILiteCollection<TimeRecord> timeRecordCollection = database.GetCollection<TimeRecord>(CollectionName);
        return timeRecordCollection
            .FindAll();
    }
}