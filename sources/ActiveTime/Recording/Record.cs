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
    /// <summary>
    /// Represents an interval of Time within a day.
    /// </summary>
    public class Record
    {
        /// <summary>
        /// The Date for which this record is created.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Gets the Date for which this record is created.
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }

        /// <summary>
        /// The Time of day representing the start Time.
        /// </summary>
        private TimeSpan startTime;

        /// <summary>
        /// Gets the Time of day representing the start Time.
        /// </summary>
        public TimeSpan StartTime
        {
            get { return startTime; }
        }

        /// <summary>
        /// The Time of day representing the end Time.
        /// </summary>
        private TimeSpan endTime;

        /// <summary>
        /// Gets the Time of day representing the end Time.
        /// </summary>
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> class.
        /// </summary>
        /// <param name="Date">The Date for which this record is created.</param>
        /// <param name="startTime">The Time of day representing the start Time.</param>
        public Record(DateTime date, TimeSpan startTime)
            : this(date, startTime, startTime)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> class.
        /// </summary>
        /// <param name="Date">The Date for which this record is created.</param>
        /// <param name="startTime">The Time of day representing the start Time.</param>
        /// <param name="endTime">The Time of day representing the end Time.</param>
        public Record(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            this.date = date;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        #endregion
    }
}
