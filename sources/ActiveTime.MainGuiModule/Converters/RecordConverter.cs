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
using System.Windows.Data;
using DustInTheWind.ActiveTime.Common.Recording;

namespace DustInTheWind.ActiveTime.MainGuiModule.Converters
{
    public class RecordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value == null || value.GetType() != typeof(Record))
            //    return null;

            if (value == null || !(value is DayTimeInterval))
                return null;

            if (targetType == typeof(string) || targetType == typeof(object))
            {
                DayTimeInterval record = (DayTimeInterval)value;

                TimeSpan timeDiff = record.EndTime - record.StartTime;
                return record.StartTime.ToString() + " - " + record.EndTime.ToString() + " = " + timeDiff.ToString();
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
