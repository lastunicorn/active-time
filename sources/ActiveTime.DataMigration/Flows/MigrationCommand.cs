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
using DustInTheWind.ActiveTime.DataMigration.Migration;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.Persistence;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Menues;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Persistence.LiteDB.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    /// <summary>
    /// Migrates the data (v2.0) from SQLite database into LiteDB.
    /// </summary>
    internal class MigrationCommand : ICommand
    {
        public bool IsActive => true;

        private IUnitOfWork sourceUnitOfWork;
        private IUnitOfWork destinationUnitOfWork;

        private TimeMigration timeMigration;
        private CommentMigration commentMigration;

        private DateTime previousTimeRecordDate;
        private DateTime previousCommentDate;

        public bool Simulate { get; set; }

        public void Execute()
        {
            PrepareMigration();

            try
            {
                CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
                CustomConsole.WriteLineEmphasies("Migrating Time Records");
                CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
                CustomConsole.WriteLine();

                timeMigration.Migrate();
                CustomConsole.WriteLine();

                CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
                CustomConsole.WriteLineEmphasies("Migrating Comment Records");
                CustomConsole.WriteLineEmphasies("---------------------------------------------------------");
                CustomConsole.WriteLine();

                commentMigration.Migrate();
                CustomConsole.WriteLine();

                destinationUnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                CustomConsole.WriteLine();
                CustomConsole.WriteLineError(ex);
            }
            finally
            {
                ConcludeMigration();
            }
        }

        private void PrepareMigration()
        {
            sourceUnitOfWork = new SQLiteUnitOfWork();
            destinationUnitOfWork = new LiteDBUnitOfWork();

            timeMigration = new TimeMigration(sourceUnitOfWork, destinationUnitOfWork);
            timeMigration.Simulate = Simulate;
            timeMigration.TimeRecordMigrated += HandleTimeRecordMigrated;
            previousTimeRecordDate = DateTime.MinValue;

            commentMigration = new CommentMigration(sourceUnitOfWork, destinationUnitOfWork);
            commentMigration.Simulate = Simulate;
            commentMigration.CommentMigrated += HandleCommentMigrated;
            previousCommentDate = DateTime.MinValue;
        }

        private void HandleTimeRecordMigrated(object sender, TimeRecordMigratedEventArgs e)
        {
            TimeRecord timeRecord = e.TimeRecord;

            DateTime date = timeRecord.Date;

            bool isSameMonth = date.Year == previousTimeRecordDate.Year && date.Month == previousTimeRecordDate.Month;
            if (!isSameMonth)
            {
                Console.WriteLine();
                Console.Write(date.Year + " " + date.Month + " -");
            }

            Console.Write(" " + date.Day);

            previousTimeRecordDate = date;
        }

        private void HandleCommentMigrated(object sender, CommentMigratedEventArgs e)
        {
            DateRecord dateRecord = e.DateRecord;

            DateTime date = dateRecord.Date;

            bool isSameMonth = date.Year == previousCommentDate.Year && date.Month == previousCommentDate.Month;
            if (!isSameMonth)
            {
                Console.WriteLine();
                Console.Write(date.Year + " " + date.Month + " -");
            }

            Console.Write(" " + date.Day);

            previousCommentDate = date;
        }

        private void ConcludeMigration()
        {
            CustomConsole.WriteLine();
            CustomConsole.WriteLineSuccess(string.Format("Successfully migrated time records: {0}", timeMigration.MigratedRecordsCount));
            CustomConsole.WriteLineSuccess(string.Format("Already present time records: {0}", timeMigration.IgnoredRecordsCount));

            CustomConsole.WriteLine();
            CustomConsole.WriteLineSuccess(string.Format("Successfully migrated comment records: {0}", commentMigration.MigratedRecordsCount));
            CustomConsole.WriteLineSuccess(string.Format("Already present comment records: {0}", commentMigration.IgnoredRecordsCount));

            if (timeMigration.Warnings.Count > 0)
            {
                CustomConsole.WriteLine();
                CustomConsole.WriteLineWarning("Warnings (time records):");

                foreach (string warningText in timeMigration.Warnings.Values)
                    CustomConsole.WriteLineWarning(string.Format(" {0}", warningText));
            }

            if (commentMigration.Warnings.Count > 0)
            {
                CustomConsole.WriteLine();
                CustomConsole.WriteLineWarning("Warnings (comment records):");

                foreach (string warningText in commentMigration.Warnings.Values)
                    CustomConsole.WriteLineWarning(string.Format(" {0}", warningText));
            }

            sourceUnitOfWork?.Dispose();
            destinationUnitOfWork?.Dispose();
        }
    }
}