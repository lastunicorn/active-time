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
using DustInTheWind.ActiveTime.DataMigration.Statistics;
using DustInTheWind.ConsoleTools;

namespace DustInTheWind.ActiveTime.DataMigration.ViewControls
{
    internal class TimePerDayView
    {
        private readonly TimePerDay timePerDay;

        private ConsoleColor color;
        private bool isFreeDay;
        private int realStarCount;
        private int missingStarCount;
        private int overtimeStarCount;

        public TimePerDayView(TimePerDay timePerDay)
        {
            this.timePerDay = timePerDay ?? throw new ArgumentNullException(nameof(timePerDay));
        }

        private void Analyze()
        {
            if (timePerDay.IsWeekEnd)
            {
                color = ConsoleColor.DarkCyan;
                isFreeDay = true;
            }
            else if (timePerDay.IsVacation)
            {
                color = ConsoleColor.DarkCyan;
                isFreeDay = true;
            }
            else if (timePerDay.IsHoliday)
            {
                color = ConsoleColor.DarkCyan;
                isFreeDay = true;
            }
            else if (timePerDay.IsInvalidTime)
            {
                color = ConsoleColor.DarkGray;
                isFreeDay = false;
            }
            else if (timePerDay.Time < TimeSpan.FromHours(3))
            {
                color = ConsoleColor.Red;
                isFreeDay = false;
            }
            else
            {
                color = ConsoleColor.Gray;
                isFreeDay = false;
            }

            int totalStarCount = (int)Math.Round(timePerDay.Time.TotalMinutes / 15);

            if (totalStarCount < 0)
                totalStarCount = 0;

            const int idealStarCount = 7 * 4;

            realStarCount = totalStarCount <= idealStarCount ? totalStarCount : idealStarCount;
            missingStarCount = totalStarCount < idealStarCount ? idealStarCount - totalStarCount : 0;
            overtimeStarCount = totalStarCount > idealStarCount ? totalStarCount - idealStarCount : 0;
        }

        public void Display()
        {
            Analyze();

            CustomConsole.Write(color, "{0,-14:yyyy MM dd ddd} - {1}", timePerDay.Date, new string('*', realStarCount));

            if (!isFreeDay)
            {
                if (missingStarCount > 0)
                    CustomConsole.Write(color, "{0}", new string('_', missingStarCount));

                if (overtimeStarCount > 0)
                    CustomConsole.Write(color, "{0}", new string('*', overtimeStarCount));
            }

            CustomConsole.WriteLine();
        }
    }
}