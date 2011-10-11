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

using DustInTheWind.ActiveTime.PersistenceModule;
using NHibernate;

namespace DustInTheWind.ActiveTime.PersistenceModule.Repositories
{
    /// <summary>
    /// Provides basic functionality for a repository class.
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// The session used to access the database.
        /// </summary>
        private readonly ISession currentSession;

        /// <summary>
        /// Gets the session used to access the database.
        /// </summary>
        protected ISession CurrentSession
        {
            get { return currentSession; }
        }

        /// <summary>
        /// Initializes a new instnace of the <see cref="RepositoryBase"/> class.
        /// </summary>
        protected RepositoryBase()
        {
            currentSession = SessionProvider.OpenSession();
        }

        /// <summary>
        /// Initializes a new instnace of the <see cref="RepositoryBase"/> class with
        /// the session instance to be used to access the database.
        /// </summary>
        /// <param name="session">The session instance to be used to access the database.</param>
        protected RepositoryBase(ISession session)
        {
            currentSession = session;
        }
    }
}
