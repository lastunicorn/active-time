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

using NHibernate;

namespace DustInTheWind.ActiveTime.Persistence.Repositories
{
    /// <summary>
    /// Provides basic functionality for a repository class.
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// The session used to access the database.
        /// </summary>
        private ISession currentSession;

        /// <summary>
        /// Gets the session used to access the database.
        /// </summary>
        protected ISession CurrentSession
        {
            get
            {
                if (currentSession == null)
                {
                    currentSession = SessionProvider.OpenSession();
                }

                return currentSession;
            }
        }

        /// <summary>
        /// Initializes a new instnace of the <see cref="RepositoryBase"/> class.
        /// </summary>
        public RepositoryBase()
        {
        }

        /// <summary>
        /// Initializes a new instnace of the <see cref="RepositoryBase"/> class with
        /// the session instance to be used to access the database.
        /// </summary>
        /// <param name="session">The session instance to be used to access the database.</param>
        public RepositoryBase(ISession session)
        {
            this.currentSession = session;
        }
    }
}
