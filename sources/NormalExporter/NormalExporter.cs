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
    public class NormalExporter : IExporter
    {
        private const string USER_NAME_KEY = "UserName";

        private Dictionary<string, object> parameters;
        private string userName;

        /// <summary>
        /// Gets a string that identifies the exporter.
        /// </summary>
        public string Name
        {
            get { return GetType().FullName; }
        }

        /// <summary>
        /// Gets a friendly name to be displayed to the user.
        /// </summary>
        public string FriendlyName
        {
            get { return "Normal CSV Exporter"; }
        }

        /// <summary>
        /// Gets a short description of the exporter.
        /// </summary>
        public string Description
        {
            get { return "Exports the data in a csv format."; }
        }

        public void Initialize(Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                ClearParameters();
            }
            else
            {
                ValidateParameters(parameters);
                SetParameters(parameters);
            }

            Reset();
        }

        private void ClearParameters()
        {
            parameters = new Dictionary<string, object>();
            userName = string.Empty;
        }

        private void SetParameters(Dictionary<string, object> parameters)
        {
            this.parameters = parameters;
            userName = parameters[USER_NAME_KEY] as string;
        }

        private void ValidateParameters(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey(USER_NAME_KEY))
                throw new MandatoryParameterException(USER_NAME_KEY);

            if (parameters[USER_NAME_KEY] == null)
                throw new ParameterNullException(USER_NAME_KEY, typeof(string));

            if (!(parameters[USER_NAME_KEY] is string))
                throw new ParameterTypeException(USER_NAME_KEY, typeof(string), parameters[USER_NAME_KEY].GetType());
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// Writes nothing.
        /// </summary>
        public void ExportHeader(StreamWriter sw)
        {
        }

        /// <summary>
        /// Writes nothing.
        /// </summary>
        public void ExportFooter(StreamWriter sw)
        {
        }

        /// <summary>
        /// Writes a day record on three lines. There are written two working intervals
        /// separated by a break (the biggest break).
        /// </summary>
        /// <param name="dayRecord">The record that should be exported.</param>
        public void ExportDayRecord(StreamWriter sw, DayRecord dayRecord)
        {
            if (dayRecord == null)
                return;

            TimeSpan beginHour;
            TimeSpan endHour;
            string projectName = "";
            TimeSpan? breakStartHour = null;
            TimeSpan? breakEndHour = null;

            List<TimeSpan[]> breaks = new List<TimeSpan[]>();

            if (dayRecord.HasRecords)
            {
                beginHour = dayRecord.ActiveTimeRecords[0].StartTime;
                endHour = dayRecord.ActiveTimeRecords[0].EndTime;

                breakStartHour = dayRecord.ActiveTimeRecords[0].EndTime;
                breakEndHour = dayRecord.ActiveTimeRecords[0].EndTime;

                foreach (Record record in dayRecord.ActiveTimeRecords)
                {
                    if (breakStartHour != null)
                    {
                        breakEndHour = record.StartTime;
                        breaks.Add(new TimeSpan[] { breakStartHour.Value, breakEndHour.Value });

                        breakStartHour = null;
                        breakEndHour = null;
                    }

                    if (record.StartTime < beginHour)
                        beginHour = record.StartTime;

                    if (record.EndTime > endHour)
                        endHour = record.EndTime;

                    //if (record.StartTime > breakStartHour)
                    //    breakStartHour = record.EndTime;

                    breakStartHour = record.EndTime;
                }
            }
            else
            {
                beginHour = TimeSpan.FromHours(10);
                endHour = TimeSpan.FromHours(18);
            }

            int breakIndex = GetBiggestBreakIndex(breaks);
            if (breakIndex >= 0)
            {
                breakStartHour = breaks[breakIndex][0];
                breakEndHour = breaks[breakIndex][1];

                sw.WriteLine(string.Format("{0};{1};{2};{3};;;{4};{5}", dayRecord.Date.ToString("dd.MM.yyyy"), userName, beginHour.ToString(), breakStartHour.ToString(), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
                sw.WriteLine(string.Format("{0};{1};{2};{3};;;Break;Break", dayRecord.Date.ToString("dd.MM.yyyy"), userName, breakStartHour.ToString(), breakEndHour.ToString()));
                sw.WriteLine(string.Format("{0};{1};{2};{3};;;{4};{5}", dayRecord.Date.ToString("dd.MM.yyyy"), userName, breakEndHour.ToString(), endHour.ToString(), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
            }
            else
            {
                sw.WriteLine(string.Format("{0};{1};{2};{3};;;{4};{5}", dayRecord.Date.ToString("dd.MM.yyyy"), userName, beginHour.ToString(), endHour.ToString(), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
            }
        }

        private int GetBiggestBreakIndex(List<TimeSpan[]> breaks)
        {
            if (breaks == null || breaks.Count == 0)
                return -1;

            int index = 0;
            TimeSpan breakTime = breaks[0][1] - breaks[0][0];

            for (int i = 1; i < breaks.Count; i++)
            {
                TimeSpan time = breaks[i][1] - breaks[i][0];

                if (time > breakTime)
                {
                    breakTime = time;
                    index = i;
                }
            }

            return index;
        }
    }
}
