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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using DustInTheWind.ActiveTime.Recording;

namespace DustInTheWind.ActiveTime.Exporters
{
    [Export(typeof(IExporter))]
    internal class MinimalExporter : IExporter
    {
        public string Name
        {
            get { return GetType().FullName; }
        }

        public string FriendlyName
        {
            get { return "Minimal CSV Exporter"; }
        }

        public string Description
        {
            get { return "Exports the data in a csv format. From the daily records will be exported only the start and end time."; }
        }

        public void Initialize(Dictionary<string, object> parameters)
        {
        }

        public void Reset()
        {
        }

        public void ExportHeader(StreamWriter sw)
        {
        }

        public void ExportFooter(StreamWriter sw)
        {
        }

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
                beginHour = dayRecord.ActiveTimeRecords[0].StartTime;
                endHour = dayRecord.ActiveTimeRecords[0].EndTime;

                foreach (DayTimeInterval record in dayRecord.ActiveTimeRecords)
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
