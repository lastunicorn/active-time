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
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ConsoleTools;
using LiteDB;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Persistence.LiteDB.Module.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    internal class DisplayFlow : IFlow
    {
        public void Run()
        {
            DisplayLiteDBDatabase();
            WindConsole.WriteLine();
            DisplaySQLiteDatabase();
        }

        private static void DisplaySQLiteDatabase()
        {
            WindConsole.WriteLineEmphasies("=========================================================");
            WindConsole.WriteLineEmphasies("Database: " + SQLiteUnitOfWork.ConnectionString);
            WindConsole.WriteLineEmphasies("=========================================================");
            WindConsole.WriteLine();

            using (SQLiteUnitOfWork unitOfWork = new SQLiteUnitOfWork())
            {
                DisplayAllData(unitOfWork);
                unitOfWork.DisplayAllTables();
            }
        }

        private static void DisplayLiteDBDatabase()
        {
            WindConsole.WriteLineEmphasies("=========================================================");
            WindConsole.WriteLineEmphasies("Database: " + LiteDBUnitOfWork.ConnectionString);
            WindConsole.WriteLineEmphasies("=========================================================");
            WindConsole.WriteLine();

            using (LiteDatabase database = new LiteDatabase(LiteDBUnitOfWork.ConnectionString))
            {
                Console.WriteLine("Collections:");
                IEnumerable<string> collectionNames = database.GetCollectionNames();

                foreach (string collectionName in collectionNames)
                    WindConsole.WriteLine("- " + collectionName);

                WindConsole.WriteLine();
            }

            using (IUnitOfWork unitOfWork = new LiteDBUnitOfWork())
                DisplayAllData(unitOfWork);
        }

        private static void DisplayAllData(IUnitOfWork unitOfWork)
        {
            WindConsole.WriteLineEmphasies("---------------------------------------------------------");
            WindConsole.WriteLineEmphasies("TimeRecord");
            WindConsole.WriteLineEmphasies("---------------------------------------------------------");
            WindConsole.WriteLine();

            IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetAll();

            foreach (TimeRecord timeRecord in timeRecords)
                Console.WriteLine(timeRecord);

            Console.WriteLine();

            WindConsole.WriteLineEmphasies("---------------------------------------------------------");
            WindConsole.WriteLineEmphasies("DayComment");
            WindConsole.WriteLineEmphasies("---------------------------------------------------------");
            WindConsole.WriteLine();

            IList<DayComment> dayComments = unitOfWork.DayCommentRepository.GetAll();

            foreach (DayComment dayComment in dayComments)
                WindConsole.WriteLine(dayComment);

            WindConsole.WriteLine();
        }
    }
}