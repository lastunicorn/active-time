// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.MainGuiModule.Services
{
    class CommentsService : ICommentsService
    {
        private readonly IDayCommentRepository repository;

        private DayComment record;
        public DayComment Record
        {
            get { return record; }
            set { record = value; }
        }

        public event EventHandler RecordChanged;

        protected virtual void OnRecordChanged(EventArgs e)
        {
            if (RecordChanged != null)
                RecordChanged(this, e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsService"/> class.
        /// </summary>
        /// <param name="repository">The repository class used to access the database.</param>
        public CommentsService(IDayCommentRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            this.repository = repository;
        }

        public void RetrieveRecord(DateTime date)
        {
            record = repository.GetByDate(date);
            OnRecordChanged(EventArgs.Empty);
        }

        public void SaveRecord()
        {
            repository.AddOrUpdate(record);
        }
    }
}
