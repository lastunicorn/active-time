// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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

namespace DustInTheWind.ActiveTime.Recording
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
