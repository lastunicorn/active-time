using System;
using Autofac;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime
{
    internal class AutofacUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ILifetimeScope container;

        public AutofacUnitOfWorkFactory(ILifetimeScope container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public IUnitOfWork CreateNew()
        {
            return container.Resolve<IUnitOfWork>();
        }
    }
}