using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.Recording;
using System;

namespace DustInTheWind.ActiveTime.Persistence
{
    public interface IRecordRepository
    {
        void Add(Record record);
        void Update(Record record);
        void Delete(Record record);
        Record GetById(int id);
        DayRecord GetDayRecord(DateTime date);
        RecordList GetAll();

    }
}