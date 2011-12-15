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
using DustInTheWind.ActiveTime.Common.Persistence;
using NHibernate;

namespace DustInTheWind.ActiveTime.PersistenceModule.Repositories
{
    /// <summary>
    ///  Repository class that provides access to the comment records storred in the persistent layer.
    /// </summary>
    public class DayCommentRepository : CrudRepositoryBase<DayComment>, IDayCommentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DayCommentRepository"/> class.
        /// </summary>
        public DayCommentRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayCommentRepository"/> class with
        /// the session to be used to access the database.
        /// </summary>
        /// <param name="session">The session that will be used to access the database.</param>
        public DayCommentRepository(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Returns from the database the comment for the specified date.
        /// </summary>
        /// <param name="date">The date of the comment to return.</param>
        /// <returns>An instance of <see cref="DayComment"/> containing the comment from the database.</returns>
        public DayComment GetByDate(DateTime date)
        {
            return CurrentSession.QueryOver<DayComment>()
                .Where(c => c.Date == date)
                .SingleOrDefault();
        }
    }
}
