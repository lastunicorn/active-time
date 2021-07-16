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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.DataMigration.Migration
{
    internal class CommentMigration
    {
        private readonly IUnitOfWork sourceUnitOfWork;
        private readonly IUnitOfWork destinationUnitOfWork;

        public bool Simulate { get; set; }
        public int MigratedRecordsCount { get; private set; }
        public int IgnoredRecordsCount { get; private set; }
        public Dictionary<DateTime, string> Warnings { get; } = new Dictionary<DateTime, string>();

        public event EventHandler<CommentMigratedEventArgs> CommentMigrated;

        public CommentMigration(IUnitOfWork sourceUnitOfWork, IUnitOfWork destinationUnitOfWork)
        {
            this.sourceUnitOfWork = sourceUnitOfWork ?? throw new ArgumentNullException(nameof(sourceUnitOfWork));
            this.destinationUnitOfWork = destinationUnitOfWork ?? throw new ArgumentNullException(nameof(destinationUnitOfWork));
        }

        public void Migrate()
        {
            PrepareMigration();
            MigrateComments();
        }

        private void PrepareMigration()
        {
            Warnings.Clear();
            MigratedRecordsCount = 0;
            IgnoredRecordsCount = 0;
        }

        private void MigrateComments()
        {
            IEnumerable<DayRecord> dayComments = sourceUnitOfWork.DayCommentRepository.GetAll();

            foreach (DayRecord dayComment in dayComments)
            {
                try
                {
                    MigrateComment(dayComment);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error migrating comment for date " + dayComment.Date, ex);
                }

                OnCommentMigrated(new CommentMigratedEventArgs(dayComment));
            }
        }

        private void MigrateComment(DayRecord dayRecord)
        {
            DateTime date = dayRecord.Date;

            DayRecord destinationRecord = destinationUnitOfWork.DayCommentRepository.GetByDate(date);

            if (destinationRecord != null)
            {
                bool existsIdenticalRecord = destinationRecord.Date - dayRecord.Date > TimeSpan.FromSeconds(-1) &&
                                             destinationRecord.Date - dayRecord.Date < TimeSpan.FromSeconds(1) &&
                                             destinationRecord.Comment == dayRecord.Comment;

                if (existsIdenticalRecord)
                {
                    IgnoredRecordsCount++;
                }
                else
                {
                    if (!Warnings.ContainsKey(date))
                    {
                        string message = string.Format("Date {0:d} conflict: Comment already exists in the destination database. No record will be imported.", date);
                        Warnings.Add(date, message);
                    }
                }
            }
            else
            {
                if (!Simulate)
                    InsertRecordInDestination(dayRecord);

                MigratedRecordsCount++;
            }
        }

        private void InsertRecordInDestination(DayRecord dayRecord)
        {
            DayRecord dayRecordCopy = new DayRecord
            {
                Date = dayRecord.Date,
                Comment = dayRecord.Comment
            };

            destinationUnitOfWork.DayCommentRepository.Add(dayRecordCopy);
        }

        protected virtual void OnCommentMigrated(CommentMigratedEventArgs e)
        {
            CommentMigrated?.Invoke(this, e);
        }
    }
}