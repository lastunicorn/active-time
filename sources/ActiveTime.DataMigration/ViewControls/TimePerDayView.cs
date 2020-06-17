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