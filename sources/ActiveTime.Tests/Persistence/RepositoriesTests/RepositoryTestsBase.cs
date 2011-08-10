using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests
{
    public  class RepositoryTestsBase
    {
        protected ISessionFactory sessionFactory;
        protected ISession currentSession;

        [SetUp]
        public void SetUp()
        {
            sessionFactory = new Configuration().Configure().BuildSessionFactory();
            currentSession = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(currentSession);

            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            currentSession.Transaction.Rollback();
            CurrentSessionContext.Unbind(sessionFactory);

            OnTearDown();
        }

        protected virtual void OnTearDown()
        {
        }
    }
}
