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
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.UI.IViews;
using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.Controllers
{
    class StatisticsPresenter
    {
        private IStatisticsView view;
        private StatisticsModel model;

        public StatisticsPresenter(IStatisticsView view)
        {
            this.view = view;

            DateTime now = DateTime.Now;

            model = new StatisticsModel();
            model.Year = now.Year;
            model.SelectedMonth = model.Months[now.Month - 1];
        }


        public void WindowLoaded()
        {
            try
            {
                this.view.Model = model;
                RefreshData();
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void YearValueChanged()
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void MonthChanged()
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        private void RefreshData()
        {
            //TimeSpan totalWorkTime = TimeSpan.Zero;
            //TimeSpan totalBreakTime = TimeSpan.Zero;

            //int year = model.Year;
            //Month month = model.SelectedMonth;
            //if (year > 0 && month != null)
            //{
            //    int daysInMonth = DateTime.DaysInMonth(year, month.Value);
            //    for (int i = 1; i <= daysInMonth; i++)
            //    {
            //        DateTime date = new DateTime(year, month.Value, i);

            //        IList<TimeRecord> timeRecords = activeTimeApplication.RecordRepository.GetByDate(date);
            //        DayRecord dayRecord = DayRecord.FromTimeRecords(timeRecords);

            //        if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) || (dayRecord == null || dayRecord.IsEmpty))
            //            continue;

            //        if (dayRecord.HasRecords)
            //        {
            //            TimeSpan lastHour = TimeSpan.Zero;

            //            foreach (DayTimeInterval record in dayRecord.ActiveTimeRecords)
            //            {
            //                if (lastHour != TimeSpan.Zero)
            //                {
            //                    totalBreakTime += record.StartTime - lastHour;
            //                }

            //                totalWorkTime += record.EndTime - record.StartTime;

            //                lastHour = record.EndTime;
            //            }
            //        }
            //    }
            //}


            //model.TotalWorkTime = totalWorkTime;
            //model.TotalBreakTime = totalBreakTime;
            ////view.DisplayInfoMessage(string.Format("Total work time: {0}\nTotal break time: {1}", TotalWorkTime, TotalBreakTime));
        }
    }
}
