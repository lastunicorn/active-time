// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using DustInTheWind.ConsoleTools.MenuControl;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    internal class StatisticsCommand : ICommand
    {
        public bool IsActive => true;

        public void Execute()
        {
            DateTime startTime = new DateTime(2017, 01, 01);
            DateTime endTime = DateTime.Today.AddDays(-1);

            using (AnalisedPeriod analisedPeriod = new AnalisedPeriod(startTime, endTime))
            {
                DisplayAverage(analisedPeriod);
                Display(analisedPeriod);
            }
        }

        private static void DisplayAverage(AnalisedPeriod analisedPeriod)
        {
            TimeSpan averageActiveTime = analisedPeriod.CalcualteAverageTime();
            Console.WriteLine(averageActiveTime);
        }

        private static void Display(AnalisedPeriod analisedPeriod)
        {
            foreach (TimePerDay timePerDay in analisedPeriod)
            {
                ConsoleColor color = DecideDayColor(timePerDay);
                int count = (int)Math.Round(timePerDay.Time.TotalMinutes / 15);

                CustomConsole.WriteLine(color, "{0,-14:yyyy MM dd ddd} - {1}", timePerDay.Date, new string('*', count));
            }
        }

        private static ConsoleColor DecideDayColor(TimePerDay timePerDay)
        {
            if (timePerDay.IsWeekEnd)
                return ConsoleColor.DarkCyan;

            if (timePerDay.IsVacation)
                return ConsoleColor.DarkCyan;

            if (timePerDay.IsHoliday)
                return ConsoleColor.DarkCyan;

            if (timePerDay.IsInvalidTime)
                return ConsoleColor.DarkGray;

            if (timePerDay.Time < TimeSpan.FromHours(3))
                return ConsoleColor.Red;

            return ConsoleColor.Gray;
        }
    }
}