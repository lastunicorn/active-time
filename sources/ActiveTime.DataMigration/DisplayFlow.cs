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
using DustInTheWind.ActiveTime.Common.Persistence;
using LiteDB;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.PersistenceModule.SQLite.AdoNet.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.PersistenceModule.LiteDB.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration
{
    internal class DisplayFlow : IFlow
    {
        public void Run()
        {
            DisplayLiteDBDatabase();
            Console.WriteLine();
            DisplaySQLiteDatabase();
        }

        private static void DisplaySQLiteDatabase()
        {
            Console.WriteLine("=========================================================");
            Console.WriteLine("Database: " + SQLiteUnitOfWork.ConnectionString);
            Console.WriteLine("=========================================================");
            Console.WriteLine();

            using (IUnitOfWork unitOfWork = new SQLiteUnitOfWork())
                DisplayAllData(unitOfWork);
        }

        private static void DisplayLiteDBDatabase()
        {
            Console.WriteLine("=========================================================");
            Console.WriteLine("Database: " + LiteDBUnitOfWork.ConnectionString);
            Console.WriteLine("=========================================================");
            Console.WriteLine();

            using (LiteDatabase database = new LiteDatabase(LiteDBUnitOfWork.ConnectionString))
            {
                Console.WriteLine("Collections:");
                IEnumerable<string> collectionNames = database.GetCollectionNames();

                foreach (string collectionName in collectionNames)
                    Console.WriteLine("- " + collectionName);

                Console.WriteLine();
            }
            using (IUnitOfWork unitOfWork = new LiteDBUnitOfWork())
                DisplayAllData(unitOfWork);
        }

        private static void DisplayAllData(IUnitOfWork unitOfWork)
        {
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("TimeRecord");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine();

            IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetAll();

            foreach (TimeRecord timeRecord in timeRecords)
                Console.WriteLine(timeRecord);

            Console.WriteLine();

            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("DayComment");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine();

            IList<DayComment> dayComments = unitOfWork.DayCommentRepository.GetAll();

            foreach (DayComment dayComment in dayComments)
                Console.WriteLine(dayComment);

            Console.WriteLine();
        }
    }
}