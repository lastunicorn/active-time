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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DustInTheWind.ActiveTime.Exporters;
using DustInTheWind.ActiveTime.Recording;
using DustInTheWind.ActiveTime.UI.IViews;
using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.Controllers
{
    internal class MainPresenter
    {
        /// <summary>
        /// The view used to interact with the user.
        /// </summary>
        private IMainView view;

        /// <summary>
        /// The model containing data displayed in the current window.
        /// </summary>
        private MainModel model;

        /// <summary>
        /// The application model.
        /// </summary>
        private ActiveTimeApplication activeTimeApplication;

        private TrayIconManager trayIconManager;

        public TrayIconManager TrayIconManager
        {
            get { return trayIconManager; }
            set { trayIconManager = value; }
        }

        /// <summary>
        /// The default Text of the status.
        /// </summary>
        private const string DEFAULT_STATUS = "Ready";

        /// <summary>
        /// The Time in miliseconds after which the status Text will be reset to the default one.
        /// </summary>
        private const int STATUS_TIMEOUT = 5000;

        /// <summary>
        /// Timer used to reset the status Text.
        /// </summary>
        private Timer timerStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPresenter"/> class.
        /// </summary>
        /// <param name="view">The view used to interact with the user.</param>
        /// <param name="model"></param>
        /// <param name="activeTimeApplication">The application model.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MainPresenter(IMainView view, MainModel model, ActiveTimeApplication activeTimeApplication)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            if (activeTimeApplication == null)
                throw new ArgumentNullException("activeTimeApplication");

            this.view = view;
            this.activeTimeApplication = activeTimeApplication;

            timerStatus = new Timer(new TimerCallback(ResetStatusTextTh));

            this.model = model;
            model.DateChanged += new EventHandler(model_DateChanged);

            activeTimeApplication.Recorder.Started += new EventHandler(recorder_Started);
            activeTimeApplication.Recorder.Stopped += new EventHandler(recorder_Stopped);
            activeTimeApplication.Recorder.Stamping += new EventHandler(recorder_Stamping);
            activeTimeApplication.Recorder.Stamped += new EventHandler(recorder_Stamped);
        }

        /// <summary>
        /// The call-back method of the timer that resets the status to the default Text.
        /// </summary>
        /// <param name="o">Unused</param>
        private void ResetStatusTextTh(object o)
        {
            model.StatusText = DEFAULT_STATUS;
        }

        private void model_DateChanged(object sender, EventArgs e)
        {
            UpdateModel();
        }

        /// <summary>
        /// Sets the status of the model to the specified Text and
        /// starts the timer that will reset it back to the default one.
        /// </summary>
        /// <param name="Text">The Text to be set as status.</param>
        /// <param name="timeout">The Time in miliseconds after which the status will be reset to the default Text. If this Value is 0, the status will never be reset.</param>
        private void SetStatus(string text, int timeout)
        {
            model.StatusText = text;
            if (timeout > 0)
            {
                timerStatus.Change(timeout, -1);
            }
        }

        private void recorder_Started(object sender, EventArgs e)
        {
            UpdateModel();
            SetStatus("Recorder started.", STATUS_TIMEOUT);
        }

        private void recorder_Stopped(object sender, EventArgs e)
        {
            UpdateModel();
            SetStatus("Recorder stopped.", STATUS_TIMEOUT);
        }

        private void recorder_Stamping(object sender, EventArgs e)
        {
            SetStatus("Updating the current record's time.", STATUS_TIMEOUT);
        }

        private void recorder_Stamped(object sender, EventArgs e)
        {
            SetStatus("Current record's time has been updated.", STATUS_TIMEOUT);
            UpdateModel();
        }

        private void UpdateModel()
        {
            if (model.Date != null)
            {
                DayRecord dayRecord = activeTimeApplication.Dal.GetDayRecord(model.Date.Value);
                model.DayRecord = dayRecord;
            }
            else
            {
                model.DayRecord = null;
            }
        }

        public void DateChanged()
        {
            SetStatus("Date changed.", STATUS_TIMEOUT);
        }

        private bool allowClose = false;

        public bool AllowClose
        {
            get { return allowClose; }
            set { allowClose = value; }
        }

        public bool WindowClosing()
        {
            if (!allowClose)
            {
                view.Hide();
            }

            return allowClose;
        }

        public void ViewLoaded()
        {
            //view.SelectedDate = DateTime.Today;
            model.Date = DateTime.Today;
        }

        public void ButtonRefreshClicked()
        {
            UpdateModel();

            SetStatus("Refreshed.", STATUS_TIMEOUT);
        }

        public void ButtonCommentsClicked()
        {
            //if (datePicker1.SelectedDate != null)
            //{
            //    CommentsWindow window = new CommentsWindow(dal, datePicker1.SelectedDate.Value);

            //    window.ShowDialog();
            //}
        }

        public void MenuItemDeleteClicked()
        {
            IList selectedItems = view.SelectedRecords;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                List<Record> recordsToDelete = new List<Record>();

                foreach (object selectedItem in selectedItems)
                {
                    if (selectedItem != null && selectedItem is Record)
                    {
                        Record selectedRecord = (Record)selectedItem;
                        recordsToDelete.Add((Record)selectedItem);
                    }
                }

                if (view.Confirm(string.Format("Delete {0} records?", recordsToDelete.Count), "Confirm Delete"))
                {
                    foreach (Record record in recordsToDelete)
                    {
                        activeTimeApplication.Dal.DeleteRecord(record);
                    }
                }

                UpdateModel();
            }
        }

        public void MenuItemExitClicked()
        {
            if (!Properties.Settings.Default.ConfirmExitApplication || view.Confirm("Are you sure you want to close ActiveTime?", "Confirm Exit"))
            {
                allowClose = true;
                view.ExitApplication();
            }
        }

        public void MenuItemExportSafeClicked()
        {
            try
            {
                DateTime? selectedDate = model.Date;
                if (selectedDate != null)
                {
                    using (StreamWriter sw = new StreamWriter("export.csv"))
                    {
                        // Write the header.
                        sw.WriteLine("Date;Person;Start Time HH:mm;End Time HH:mm;Working hours;Lunch Break Hours;Description;");

                        int daysInMonth = DateTime.DaysInMonth(selectedDate.Value.Year, selectedDate.Value.Month);
                        for (int i = 1; i <= daysInMonth; i++)
                        {
                            DateTime date = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, i);

                            DayRecord dayRecords = activeTimeApplication.Dal.GetDayRecord(date);

                            if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (dayRecords == null || dayRecords.IsEmpty))
                                continue;

                            IExporter exporter = activeTimeApplication.Exporters["DustInTheWind.ActiveTime.Exporters.SafeExporter"];
                            exporter.ExportDayRecord(sw, dayRecords);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void MenuItemExportClicked()
        {
            try
            {
                view.ShowExportWindow(activeTimeApplication);
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void MenuItemExportFullClicked()
        {
            try
            {
                DateTime? selectedDate = model.Date;
                if (selectedDate != null)
                {
                    IExporter exporter = activeTimeApplication.Exporters["DustInTheWind.ActiveTime.Exporters.FullExporter"];

                    if (exporter != null)
                    {
                        using (StreamWriter sw = new StreamWriter("export_full.csv"))
                        {
                            exporter.ExportHeader(sw);

                            int daysInMonth = DateTime.DaysInMonth(selectedDate.Value.Year, selectedDate.Value.Month);
                            for (int i = 1; i <= daysInMonth; i++)
                            {
                                DateTime date = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, i);

                                DayRecord dayRecords = activeTimeApplication.Dal.GetDayRecord(date);

                                if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (dayRecords == null || dayRecords.IsEmpty))
                                    continue;

                                exporter.ExportDayRecord(sw, dayRecords);
                            }

                            exporter.ExportFooter(sw);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void MenuItemExportNormalClicked()
        {
            try
            {
                DateTime? selectedDate = model.Date;
                if (selectedDate != null)
                {
                    IExporter exporter = activeTimeApplication.Exporters["DustInTheWind.ActiveTime.Exporters.NormalExporter"];

                    if (exporter != null)
                    {
                        using (StreamWriter sw = new StreamWriter("export_normal.csv"))
                        {
                            exporter.ExportHeader(sw);

                            int daysInMonth = DateTime.DaysInMonth(selectedDate.Value.Year, selectedDate.Value.Month);
                            for (int i = 1; i <= daysInMonth; i++)
                            {
                                DateTime date = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, i);

                                DayRecord dayRecords = activeTimeApplication.Dal.GetDayRecord(date);

                                if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (dayRecords == null || dayRecords.IsEmpty))
                                    continue;

                                exporter.ExportDayRecord(sw, dayRecords);
                            }

                            exporter.ExportFooter(sw);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void MenuItemExportMinimalClicked()
        {
            try
            {
                DateTime? selectedDate = model.Date;
                if (selectedDate != null)
                {
                    IExporter exporter = activeTimeApplication.Exporters["DustInTheWind.ActiveTime.Exporters.MinimalExporter"];

                    if (exporter != null)
                    {
                        using (StreamWriter sw = new StreamWriter("export_minimal.csv"))
                        {
                            exporter.ExportHeader(sw);

                            int daysInMonth = DateTime.DaysInMonth(selectedDate.Value.Year, selectedDate.Value.Month);
                            for (int i = 1; i <= daysInMonth; i++)
                            {
                                DateTime date = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, i);

                                DayRecord dayRecords = activeTimeApplication.Dal.GetDayRecord(date);

                                if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (dayRecords == null || dayRecords.IsEmpty))
                                    continue;

                                exporter.ExportDayRecord(sw, dayRecords);
                            }

                            exporter.ExportFooter(sw);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void MenuItemStatisticsClicked()
        {
            try
            {
                view.ShowStatisticsWindow(activeTimeApplication);

                //TimeSpan totalWorkTime = TimeSpan.Zero;
                //TimeSpan totalBreakTime = TimeSpan.Zero;

                //DateTime? selectedDate = model.Date;
                //if (selectedDate != null)
                //{
                //    int daysInMonth = DateTime.DaysInMonth(selectedDate.Value.Year, selectedDate.Value.Month);
                //    for (int i = 1; i <= daysInMonth; i++)
                //    {
                //        DateTime date = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, i);

                //        DayRecord dayRecord = activeTimeApplication.Dal.GetDayRecord(date);

                //        if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (dayRecord == null || dayRecord.IsEmpty))
                //            continue;

                //        if (dayRecord.HasRecords)
                //        {
                //            TimeSpan lastHour = TimeSpan.Zero;

                //            foreach (Record record in dayRecord.Records)
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

                //view.DisplayInfoMessage(string.Format("Total work time: {0}\nTotal break time: {1}", totalWorkTime, totalBreakTime));
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void MenuItemAboutClicked()
        {
            try
            {
                view.ShowAboutWindow();
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }
    }
}
