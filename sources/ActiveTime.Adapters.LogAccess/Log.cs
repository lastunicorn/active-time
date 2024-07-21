// ActiveTime
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

using DustInTheWind.ActiveTime.Ports.LogAccess;

namespace DustInTheWind.ActiveTime.Adapters.LogAccess;

public class Log : ILog
{
    private const string LogDirectory = "Logs";

    private static void WriteLog(string message, DateTime dateTime)
    {
        EnsureLogDirectory();

        string logFileName = dateTime.ToString("yyyy MM dd") + ".log";
        string logFilePath = Path.Combine(LogDirectory, logFileName);

        using StreamWriter sw = new(logFilePath, true);
        string line = $"[{dateTime:yyyy-MM-dd HH:mm:ss.fff}] {message}";
        sw.WriteLine(line);
    }

    private static void EnsureLogDirectory()
    {
        if (!Directory.Exists(LogDirectory))
            Directory.CreateDirectory(LogDirectory);
    }

    public void Write(string message)
    {
        DateTime now = DateTime.Now;
        WriteLog(message, now);
    }
}