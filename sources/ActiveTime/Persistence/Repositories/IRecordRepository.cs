namespace DustInTheWind.ActiveTime.Persistence
{
    internal interface IRecordRepository
    {
        void Add(Record record);
        void Update(Record record);
        void Delete(Record record);
        Record GetById(int id);
        RecordList GetAll();
    }
}