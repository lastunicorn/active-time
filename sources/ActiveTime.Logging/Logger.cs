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
using System.IO;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Logging;

namespace DustInTheWind.ActiveTime.Logging
{
    public class Logger : ILogger
    {
        private const string LogDirectory = "Logs";

        public void Log(DayComment value)
        {
            string message = string.Format("{0} - {1:yyyy MM dd} - {2}", value.Id, value.Date, value.Comment);
            DateTime now = DateTime.Now;

            WriteLog(message, now);
        }

        private static void WriteLog(string message, DateTime dateTime)
        {
            EnsureLogDirectory();

            string logFileName = dateTime.ToString("yyyy MM dd") + ".log";
            string logFilePath = Path.Combine(LogDirectory, logFileName);

            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                string line = string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] {1}", dateTime, message);
                sw.WriteLine(line);
            }
        }

        private static void EnsureLogDirectory()
        {
            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);
        }

        public void Log(string message)
        {
            DateTime now = DateTime.Now;
            WriteLog(message, now);
        }
    }
}