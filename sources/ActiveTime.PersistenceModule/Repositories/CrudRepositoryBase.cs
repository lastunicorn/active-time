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
using NHibernate;

namespace DustInTheWind.ActiveTime.PersistenceModule.Repositories
{
    /// <summary>
    /// Provides basic functionality for a repository class.
    /// </summary>
    public abstract class CrudRepositoryBase<TEntity> : RepositoryBase
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instnace of the <see cref="CrudRepositoryBase{T}"/> class.
        /// </summary>
        protected CrudRepositoryBase()
        {
        }

        /// <summary>
        /// Initializes a new instnace of the <see cref="CrudRepositoryBase{T}"/> class with
        /// the session instance to be used to access the database.
        /// </summary>
        /// <param name="session">The session instance to be used to access the database.</param>
        protected CrudRepositoryBase(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Adds a new comment record into the database.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/> object containing the data.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            CurrentSession.Save(entity);
        }

        /// <summary>
        /// Updates an existing comment into the database.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/> object containing the data.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            CurrentSession.Update(entity);
            CurrentSession.Flush();
        }

        /// <summary>
        /// If the comment record is new, it is added into the database. If not, the record is updated.
        /// A record is considered new if its id is zero.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/> object containing the data.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddOrUpdate(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            CurrentSession.SaveOrUpdate(entity);
            CurrentSession.Flush();
        }

        /// <summary>
        /// Deletes an existing comment record from the database.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/> object containing the data.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            CurrentSession.Delete(entity);
            CurrentSession.Flush();
        }

        /// <summary>
        /// Returns from the database the comment with the specified id.
        /// </summary>
        /// <param name="id">The id of the comment to return.</param>
        /// <returns>An instance of <see cref="TEntity"/> containing the comment from the database.</returns>
        public TEntity GetById(int id)
        {
            return CurrentSession.Get<TEntity>(id);
        }

        /// <summary>
        /// Returns from the database all the comments.
        /// </summary>
        /// <returns>An instance of <see cref="TEntity"/> containing all the requested comments.</returns>
        public IList<TEntity> GetAll()
        {
            return CurrentSession.QueryOver<TEntity>().List();
        }
    }
}
