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

namespace DustInTheWind.ActiveTime.Common.Recording
{
    /// <summary>
    /// Represents an interval of time within a day.
    /// </summary>
    public class Break : DayTimeInterval
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Break"/> class.
        /// </summary>
        /// <param name="startTime">The time of day representing the start time.</param>
        public Break(TimeSpan startTime)
            : base(startTime, startTime)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Break"/> class.
        /// </summary>
        /// <param name="startTime">The time of day representing the start time.</param>
        /// <param name="endTime">The time of day representing the end time.</param>
        public Break(TimeSpan startTime, TimeSpan endTime)
            : base(startTime, endTime)
        {
        }

        #endregion
    }
}
