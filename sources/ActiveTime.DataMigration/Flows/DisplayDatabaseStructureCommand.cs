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
using System.Collections.Generic;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Menues;
using LiteDB;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Persistence.LiteDB.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    internal class DisplayDatabaseStructureCommand : ICommand
    {
        public bool IsActive => true;

        public void Execute()
        {
            DisplayLiteDBDatabase();
            Console.WriteLine();
            DisplaySQLiteDatabase();
        }

        private static void DisplayLiteDBDatabase()
        {
            CustomConsole.WriteLineEmphasies("=========================================================");
            CustomConsole.WriteLineEmphasies("Database: " + LiteDBUnitOfWork.ConnectionString);
            CustomConsole.WriteLineEmphasies("=========================================================");
            CustomConsole.WriteLine();

            using (LiteDatabase database = new LiteDatabase(LiteDBUnitOfWork.ConnectionString))
            {
                Console.WriteLine("Collections:");
                IEnumerable<string> collectionNames = database.GetCollectionNames();

                foreach (string collectionName in collectionNames)
                    CustomConsole.WriteLine("- " + collectionName);

                CustomConsole.WriteLine();
            }
        }

        private static void DisplaySQLiteDatabase()
        {
            CustomConsole.WriteLineEmphasies("=========================================================");
            CustomConsole.WriteLineEmphasies("Database: " + SQLiteUnitOfWork.ConnectionString);
            CustomConsole.WriteLineEmphasies("=========================================================");
            CustomConsole.WriteLine();

            using (SQLiteUnitOfWork unitOfWork = new SQLiteUnitOfWork())
            {
                unitOfWork.DisplayAllTables();
            }
        }
    }
}