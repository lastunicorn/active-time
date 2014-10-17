using System;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.Services
{
    public class DayReport
    {
        public DateTime Date { get; set; }

        public string Comment { get; set; }

        public DayReport(DayComment dayComment)
        {
            Date = dayComment.Date;
            Comment = dayComment.Comment;
        }
    }
}