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
using LiteDB;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Persistence.LiteDB.Module.UnitOfWork;
using DustInTheWind.ActiveTime.DataMigration.Utils;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    internal class DisplayFlow : IFlow
    {
        public void Run()
        {
            DisplayLiteDBDatabase();
            CustomConsole.WriteLine();
            DisplaySQLiteDatabase();
        }

        private static void DisplaySQLiteDatabase()
        {
            CustomConsole.WriteLineEmphasies("=========================================================");
            CustomConsole.WriteLineEmphasies("Database: " + SQLiteUnitOfWork.ConnectionString);
            CustomConsole.WriteLineEmphasies("=========================================================");
            CustomConsole.WriteLine();

            using (SQLiteUnitOfWork unitOfWork = new SQLiteUnitOfWork())
            {
                DisplayAllData(unitOfWork);
                unitOfWork.DisplayAllTables();
            }
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

            using (IUnitOfWork unitOfWork = new LiteDBUnitOfWork())
                DisplayAllData(unitOfWork);
        }

        private static void DisplayAllData(IUnitOfWork unitOfWork)
        {
            CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
            CustomConsole.WriteLineEmphasies("TimeRecord");
            CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
            CustomConsole.WriteLine();

            IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetAll();

            foreach (TimeRecord timeRecord in timeRecords)
                Console.WriteLine(timeRecord);

            Console.WriteLine();

            CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
            CustomConsole.WriteLineEmphasies("DayComment");
            CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
            CustomConsole.WriteLine();

            IList<DayComment> dayComments = unitOfWork.DayCommentRepository.GetAll();

            foreach (DayComment dayComment in dayComments)
                CustomConsole.WriteLine(dayComment);

            CustomConsole.WriteLine();
        }
    }
}