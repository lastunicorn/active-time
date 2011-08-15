using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NHibernate;
using NHibernate.Exceptions;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests
{
    public abstract class RepositoryCrudTestsBase<TEntity, TRepository> : RepositoryTestsBase
        where TEntity : EntityBase
        where TRepository : CrudRepositoryBase<TEntity>
    {
        private TEntity entity;
        private TRepository repository;

        protected override void OnSetUp()
        {
            base.OnSetUp();

            repository = CreateRepository();
            entity = CreateEntity();
        }

        [Test]
        public virtual void TestSelectQueryWorks()
        {
            CurrentSession.Save(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            CurrentSession.QueryOver<TEntity>()
                .Take(5)
                .List();
        }

        #region Create

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAdd_Null()
        {
            // Execute
            repository.Add(null);
        }

        [Test]
        public void TestAdd_CheckIfExistsInDb()
        {
            // Execute
            repository.Add(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            Assert.That(entity.Id, Is.GreaterThan(0));
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNotNull(reloadedEntity);
            AssertAreEqual(entity, reloadedEntity);
        }

        /// <summary>
        /// Adding an already existent entity should update it.
        /// </summary>
        [Test]
        [ExpectedException(typeof(GenericADOException))]
        public void TestAdd_DuplicateBusinessKey()
        {
            if (!HasBusinessKey)
                Assert.Inconclusive("The entity does not have a business key.");

            repository.Add(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            repository.Add(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            Assert.That(entity.Id, Is.GreaterThan(0));
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNotNull(reloadedEntity);
            AssertAreEqual(entity, reloadedEntity);
        }

        #endregion

        #region Retrieve

        [Test]
        public void TestGetByIdOk()
        {
            // Prepare
            CurrentSession.Save(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Execute
            TEntity reloadedEntity = repository.GetById(entity.Id);

            // Verify
            Assert.IsNotNull(reloadedEntity);
            AssertAreEqual(entity, reloadedEntity);
        }

        [Test]
        public void TestGetById_Inexisting()
        {
            // Execute
            TEntity reloadedEntity = repository.GetById(-100);

            // Verify
            Assert.IsNull(reloadedEntity);
        }

        #endregion

        #region Update

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUpdate_Null()
        {
            // Execute
            repository.Update(null);
        }

        [Test]
        public void TestUpdate_EntityWasUpdatedInDb()
        {
            // Prepare
            CurrentSession.Save(entity);
            CurrentSession.Flush();
            ModifyAllButBusinessKey(entity);
            if (IsBusinessKeyMutable)
                ModifyBusinessKey(entity);

            // Execute
            repository.Update(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            Assert.That(entity.Id, Is.GreaterThan(0));
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNotNull(reloadedEntity);
            AssertAreEqual(entity, reloadedEntity);
        }

        [Test]
        [ExpectedException(typeof(HibernateException))]
        public void TestUpdate_ModifyImmutableBusinessKey()
        {
            if (!HasBusinessKey)
                Assert.Inconclusive("The entity does not have a business key.");

            if (IsBusinessKeyMutable)
                Assert.Inconclusive("The business key of the entity is mutable.");

            // Prepare
            CurrentSession.Save(entity);
            CurrentSession.Flush();
            ModifyBusinessKey(entity);

            // Execute
            repository.Update(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            Assert.That(entity.Id, Is.GreaterThan(0));
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNotNull(reloadedEntity);
            AssertAreEqual(entity, reloadedEntity);
        }

        /// <summary>
        /// Updating an inexistent entity should add it to the database.
        /// </summary>
        [Test]
        [ExpectedException()]
        public void TestUpdate_InexistentRecord()
        {
            // Execute
            repository.Update(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNotNull(reloadedEntity);
            AssertAreEqual(entity, reloadedEntity);
        }

        #endregion

        #region Delete

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDelete_Null()
        {
            repository.Delete(null);
        }

        [Test]
        public void TestDelete_CheckEntityWasDeletedInDb()
        {
            // Prepare
            CurrentSession.Save(entity);
            CurrentSession.Flush();

            // Execute
            repository.Delete(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNull(reloadedEntity);
        }

        [Test]
        public void TestDelete_Inexistent()
        {
            // Execute
            repository.Delete(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNull(reloadedEntity);
        }

        #endregion


        #region Create Or Update

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddOrUpdate_Null()
        {
            repository.AddOrUpdate(null);
        }

        [Test]
        public void TestAddOrUpdate_Add_InDb()
        {
            // Execute
            repository.AddOrUpdate(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            Assert.That(entity.Id, Is.GreaterThan(0));
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNotNull(reloadedEntity);
            AssertAreEqual(entity, reloadedEntity);
        }

        [Test]
        public void TestAddOrUpdate_Update_InDb()
        {
            // Prepare
            CurrentSession.Save(entity);
            CurrentSession.Flush();
            ModifyAllButBusinessKey(entity);

            // Execute
            repository.AddOrUpdate(entity);
            CurrentSession.Flush();
            CurrentSession.Evict(entity);

            // Verify
            Assert.That(entity.Id, Is.GreaterThan(0));
            TEntity reloadedEntity = CurrentSession.Get<TEntity>(entity.Id);
            Assert.IsNotNull(reloadedEntity);
            AssertAreEqual(entity, reloadedEntity);
        }

        #endregion

        #region RetrieveAll

        [Test]
        public void TestGetAll([Range(0, 3)]int count)
        {
            // Prepare
            ICollection<TEntity> expectedEntities = CreateEntities(10);
            foreach (TEntity entity in expectedEntities)
            {
                CurrentSession.Save(entity);
                CurrentSession.Flush();
                CurrentSession.Evict(entity);
            }

            // Execute
            ICollection<TEntity> actualEntities = repository.GetAll();


            AssertAreEquivalent(expectedEntities, actualEntities);
        }

        #endregion

        protected abstract TRepository CreateRepository();
        protected abstract TEntity CreateEntity();
        protected abstract ICollection<TEntity> CreateEntities(int count);
        protected abstract void ModifyAllButBusinessKey(TEntity entity);
        protected abstract bool AreEquals(TEntity a, TEntity b);

        protected abstract bool HasBusinessKey { get; }
        protected abstract bool IsBusinessKeyMutable { get; }
        protected abstract void ModifyBusinessKey(TEntity entity);

        #region Assert Util

        private void AssertAreEqual(TEntity expectedEntity, TEntity actualEntity)
        {
            if (!AreEquals(expectedEntity, actualEntity))
                Assert.Fail("The two entities are not equal.");
        }

        private void AssertAreEquivalent(ICollection<TEntity> expectedEntities, ICollection<TEntity> actualEntities)
        {
            Assert.That(actualEntities.Count == expectedEntities.Count);

            foreach (TEntity entity in actualEntities)
            {
                if (!expectedEntities.Contains(entity, new EntityComparer<TEntity> { Comparer = AreEquals }))
                {
                    Assert.Fail("The lists are not equivalent.");
                }
            }
        }

        private class EntityComparer<T> : IEqualityComparer<T>
            where T : EntityBase
        {
            public Func<T, T, bool> Comparer;

            public bool Equals(T x, T y)
            {
                return Comparer(x, y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

        #endregion
    }
}