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
    public class RepositoryTestsBase
    {
        private ISessionFactory sessionFactory;
        protected ISessionFactory SessionFactory
        {
            get { return sessionFactory; }
        }

        private ISession session;
        protected ISession Session
        {
            get { return session; }
        }

        [SetUp]
        public void SetUp()
        {
            //sessionFactory = new Configuration().Configure().BuildSessionFactory();
            sessionFactory = NHibernateHelper.SessionFactory;
            session = sessionFactory.OpenSession();
            ITransaction transaction = session.BeginTransaction();
            CurrentSessionContext.Bind(session);

            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            session.Transaction.Rollback();
            CurrentSessionContext.Unbind(sessionFactory);

            OnTearDown();
        }

        protected virtual void OnTearDown()
        {
        }

        protected void AssertRecordExistsInDb(string tableName, string idName, string idValue)
        {
            string sql = string.Format("select count(*) as c from {0} as t where t.{1} = {2}", tableName, idName, idValue);
            ISQLQuery query = Session.CreateSQLQuery(sql)
                .AddScalar("c", NHibernateUtil.Int64);

            IList<long> results = query.List<long>();
            long recordCount = results.Count > 0 ? results[0] : 0;

            Assert.That(recordCount, Is.EqualTo(1));
        }
    }
}
