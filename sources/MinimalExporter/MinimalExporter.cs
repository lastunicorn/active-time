using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DustInTheWind.ActiveTime.Recording;
using System.ComponentModel.Composition;

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
