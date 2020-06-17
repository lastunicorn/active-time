// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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
using System.Linq;
using System.Text;
using System.IO;
using DustInTheWind.ActiveTime.Goose;

namespace DustInTheWind.ActiveTime.Exporters
{
    internal class MinimalExporter : IExporter
    {
        public void ExportDayRecord(StreamWriter sw, DayRecord dayRecord)
        {
            TimeSpan beginHour;
            TimeSpan endHour;
            TimeSpan workingHour = TimeSpan.Zero;
            TimeSpan breakHour = TimeSpan.Zero;
            string projectName = "";

            List<TimeSpan[]> breaks = new List<TimeSpan[]>();

            if (dayRecord.HasRecords)
            {
                beginHour = dayRecord.Records[0].StartTime;
                endHour = dayRecord.Records[0].EndTime;

                foreach (Record record in dayRecord.Records)
                {
                    if (record.StartTime < beginHour)
                        beginHour = record.StartTime;

                    if (record.EndTime > endHour)
                        endHour = record.EndTime;
                }
            }
            else
            {
                beginHour = TimeSpan.FromHours(10);
                endHour = TimeSpan.FromHours(18);
            }

            sw.WriteLine(string.Format("{0};IUGA Alexandru;{1};{2};;;{5};{6}", dayRecord.Date.ToString("dd.MM.yyyy"), beginHour.ToString(), endHour.ToString(), workingHour.ToString(), breakHour.ToString(), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
        }
    }
}
