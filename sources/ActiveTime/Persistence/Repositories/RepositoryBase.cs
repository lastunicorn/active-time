using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace DustInTheWind.ActiveTime.Persistence
{
    abstract class RepositoryBase
    {
        protected ISessionFactory SessionFactory { get; set; }

        protected ISession CurrentSession
        {
            get { return SessionFactory.GetCurrentSession(); }
        }
    }
}
