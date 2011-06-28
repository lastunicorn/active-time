using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DustInTheWind.ActiveTime.Goose;
using System.ComponentModel.Composition;

namespace DustInTheWind.ActiveTime.Exporters
{
    [Export(typeof(IExporter))]
    internal class SafeExporter : IExporter
    {
        public string Name
        {
            get { return GetType().FullName; }
        }

        public string FriendlyName
        {
            get { return "Safe CSV Exporter"; }
        }

        public string Description
        {
            get { return @"Exports the data in a csv format. From the daily records will be exported three csv records: two records representing the work time separated by one record representing a break. The biggest break is considered."; }
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
                beginHour = dayRecord.Records[0].StartTime;
                endHour = dayRecord.Records[0].EndTime;

                breakStartHour = dayRecord.Records[0].EndTime;
                breakEndHour = dayRecord.Records[0].EndTime;

                foreach (Record record in dayRecord.Records)
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

                TimeSpan deltaToEight = TimeSpan.FromHours(8) - ((breakStartHour.Value - beginHour) + (endHour - breakEndHour.Value));
                endHour += deltaToEight;

                sw.WriteLine(string.Format("{0};IUGA Alexandru;{1};{2};;;{3};{4}", dayRecord.Date.ToString("dd.MM.yyyy"), beginHour.ToString(), breakStartHour.ToString(), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
                sw.WriteLine(string.Format("{0};IUGA Alexandru;{1};{2};;;Break;Break", dayRecord.Date.ToString("dd.MM.yyyy"), breakStartHour.ToString(), breakEndHour.ToString()));
                sw.WriteLine(string.Format("{0};IUGA Alexandru;{1};{2};;;{3};{4}", dayRecord.Date.ToString("dd.MM.yyyy"), breakEndHour.ToString(), endHour.ToString(), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
            }
            else
            {
                TimeSpan deltaToEight = TimeSpan.FromHours(8) - (endHour - beginHour);
                endHour += deltaToEight;

                sw.WriteLine(string.Format("{0};IUGA Alexandru;{1};{2};;;{3};{4}", dayRecord.Date.ToString("dd.MM.yyyy"), beginHour.ToString(), endHour.ToString(), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
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
