using System;
using DustInTheWind.ActiveTime.Goose;
using DustInTheWind.ActiveTime.UI.IViews;
using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.Controllers
{
    class StatisticsPresenter
    {
        private IStatisticsView view;
        private ActiveTimeApplication activeTimeApplication;
        private StatisticsModel model;

        public StatisticsPresenter(IStatisticsView view, ActiveTimeApplication activeTimeApplication)
        {
            this.view = view;
            this.activeTimeApplication = activeTimeApplication;

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
            TimeSpan totalWorkTime = TimeSpan.Zero;
            TimeSpan totalBreakTime = TimeSpan.Zero;

            int year = model.Year;
            Month month = model.SelectedMonth;
            if (year > 0 && month != null)
            {
                int daysInMonth = DateTime.DaysInMonth(year, month.Value);
                for (int i = 1; i <= daysInMonth; i++)
                {
                    DateTime date = new DateTime(year, month.Value, i);

                    DayRecord dayRecord = activeTimeApplication.Dal.GetDayRecord(date);

                    if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (dayRecord == null || dayRecord.IsEmpty))
                        continue;

                    if (dayRecord.HasRecords)
                    {
                        TimeSpan lastHour = TimeSpan.Zero;

                        foreach (Record record in dayRecord.Records)
                        {
                            if (lastHour != TimeSpan.Zero)
                            {
                                totalBreakTime += record.StartTime - lastHour;
                            }

                            totalWorkTime += record.EndTime - record.StartTime;

                            lastHour = record.EndTime;
                        }
                    }
                }
            }


            model.TotalWorkTime = totalWorkTime;
            model.TotalBreakTime = totalBreakTime;
            //view.DisplayInfoMessage(string.Format("Total work time: {0}\nTotal break time: {1}", TotalWorkTime, TotalBreakTime));
        }
    }
}
