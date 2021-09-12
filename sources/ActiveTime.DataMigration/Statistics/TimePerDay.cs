// ActiveTime
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
using System.Linq;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Module;

namespace DustInTheWind.ActiveTime.DataMigration.Statistics
{
    internal class TimePerDay
    {
        private readonly UnitOfWork unitOfWork;

        private DateTime date;
        private TimeSpan time;
        private bool? isVacation;
        private bool? isInvalidTime;
        private string[] commentLines;

        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                isVacation = null;
            }
        }

        public TimeSpan Time
        {
            get => time;
            set
            {
                time = value;
                isVacation = null;
            }
        }

        public bool IsInvalidTime => isInvalidTime ?? CalculateIsInvalidTime();

        public bool IsVacation => isVacation ?? CalculateIsVacation();

        public bool IsHoliday => isVacation ?? CalculateIsHoliday();

        public bool IsWeekEnd => Date.IsWeekEnd();

        public TimePerDay(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        private string[] CommentLines
        {
            get
            {
                if (commentLines == null)
                {
                    DateRecord dateRecord = unitOfWork.DateRecordRepository.GetByDate(Date);
                    commentLines = dateRecord?.Comment?.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                }

                return commentLines;
            }
        }

        private bool CalculateIsVacation()
        {
            return CommentLines
                .Any(x => x.Equals("vacation:", StringComparison.OrdinalIgnoreCase));
        }

        private bool CalculateIsHoliday()
        {
            return CommentLines
                .Any(x => x.Equals("holiday:", StringComparison.OrdinalIgnoreCase) ||
                          x.Equals("free day:", StringComparison.OrdinalIgnoreCase));
        }

        private bool CalculateIsInvalidTime()
        {
            return CommentLines
                .Any(x => x.Equals("invalid time:", StringComparison.OrdinalIgnoreCase));
        }
    }
}