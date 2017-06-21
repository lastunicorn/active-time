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

namespace DustInTheWind.ActiveTime.Common.Persistence
{
    /// <summary>
    /// Represents an interval of time within a day.
    /// </summary>
    public class DayComment
    {
        /// <summary>
        /// Gets or sets an integer value that uniquely identifies the comment.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date for which this comment is created.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        public string Comment { get; set; }
        
        /// <summary>
        /// Compares the business keys.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            DayComment dayComment = obj as DayComment;

            return dayComment != null && Date == dayComment.Date;
        }
    }
}
