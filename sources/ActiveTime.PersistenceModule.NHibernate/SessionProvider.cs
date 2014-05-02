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

using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace DustInTheWind.ActiveTime.PersistenceModule.NHibernate
{
    public static class SessionProvider
    {
        private const string databaseFileName = "db.s3db";

        private static ISessionFactory sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                    CreateSessionFactory();

                return sessionFactory;
            }
        }

        private static void CreateSessionFactory()
        {
            Configuration configuration = CreateConfiguration();
            configuration.AddAssembly(Assembly.GetExecutingAssembly());

            sessionFactory = configuration.BuildSessionFactory();
        }

        private static Configuration CreateConfiguration()
        {
            return new Configuration()
                .AddProperties(new Dictionary<string, string>
                                   {
                                       { Environment.ConnectionDriver, typeof(SQLite20Driver).FullName },
                                       { Environment.Dialect, typeof(SQLiteDialect).FullName },
                                       //{ Environment.ConnectionProvider, typeof(DriverConnectionProvider).FullName },
                                       //{ Environment.ConnectionString, string.Format("Data Source={0};Version=3;New=True;", DatabaseFile) },
                                       { Environment.ConnectionString, string.Format("Data Source={0};Version=3;", databaseFileName) },
                                       //{ Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName },
                                       { Environment.CurrentSessionContextClass, typeof(ThreadStaticSessionContext).AssemblyQualifiedName },
                                       //{ Environment.CurrentSessionContextClass, typeof(CurrentSessionContext).AssemblyQualifiedName },
                                       //{ Environment.Hbm2ddlAuto, "create" },
                                       //{ Environment.ShowSql, true.ToString() }
                                       { Environment.QuerySubstitutions, "true=1;false=0" },
                                       { Environment.ShowSql, "true" }
                                   });
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
