using System;
using System.Collections.Generic;
using DustInTheWind.WindTools;
using LiteDB;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Persistence.LiteDB.Module.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    internal class DisplayDatabaseStructureFlow : IFlow
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