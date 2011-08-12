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
    /// Represents an interval of time inside a day.
    /// </summary>
    public class DayTimeInterval
    {
        /// <summary>
        /// The lower bound of the time interval.
        /// </summary>
        private TimeSpan startTime;

        /// <summary>
        /// Gets or sets the lower bound of the time interval.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is less then zero or greater or equal to 24 hours.</exception>
        public TimeSpan StartTime
        {
            get { return startTime; }
            set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromHours(24))
                    throw new ArgumentOutOfRangeException("value", "The start time of the day time interval should be a value greater or equal to 0 and less then 24 hours.");

                startTime = value;
            }
        }

        /// <summary>
        /// The upper bound of the time interval.
        /// </summary>
        private TimeSpan endTime;

        /// <summary>
        /// Gets or sets the upper bound of the time interval.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is less then zero or greater or equal to 24 hours.</exception>
        public TimeSpan EndTime
        {
            get { return endTime; }
            set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromHours(24))
                    throw new ArgumentOutOfRangeException("endTime", "The end time of the day time interval should be a value greater or equal to 0 and less then 24 hours.");

                endTime = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTimeInterval"/> class.
        /// </summary>
        /// <param name="startTime">The lower bound of the time interval.</param>
        /// <param name="endTime">The upper bound of the time interval.</param>
        /// <exception cref="ArgumentOutOfRangeException">One of the limits of the interval is less then zero or greater or equal to 24 hours.</exception>
        public DayTimeInterval(TimeSpan startTime, TimeSpan endTime)
        {
            if (startTime < TimeSpan.Zero || startTime >= TimeSpan.FromHours(24))
                throw new ArgumentOutOfRangeException("startTime", "The start time of the day time interval should be a value greater or equal to 0 and less then 24 hours.");

            if (endTime < TimeSpan.Zero || endTime >= TimeSpan.FromHours(24))
                throw new ArgumentOutOfRangeException("endTime", "The end time of the day time interval should be a value greater or equal to 0 and less then 24 hours.");

            this.startTime = startTime;
            this.endTime = endTime;
        }

        /// <summary>
        /// Calculates and returns the time between <see cref="StartTime"/> and <see cref="EndTime"/>.
        /// </summary>
        /// <returns>A <see cref="TimeSpan"/> instance representing the time between <see cref="StartTime"/> and <see cref="EndTime"/></returns>
        public TimeSpan GetTimeSpan()
        {
            return endTime - startTime;
        }
    }
}
