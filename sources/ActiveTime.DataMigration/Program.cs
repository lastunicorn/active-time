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

namespace DustInTheWind.ActiveTime.DataMigration
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                WindConsole.WriteLine("ActiveTime Data Migration Tool");
                WindConsole.WriteLine("===============================================================================");

                Dictionary<int, string> items = CreateMenuItems();
                int selectedItem = WindConsole.Ask(items);
                IFlow flow = ChooseFlow(selectedItem);

                flow?.Run();
            }
            catch (Exception ex)
            {
                WindConsole.WriteLineError(ex);
            }

            WindConsole.Pause();
        }

        private static Dictionary<int, string> CreateMenuItems()
        {
            return new Dictionary<int, string>
            {
                {1, "Display Data"},
                {2, "Migrate Data"},
                {0, "Exit"}
            };
        }

        private static IFlow ChooseFlow(int selectedItem)
        {
            switch (selectedItem)
            {
                case 1:
                    return new DisplayFlow();

                case 2:
                    return new MigrationFlow();

                default:
                    return null;
            }
        }
    }
}
