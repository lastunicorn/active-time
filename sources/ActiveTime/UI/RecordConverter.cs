using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using DustInTheWind.ActiveTime.Goose;

namespace DustInTheWind.ActiveTime.UI
{
    public class RecordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(Record))
                return null;

            if (targetType == typeof(string) || targetType == typeof(object))
            {
                Record record = (Record)value;

                TimeSpan timeDiff = record.EndTime - record.StartTime;
                return record.StartTime.ToString() + " - " + record.EndTime.ToString() + " = " + timeDiff.ToString();
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
