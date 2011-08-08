namespace DustInTheWind.ActiveTime.Persistence
{
    internal interface ICommentRepository
    {
        void Add(DayComment comment);
        void Update(DayComment comment);
        void Delete(DayComment comment);
        DayComment GetById(int id);
        DayCommentList GetAll();
    }
}