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
using DustInTheWind.ActiveTime.DataMigration.Utils;
using DustInTheWind.ActiveTime.Persistence;
using SQLiteUnitOfWork = DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.UnitOfWork;
using LiteDBUnitOfWork = DustInTheWind.ActiveTime.Persistence.LiteDB.Module.UnitOfWork;

namespace DustInTheWind.ActiveTime.DataMigration.Flows
{
    /// <summary>
    /// Migrates the data (v2.0) from SQLite database into LiteDB.
    /// </summary>
    internal class MigrationFlow : IFlow
    {
        private IUnitOfWork sourceUnitOfWork;
        private IUnitOfWork destinationUnitOfWork;

        private TimeMigration timeMigration;
        private CommentMigration commentMigration;

        public void Run()
        {
            PrepareMigration();

            try
            {
                timeMigration.Migrate();
                commentMigration.Migrate();

                destinationUnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
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
            commentMigration = new CommentMigration(sourceUnitOfWork, destinationUnitOfWork);
        }

        private void ConcludeMigration()
        {
            Console.WriteLine();
            CustomConsole.WriteLineSuccess(string.Format("Successfully migrated time records: {0}", timeMigration.MigratedRecordsCount));
            CustomConsole.WriteLineSuccess(string.Format("Already present time records: {0}", timeMigration.IgnoredRecordsCount));

            Console.WriteLine();
            CustomConsole.WriteLineSuccess(string.Format("Successfully migrated comment records: {0}", commentMigration.MigratedRecordsCount));
            CustomConsole.WriteLineSuccess(string.Format("Already present comment records: {0}", commentMigration.IgnoredRecordsCount));

            if (timeMigration.Warnings.Count > 0)
            {
                Console.WriteLine();
                CustomConsole.WriteLineWarning("Warnings (time records):");

                foreach (string warningText in timeMigration.Warnings.Values)
                    CustomConsole.WriteLineWarning(string.Format(" {0}", warningText));
            }

            if (commentMigration.Warnings.Count > 0)
            {
                Console.WriteLine();
                CustomConsole.WriteLineWarning("Warnings (comment records):");

                foreach (string warningText in commentMigration.Warnings.Values)
                    CustomConsole.WriteLineWarning(string.Format(" {0}", warningText));
            }

            sourceUnitOfWork?.Dispose();
            destinationUnitOfWork?.Dispose();
        }
    }
}