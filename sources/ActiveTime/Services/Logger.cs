using System;
using System.IO;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.Services
{
    public class Logger
    {
        public static void Log(DayComment value)
        {
            DateTime now = DateTime.Now;
            string logFileName = now.ToString("yyyy MM dd") + ".log";
            
            const string logDirectory = "Logs";

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            string logFilePath = Path.Combine(logDirectory, logFileName);

            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                string message = string.Format("{0} - {1:yyyy MM dd} - {2}", value.Id, value.Date, value.Comment);
                string text = string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] {1}", now, message);
                sw.WriteLine(text);
            }
        }
    }
}