using DustInTheWind.ActiveTime.Persistence.Entities;
using System;

namespace DustInTheWind.ActiveTime.Persistence
{
    internal interface ICommentRepository
    {
        void Add(DayComment comment);
        void Update(DayComment comment);
        void AddOrUpdate(DayComment comment);
        void AddOrUpdate(DateTime date, string comment);
        void Delete(DayComment comment);
        DayComment GetById(int id);
        DayComment GetByDate(DateTime date);
        DayCommentList GetAll();
    }
}