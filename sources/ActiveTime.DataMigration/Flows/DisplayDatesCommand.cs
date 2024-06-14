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
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.Persistence;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Menues;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Persistence.LiteDB.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    internal class DisplayDatesCommand : ICommand
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

            using (LiteDBUnitOfWork unitOfWork = new LiteDBUnitOfWork())
            {
                DisplayTimeRecordDays(unitOfWork);
                DisplayDayCommentDays(unitOfWork);
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
                DisplayTimeRecordDays(unitOfWork);
                DisplayDayCommentDays(unitOfWork);
            }
        }

        private static void DisplayTimeRecordDays(IUnitOfWork unitOfWork)
        {
            CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
            CustomConsole.WriteLineEmphasies("TimeRecord");
            CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
            CustomConsole.WriteLine();

            IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetAll();

            DateTime previousDate = DateTime.MinValue;

            foreach (TimeRecord timeRecord in timeRecords)
            {
                DateTime date = timeRecord.Date;

                bool isSameMonth = date.Year == previousDate.Year && date.Month == previousDate.Month;
                if (!isSameMonth)
                {
                    Console.WriteLine();
                    Console.Write(date.Year + " " + date.Month + " -");
                }

                Console.Write(" " + date.Day);

                previousDate = date;
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static void DisplayDayCommentDays(IUnitOfWork unitOfWork)
        {
            CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
            CustomConsole.WriteLineEmphasies("DayComment");
            CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
            CustomConsole.WriteLine();

            IList<DateRecord> dayComments = unitOfWork.DateRecordRepository.GetAll();

            DateTime previousDate = DateTime.MinValue;

            foreach (DateRecord dayComment in dayComments)
            {
                DateTime date = dayComment.Date;

                bool isSameMonth = date.Year == previousDate.Year && date.Month == previousDate.Month;
                if (!isSameMonth)
                {
                    Console.WriteLine();
                    Console.Write(date.Year + " " + date.Month + " -");
                }

                Console.Write(" " + date.Day);

                previousDate = date;
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}