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
using System.Text;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.PersistenceModule.Entities
{
    /// <summary>
    /// Represents an interval of time within a day.
    /// </summary>
    public class TimeRecordEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the date for which this record is created.
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the time of day representing the start time.
        /// This value should be greater or equal to 0 and less then one day.
        /// </summary>
        public virtual TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time of day representing the end time.
        /// This value should be greater or equal to 0 and less then one day.
        /// </summary>
        public virtual TimeSpan EndTime { get; set; }

        /// <summary>
        /// Gets or sets the type of the time record. This type specifies if the record was normally created or was created manually.
        /// </summary>
        public virtual TimeRecordType RecordType { get; set; }

        /// <summary>
        /// Compares the business keys.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            TimeRecordEntity timeRecord = obj as TimeRecordEntity;

            return timeRecord != null && Date == timeRecord.Date && StartTime == timeRecord.StartTime && EndTime == timeRecord.EndTime;
        }

        public override Object ToObject()
        {
            return new TimeRecord
            {
                Date = Date,
                StartTime = StartTime,
                EndTime = EndTime,
                RecordType = RecordType
            };
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Date.ToShortDateString());
            sb.Append(" [");
            sb.Append(StartTime.ToString());
            sb.Append(" - ");
            sb.Append(EndTime.ToString());
            sb.Append("]");

            return sb.ToString();
        }
    }
}
