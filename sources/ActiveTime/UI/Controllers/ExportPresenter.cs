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
using System.IO;
using DustInTheWind.ActiveTime.Exporters;
using DustInTheWind.ActiveTime.Recording;
using DustInTheWind.ActiveTime.UI.IViews;
using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.Controllers
{
    internal class ExportPresenter
    {
        /// <summary>
        /// The view used to interact with the user.
        /// </summary>
        private IExportView view;

        /// <summary>
        /// The model containing data displayed in the current window.
        /// </summary>
        private ExportModel model;

        /// <summary>
        /// The application model.
        /// </summary>
        private ActiveTimeApplication activeTimeApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportPresenter"/> class.
        /// </summary>
        /// <param name="view">The view used to interact with the user.</param>
        /// <param name="activeTimeApplication">The application model.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExportPresenter(IExportView view, ActiveTimeApplication activeTimeApplication)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            if (activeTimeApplication == null)
                throw new ArgumentNullException("activeTimeApplication");

            this.view = view;
            this.activeTimeApplication = activeTimeApplication;

            DateTime now = DateTime.Now;

            model = new ExportModel();
            model.Year = now.Year;
            model.SelectedMonth = model.Months[now.Month - 1];
            model.Exporters.AddRange(activeTimeApplication.Exporters.GetExporters());
            model.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(model_PropertyChanged);
            model.DestinationFileName = "export.csv";
        }

        void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Year":
                case "SelectedMonth":
                case "SelectedExporter":
                case "DestinationFileName":
                    RefreshExportButton();
                    break;
            }
        }

        /// <summary>
        /// Method called when the windows is loaded.
        /// </summary>
        public void WindowLoaded()
        {
            try
            {
                view.Model = model;
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        /// <summary>
        /// Method called when the Browse button is clicked.
        /// </summary>
        public void BrowseButtonClicked()
        {
            try
            {
                string fileName = view.RequestNewExportFileName();
                if (fileName != null)
                {
                    model.DestinationFileName = fileName;
                    RefreshExportButton();
                }
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void RefreshExportButton()
        {
            model.ExportButtonEnabled = model.Year > 0 && model.SelectedMonth != null && model.SelectedExporter != null && !string.IsNullOrEmpty(model.DestinationFileName);
        }

        public void ExportButtonClicked()
        {
            try
            {
                int year = model.Year;
                Month month = model.SelectedMonth;
                if (year > 0 && month != null)
                {
                    IExporter exporter = model.SelectedExporter;

                    if (exporter != null)
                    {
                        using (StreamWriter sw = new StreamWriter(model.DestinationFileName))
                        {
                            exporter.ExportHeader(sw);

                            int daysInMonth = DateTime.DaysInMonth(year, month.Value);
                            for (int i = 1; i <= daysInMonth; i++)
                            {
                                DateTime date = new DateTime(year, month.Value, i);

                                DayRecord dayRecords = activeTimeApplication.RecordRepository.GetDayRecord(date);

                                if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (dayRecords == null || dayRecords.IsEmpty))
                                    continue;

                                exporter.ExportDayRecord(sw, dayRecords);
                            }

                            exporter.ExportFooter(sw);
                        }
                    }
                }

                view.DisplayInfoMessage("Done!");
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }
    }
}
