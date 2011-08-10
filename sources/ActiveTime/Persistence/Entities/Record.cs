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

namespace DustInTheWind.ActiveTime.Persistence.Entities
{
    /// <summary>
    /// Represents an interval of time within a day.
    /// </summary>
    public class Record
    {
        public int Id { get; set; }
    
        /// <summary>
        /// Gets or sets the date for which this record is created.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the time of day representing the start time.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time of day representing the end time.
        /// </summary>
        public TimeSpan EndTime { get; set; }

        public RecordType RecordType { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Record);
        }

        public bool Equals(Record record)
        {
            return record != null && Id == record.Id;
        }
    }
}
