﻿// ActiveTime
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

using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Menues;
using LiteDB;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Adapters.DataAccess.SQLite.AdoNet.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Adapters.DataAccess.LiteDB.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration.Flows;

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

        using LiteDatabase database = new(LiteDBUnitOfWork.ConnectionString);
        
        Console.WriteLine("Collections:");
        IEnumerable<string> collectionNames = database.GetCollectionNames();

        foreach (string collectionName in collectionNames)
            CustomConsole.WriteLine("- " + collectionName);

        CustomConsole.WriteLine();
    }

    private static void DisplaySQLiteDatabase()
    {
        CustomConsole.WriteLineEmphasies("=========================================================");
        CustomConsole.WriteLineEmphasies("Database: " + SQLiteUnitOfWork.ConnectionString);
        CustomConsole.WriteLineEmphasies("=========================================================");
        CustomConsole.WriteLine();

        using SQLiteUnitOfWork unitOfWork = new();
        unitOfWork.DisplayAllTables();
    }
}