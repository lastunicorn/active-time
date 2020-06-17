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
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.Repositories;

namespace DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module
{
    public class UnitOfWork : IUnitOfWork
    {
        public static string ConnectionString { get; set; } = "Data Source=db.s3db";

        private DbConnection connection;
        private DbTransaction transaction;

        private TimeRecordRepository timeRecordRepository;
        public ITimeRecordRepository TimeRecordRepository
        {
            get
            {
                if (timeRecordRepository == null)
                {
                    OpenDatabaseAndTransaction();
                    timeRecordRepository = new TimeRecordRepository(connection);
                }

                return timeRecordRepository;
            }
        }

        private DayCommentRepository dayCommentRepository;
        public IDayCommentRepository DayCommentRepository
        {
            get
            {
                if (dayCommentRepository == null)
                {
                    OpenDatabaseAndTransaction();
                    dayCommentRepository = new DayCommentRepository(connection);
                }

                return dayCommentRepository;
            }
        }

        private void OpenDatabaseAndTransaction()
        {
            if (disposed)
                throw new ObjectDisposedException("UnitOfWork");

            if (connection == null)
            {
                connection = new SQLiteConnection(ConnectionString);
                connection.Open();
            }

            if (transaction == null)
                transaction = connection.BeginTransaction();
        }

        public void DisplayAllTables()
        {
            OpenDatabaseAndTransaction();

            DataTable dataTable = connection.GetSchema("Tables");

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine();
                Console.WriteLine("------------------------------------------");

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    DataColumn column = dataTable.Columns[i];
                    Console.WriteLine(column.ColumnName + ": " + row[i]);
                }
            }

            DumpTable("dbinfo");
        }

        private void DumpTable(string dbName)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "select * from " + dbName;
                DbDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Console.WriteLine();

                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        Console.WriteLine("-> " + dataReader[i]);
                    }
                }
            }
        }

        public void Commit()
        {
            if (disposed)
                throw new ObjectDisposedException("UnitOfWork");

            if (transaction == null)
                return;

            transaction.Commit();

            transaction.Dispose();
            transaction = null;
        }

        public void Rollback()
        {
            if (disposed)
                throw new ObjectDisposedException("UnitOfWork");

            if (transaction == null)
                return;

            transaction.Rollback();

            transaction.Dispose();
            transaction = null;
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                    transaction = null;
                }

                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }

            disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
