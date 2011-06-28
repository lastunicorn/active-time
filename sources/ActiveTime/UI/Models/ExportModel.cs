using System;
using System.Collections.Generic;
using System.ComponentModel;
using DustInTheWind.ActiveTime.Exporters;

namespace DustInTheWind.ActiveTime.UI.Models
{
    public class ExportModel : INotifyPropertyChanged
    {
        private bool exportButtonEnabled;

        public bool ExportButtonEnabled
        {
            get { return exportButtonEnabled; }
            set
            {
                exportButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExportButtonEnabled"));
            }
        }

        private Month[] months;

        public Month[] Months
        {
            get { return months; }
        }

        private int year;

        public int Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Year"));
            }
        }

        private Month selectedMonth;

        public Month SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMonth"));
            }
        }

        private List<IExporter> exporters = new List<IExporter>();

        public List<IExporter> Exporters
        {
            get { return exporters; }
        }

        private IExporter selectedExporter;

        public IExporter SelectedExporter
        {
            get { return selectedExporter; }
            set
            {
                selectedExporter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedExporter"));
            }
        }

        private string destinationFileName;

        public string DestinationFileName
        {
            get { return destinationFileName; }
            set
            {
                destinationFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DestinationFileName"));
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        public ExportModel()
        {
            months = new Month[12];

            for (int i = 0; i < 12; i++)
            {
                months[i] = new Month(i + 1, string.Format("{0:MMMM}", new DateTime(1, i + 1, 1)));
            }
        }
    }
}
