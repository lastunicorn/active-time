using System;
using DustInTheWind.ActiveTime.Goose;

namespace DustInTheWind.ActiveTime
{
    public class DayRecord
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private Record[] records;

        public Record[] Records
        {
            get { return records; }
            set { records = value; }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public bool IsEmpty
        {
            get { return (records == null || records.Length == 0) && (comment == null || comment.Length == 0); }
        }

        public bool HasRecords
        {
            get { return records != null && records.Length > 0; }
        }

        public bool HasComment
        {
            get { return comment != null && comment.Length > 0; }
        }

        public TimeSpan GetTotalTime()
        {
            TimeSpan totalTime = TimeSpan.Zero;

            if (records != null)
            {
                foreach (Record record in records)
                {
                    totalTime += record.EndTime - record.StartTime;
                }
            }

            return totalTime;
        }

        public TimeSpan GetIntervalTime()
        {
            TimeSpan totalTime = TimeSpan.Zero;

            if (records != null)
            {
                TimeSpan beginHour = records[0].StartTime;
                TimeSpan endHour = records[0].EndTime;

                foreach (Record record in records)
                {
                    if (record.StartTime < beginHour)
                        beginHour = record.StartTime;

                    if (record.EndTime > endHour)
                        endHour = record.EndTime;
                }

                totalTime = endHour - beginHour;
            }

            return totalTime;
        }

        public TimeSpan? GetBeginTime()
        {
            if (records != null && records.Length > 0)
            {
                return records[0].StartTime;
            }
            else
            {
                return null;
            }
        }
    }
}
