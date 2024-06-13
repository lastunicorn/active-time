﻿// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DustInTheWind.ActiveTime.Presentation.Converters;

/// <summary>
/// Converts the <see cref="TextWrapping"/> value into <see cref="bool"/> value and viceversa.
/// </summary>
public class BooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || value.GetType() != typeof(bool))
            return null;

        if (value.GetType() == targetType)
            return value;

        if (targetType == typeof(TextWrapping))
            return (bool)value ? TextWrapping.Wrap : TextWrapping.NoWrap;

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || targetType != typeof(bool))
            return null;

        if (value.GetType() == typeof(TextWrapping))
        {
            TextWrapping textWrapping = (TextWrapping)value;

            return textWrapping != TextWrapping.NoWrap;
        }

        return null;
    }
}