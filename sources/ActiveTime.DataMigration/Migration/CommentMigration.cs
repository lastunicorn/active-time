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

namespace DustInTheWind.ActiveTime.DataMigration.Migration
{
    internal class CommentMigration
    {
        private readonly IUnitOfWork sourceUnitOfWork;
        private readonly IUnitOfWork destinationUnitOfWork;

        public int MigratedRecordsCount { get; private set; }
        public int IgnoredRecordsCount { get; private set; }
        public Dictionary<DateTime, string> Warnings { get; } = new Dictionary<DateTime, string>();

        public CommentMigration(IUnitOfWork sourceUnitOfWork, IUnitOfWork destinationUnitOfWork)
        {
            if (sourceUnitOfWork == null) throw new ArgumentNullException(nameof(sourceUnitOfWork));
            if (destinationUnitOfWork == null) throw new ArgumentNullException(nameof(destinationUnitOfWork));

            this.sourceUnitOfWork = sourceUnitOfWork;
            this.destinationUnitOfWork = destinationUnitOfWork;
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
            IEnumerable<DayComment> dayComments = sourceUnitOfWork.DayCommentRepository.GetAll();

            foreach (DayComment dayComment in dayComments)
                MigrateComment(dayComment);

            Console.WriteLine();
        }

        private void MigrateComment(DayComment dayComment)
        {
            DateTime date = dayComment.Date;

            DayComment destinationRecord = destinationUnitOfWork.DayCommentRepository.GetByDate(date);

            if (destinationRecord != null)
            {
                bool existsIdenticalRecord = destinationRecord.Date - dayComment.Date > TimeSpan.FromSeconds(-1) &&
                                             destinationRecord.Date - dayComment.Date < TimeSpan.FromSeconds(1) &&
                                             destinationRecord.Comment == dayComment.Comment;

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
                InsertRecordInDestination(dayComment);
                MigratedRecordsCount++;
            }
        }

        private void InsertRecordInDestination(DayComment dayComment)
        {
            DayComment dayCommentCopy = new DayComment
            {
                Date = dayComment.Date,
                Comment = dayComment.Comment
            };

            destinationUnitOfWork.DayCommentRepository.Add(dayCommentCopy);
            Console.Write(".");
        }
    }
}
