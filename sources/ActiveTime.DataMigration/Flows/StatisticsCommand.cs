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

using DustInTheWind.ActiveTime.DataMigration.Statistics;
using DustInTheWind.ActiveTime.DataMigration.ViewControls;
using DustInTheWind.ConsoleTools.Menues;

namespace DustInTheWind.ActiveTime.DataMigration.Flows;

internal class StatisticsCommand : ICommand
{
    public bool IsActive => true;

    public void Execute()
    {
        // 2011
        //DateTime startTime = new DateTime(2011, 01, 01);
        //DateTime endTime = new DateTime(2012, 01, 01);

        // 2012
        //DateTime startTime = new DateTime(2012, 01, 01);
        //DateTime endTime = new DateTime(2013, 01, 01);

        // 2013
        //DateTime startTime = new DateTime(2013, 01, 01);
        //DateTime endTime = new DateTime(2014, 01, 01);

        // 2014
        //DateTime startTime = new DateTime(2014, 01, 01);
        //DateTime endTime = new DateTime(2015, 01, 01);

        // 2015 - fixed
        //DateTime startTime = new DateTime(2015, 01, 01);
        //DateTime endTime = new DateTime(2016, 01, 01);

        // 2016 - fixed
        //DateTime startTime = new DateTime(2016, 01, 01);
        //DateTime endTime = new DateTime(2017, 01, 01);

        // 2017 - fixed
        //DateTime startTime = new DateTime(2017, 01, 01);
        //DateTime endTime = DateTime.Today.AddDays(-1);

        // 2017 03 - fixed

        DateTime startTime = new(2018, 03, 01);
        DateTime endTime = new DateTime(2018, 04, 01).AddDays(-1);

        // All
        //DateTime startTime = new DateTime(2015, 01, 01);
        //DateTime endTime = DateTime.Today.AddDays(-1);

        using (AnalisedPeriod analisedPeriod = new(startTime, endTime))
        {
            DisplayAverage(analisedPeriod);
            Display(analisedPeriod);
        }
    }

    private static void DisplayAverage(AnalisedPeriod analisedPeriod)
    {
        TimeSpan averageActiveTime = analisedPeriod.CalcualteAverageTime();
        Console.WriteLine(averageActiveTime);
    }

    private static void Display(AnalisedPeriod analisedPeriod)
    {
        foreach (TimePerDay timePerDay in analisedPeriod)
        {
            TimePerDayView timePerDayView = new(timePerDay);
            timePerDayView.Display();
        }
    }
}