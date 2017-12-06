using System;
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.WindTools;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Persistence.LiteDB.Module.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    internal class DisplayDatesFlow : IFlow
    {
        public void Run()
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

            IList<DayComment> dayComments = unitOfWork.DayCommentRepository.GetAll();

            DateTime previousDate = DateTime.MinValue;

            foreach (DayComment dayComment in dayComments)
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