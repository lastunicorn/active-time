using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using DustInTheWind.ActiveTime.Recording;

namespace DustInTheWind.ActiveTime.UI
{
    public class RecordListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(List<Record>))
                return null;

            if (targetType == typeof(string))
            {
                List<Record> records = (List<Record>)value;

                StringBuilder sb = new StringBuilder();

                foreach (Record record in records)
                {
                    if (record == null)
                    {
                        sb.AppendLine("null");
                    }
                    else
                    {
                        TimeSpan timeDiff = record.EndTime - record.StartTime;
                        sb.AppendLine(record.StartTime.ToString() + " - " + record.EndTime.ToString() + " = " + timeDiff.ToString());
                    }
                }

                return sb.ToString();
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
