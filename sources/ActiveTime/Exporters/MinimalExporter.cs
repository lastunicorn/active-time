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
