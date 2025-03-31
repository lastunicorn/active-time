﻿// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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
using System.Collections.Generic;

namespace DustInTheWind.ActiveTime.Common
{
    /// <summary>
    /// Represents an interval of time within a day.
    /// </summary>
    public class DateRecord
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

        public List<TimeRecord> TimeRecords { get; set; }

        /// <summary>
        /// Compares the business keys.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            return obj is DateRecord dayComment && Date == dayComment.Date;
        }

        public override string ToString()
        {
            string shortDateString = Date.ToShortDateString();
            return $"{shortDateString} - {Comment}";
        }
    }
}
