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

using System.Collections;
using DustInTheWind.ActiveTime.Adapters.DataAccess.LiteDB;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;

namespace DustInTheWind.ActiveTime.DataMigration.Statistics
{
    internal class AnalisedPeriod : IEnumerable<TimePerDay>, IDisposable
    {
        private readonly DateTime startTime;
        private readonly DateTime endTime;

        private readonly UnitOfWork unitOfWork;

        private List<TimePerDay> cache;

        public AnalisedPeriod(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime)
                throw new ArgumentOutOfRangeException(nameof(endTime), $"{nameof(endTime)} must be greater than {nameof(startTime)}");

            this.startTime = startTime;
            this.endTime = endTime;

            unitOfWork = new UnitOfWork();
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }

        public IEnumerator<TimePerDay> GetEnumerator()
        {
            if (cache != null)
            {
                foreach (TimePerDay timePerDay in cache)
                    yield return timePerDay;
            }
            else
            {
                cache = new List<TimePerDay>();

                IEnumerable<TimePerDay> itemsForEntirePeriod = GetItemsForEntirePeriod();

                foreach (TimePerDay timePerDay in itemsForEntirePeriod)
                {
                    cache.Add(timePerDay);
                    yield return timePerDay;
                }
            }
        }

        private IEnumerable<TimePerDay> GetItemsForEntirePeriod()
        {
            IEnumerable<TimePerDay> itemsFromDb = GetItemsFromDb();

            DateTime? lastDay = null;

            foreach (TimePerDay timePerDay in itemsFromDb)
            {
                while (true)
                {
                    lastDay = lastDay?.AddDays(1) ?? startTime;

                    if (lastDay == timePerDay.Date)
                        break;

                    yield return new TimePerDay(unitOfWork)
                    {
                        Date = lastDay.Value,
                        Time = TimeSpan.Zero
                    };
                }

                yield return timePerDay;
            }
        }

        private IEnumerable<TimePerDay> GetItemsFromDb()
        {
            ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;

            IEnumerable<TimeRecord> records = timeRecordRepository.GetAll()
                .Where(x => x.Date >= startTime && x.Date <= endTime);

            return records
                .GroupBy(x => x.Date)
                .Select(x => new TimePerDay(unitOfWork)
                {
                    Date = x.Key,
                    Time = x.Aggregate(TimeSpan.Zero, (value, current) => value + (current.EndTime - current.StartTime))
                })
                .OrderBy(x => x.Date);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TimeSpan CalcualteAverageTime()
        {
            int count = 0;
            TimeSpan sum = TimeSpan.Zero;

            foreach (TimePerDay timePerDay in this)
            {
                bool ignore = timePerDay.IsWeekEnd ||
                              timePerDay.IsHoliday ||
                              timePerDay.IsVacation ||
                              timePerDay.IsInvalidTime;

                if (ignore)
                    continue;

                count = count + 1;
                sum = sum + timePerDay.Time;
            }

            return new TimeSpan(sum.Ticks / count);
        }
    }
}