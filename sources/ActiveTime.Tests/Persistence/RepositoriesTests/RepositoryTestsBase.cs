using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NUnit.Framework;
using DustInTheWind.ActiveTime.Persistence;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests
{
    public abstract class RepositoryTestsBase
    {
        private ISession currentSession;

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

        [SetUp]
        public void SetUp()
        {
            // Force to create a session.
            ITransaction transaction = CurrentSession.BeginTransaction();

            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            CurrentSession.Transaction.Rollback();
            CurrentSession.Close();
            currentSession = null;

            OnTearDown();
        }

        protected virtual void OnTearDown()
        {
        }
    }
}
