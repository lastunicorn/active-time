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

using DustInTheWind.ActiveTime.DataMigration.Flows;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Menues;

namespace DustInTheWind.ActiveTime.DataMigration;

internal static class Program
{
    public static volatile bool ExitWasRequested;

    private static void Main(string[] args)
    {
        try
        {
            CustomConsole.WriteLine("ActiveTime Data Migration Tool");
            CustomConsole.WriteLine("===============================================================================");

            ExitWasRequested = false;
            IEnumerable<TextMenuItem> items = CreateMenuItems();

            while (!ExitWasRequested)
            {
                CustomConsole.WriteLine();

                TextMenu textMenu = new(items);
                textMenu.Display();
            }
        }
        catch (Exception ex)
        {
            CustomConsole.WriteLineError(ex);
            Pause.QuickDisplay();
        }
    }

    private static IEnumerable<TextMenuItem> CreateMenuItems()
    {
        return new List<TextMenuItem>
        {
            new()
            {
                Id = "1",
                Text = "Display Database Structure",
                Command = new DisplayDatabaseStructureCommand()
            },
            new()
            {
                Id = "2",
                Text = "Display All Data",
                Command = new DisplayDataCommand()
            },
            new()
            {
                Id = "3",
                Text = "Display Record Dates",
                Command = new DisplayDatesCommand()
            },
            new()
            {
                Id = "4",
                Text = "Simulate Migration",
                Command = new MigrationCommand { Simulate = true }
            },
            new()
            {
                Id = "5",
                Text = "Migrate",
                Command = new MigrationCommand()
            },
            new()
            {
                Id = "6",
                Text = "Statistics",
                Command = new StatisticsCommand()
            },
            new()
            {
                Id = "0",
                Text = "Exit",
                Command = new ExitCommand()
            }
        };
    }
}