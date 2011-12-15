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
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common.Persistence;
using NHibernate;

namespace DustInTheWind.ActiveTime.PersistenceModule.Repositories
{
    public class TimeRecordRepository : CrudRepositoryBase<TimeRecord>, ITimeRecordRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRecordRepository"/> class.
        /// </summary>
        public TimeRecordRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRecordRepository"/> class with
        /// the session to be used to access the database.
        /// </summary>
        /// <param name="session">The session that will be used to access the database.</param>
        public TimeRecordRepository(ISession session)
            : base(session)
        {
        }

        public IList<TimeRecord> GetByDate(DateTime date)
        {
            return CurrentSession.QueryOver<TimeRecord>()
                .Where(x => x.Date == date)
                .OrderBy(x => x.StartTime).Asc
                .List();
        }
    }
}
