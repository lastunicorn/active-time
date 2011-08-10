using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using DustInTheWind.ActiveTime.Persistence.Entities;

namespace DustInTheWind.ActiveTime.UnitTests
{
    class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static string _databaseFileName = "db.s3db";

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    //var configuration = new Configuration();
                    //configuration.Configure();
                    var configuration = CreateConfiguration();
                    configuration.AddAssembly(typeof(Record).Assembly);
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static Configuration CreateConfiguration()
        {
            Configuration configuration = new Configuration()
             .AddProperties(new Dictionary<string, string>
                               {
                                   { Environment.ConnectionDriver, typeof(SQLite20Driver).FullName },
                                   { Environment.Dialect, typeof(SQLiteDialect).FullName },
                                   { Environment.ConnectionProvider, typeof(DriverConnectionProvider).FullName },
                                   //{ Environment.ConnectionString, string.Format("Data Source={0};Version=3;New=True;", DatabaseFile) },
                                   { Environment.ConnectionString, string.Format("Data Source={0};Version=3;", _databaseFileName) },
                                   //{ Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName },
                                   { Environment.CurrentSessionContextClass, typeof(ThreadStaticSessionContext).AssemblyQualifiedName },
                                   //{ Environment.CurrentSessionContextClass, typeof(CurrentSessionContext).AssemblyQualifiedName },
                                   //{ Environment.Hbm2ddlAuto, "create" },
                                   //{ Environment.ShowSql, true.ToString() }
                                   { Environment.QuerySubstitutions, "true=1;false=0" }
                               });
            return configuration;
        }
    }
}
