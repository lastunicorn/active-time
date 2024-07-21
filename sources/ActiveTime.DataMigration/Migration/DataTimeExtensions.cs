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

namespace DustInTheWind.ActiveTime.DataMigration.Migration;

internal static class DataTimeExtensions
{
    public static bool EqualsWithin(this DateTime dateTime, DateTime otherDateTime, TimeSpan delta)
    {
        TimeSpan diff = dateTime - otherDateTime;
        return diff > -delta && diff < delta;
    }

    public static bool EqualsWithin(this TimeSpan timeSpan, TimeSpan otherTimeSpan, TimeSpan delta)
    {
        TimeSpan diff = timeSpan - otherTimeSpan;
        return diff > -delta && diff < delta;
    }
}