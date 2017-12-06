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
using DustInTheWind.WindTools;

namespace DustInTheWind.ActiveTime.DataMigration
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                CustomConsole.WriteLine("ActiveTime Data Migration Tool");
                CustomConsole.WriteLine("===============================================================================");

                while (true)
                {
                    CustomConsole.WriteLine();

                    Dictionary<int, string> items = CreateMenuItems();
                    int selectedItem = CustomConsole.Ask(items);
                    IFlow flow = ChooseFlow(selectedItem);

                    if (flow == null)
                        break;

                    RunFlow(flow);
                }
            }
            catch (Exception ex)
            {
                CustomConsole.WriteLineError(ex);
            }
        }

        private static Dictionary<int, string> CreateMenuItems()
        {
            return new Dictionary<int, string>
            {
                { 1, "Display Database Structure" },
                { 2, "Display All Data" },
                { 3, "Display Record Dates" },
                { 4, "Simulate Migration" },
                { 5, "Migrate Data" },
                { 0, "Exit" }
            };
        }

        private static IFlow ChooseFlow(int selectedItem)
        {
            switch (selectedItem)
            {
                case 1: return new DisplayDatabaseStructureFlow();
                case 2: return new DisplayDataFlow();
                case 3: return new DisplayDatesFlow();
                case 4: return new MigrationFlow { Simulate = true };
                case 5: return new MigrationFlow();
                default: return null;
            }
        }

        private static void RunFlow(IFlow flow)
        {
            try
            {
                flow.Run();
            }
            catch (Exception ex)
            {
                CustomConsole.WriteLineError(ex);
            }
        }
    }
}
