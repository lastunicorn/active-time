using System;
using Autofac;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime
{
    internal class AutofacUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ILifetimeScope context;

        public AutofacUnitOfWorkFactory(ILifetimeScope context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork CreateNew()
        {
            ILifetimeScope newContext = context.BeginLifetimeScope();
            return newContext.Resolve<IUnitOfWork>();
        }
    }
}