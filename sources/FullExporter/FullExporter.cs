using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Recording;
using System.ComponentModel.Composition;
using System.IO;

namespace DustInTheWind.ActiveTime.Exporters
{
    [Export(typeof(IExporter))]
    internal class FullExporter : IExporter
    {
        private TimeSpan totalWorkTime = TimeSpan.Zero;
        private TimeSpan totalBreakTime = TimeSpan.Zero;

        public string Name
        {
            get { return GetType().FullName; }
        }

        public string FriendlyName
        {
            get { return "Full CSV Exporter"; }
        }

        public string Description
        {
            get { return @"Exports the data in a csv format. All the work records and the break records are exported."; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullExporter"/> class.
        /// </summary>
        public FullExporter()
        {
            Reset();
        }

        public void Initialize(Dictionary<string, object> parameters)
        {
        }

        public void Reset()
        {
            totalWorkTime = TimeSpan.Zero;
            totalBreakTime = TimeSpan.Zero;
        }

        public void ExportHeader(StreamWriter sw)
        {
            // Write the header.
            sw.WriteLine("Date;Person;Start Time HH:mm;End Time HH:mm;Working hours;Lunch Break Hours;Description;");
        }

        public void ExportFooter(StreamWriter sw)
        {
            sw.WriteLine();
            sw.WriteLine("Total Work Time = " + totalWorkTime.ToString() + " - " + totalWorkTime.TotalHours);
            sw.WriteLine("Total Break Time = " + totalBreakTime.ToString() + " - " + totalBreakTime.TotalHours);
        }

        public void ExportDayRecord(StreamWriter sw, DayRecord dayRecord)
        {
            string projectName = "";

            if (dayRecord.HasRecords)
            {
                TimeSpan lastHour = TimeSpan.Zero;

                foreach (Record record in dayRecord.Records)
                {
                    if (lastHour != TimeSpan.Zero)
                    {
                        sw.WriteLine(string.Format("{0};IUGA Alexandru;{1};{2};;;Break;Break", dayRecord.Date.ToString("dd.MM.yyyy"), lastHour.ToString(), record.StartTime.ToString()));
                        totalBreakTime += record.StartTime - lastHour;
                    }

                    sw.WriteLine(string.Format("{0};IUGA Alexandru;{1};{2};;;{3};{4}", dayRecord.Date.ToString("dd.MM.yyyy"), record.StartTime.ToString(), record.EndTime.ToString(), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
                    totalWorkTime += record.EndTime - record.StartTime;

                    lastHour = record.EndTime;
                }
            }
            else
            {
                sw.WriteLine(string.Format("{0};IUGA Alexandru;10:00;14:00;;;{1};{2}", dayRecord.Date.ToString("dd.MM.yyyy"), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
                sw.WriteLine(string.Format("{0};IUGA Alexandru;14:00;15:00;;;Break;Break", dayRecord.Date.ToString("dd.MM.yyyy")));
                sw.WriteLine(string.Format("{0};IUGA Alexandru;15:00;19:00;;;{1};{2}", dayRecord.Date.ToString("dd.MM.yyyy"), CsvUtil.CsvEncode(dayRecord.Comment), CsvUtil.CsvEncode(projectName)));
            }
        }
    }
}
