// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using DustInTheWind.ActiveTime.DataMigration.Flows;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.MenuControl;

namespace DustInTheWind.ActiveTime.DataMigration
{
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

                    TextMenu textMenu = new TextMenu(items);
                    textMenu.Display();
                }
            }
            catch (Exception ex)
            {
                CustomConsole.WriteLineError(ex);
            }
        }

        private static IEnumerable<TextMenuItem> CreateMenuItems()
        {
            return new List<TextMenuItem>
            {
                new TextMenuItem
                {
                    Id = "1",
                    Text = "Display Database Structure",
                    Command = new DisplayDatabaseStructureCommand()
                },
                new TextMenuItem
                {
                    Id = "2",
                    Text = "Display All Data",
                    Command = new DisplayDataCommand()
                },
                new TextMenuItem
                {
                    Id = "3",
                    Text = "Display Record Dates",
                    Command = new DisplayDatesCommand()
                },
                new TextMenuItem
                {
                    Id = "4",
                    Text = "Simulate Migration",
                    Command = new MigrationCommand { Simulate = true }
                },
                new TextMenuItem
                {
                    Id = "5",
                    Text = "Migrate",
                    Command = new MigrationCommand()
                },
                new TextMenuItem
                {
                    Id = "6",
                    Text = "Statistics",
                    Command = new StatisticsCommand()
                },
                new TextMenuItem
                {
                    Id = "0",
                    Text = "Exit",
                    Command = new ExitCommand()
                }
            };
        }
    }
}
