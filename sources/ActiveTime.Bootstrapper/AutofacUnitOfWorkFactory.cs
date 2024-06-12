﻿// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using Autofac;
using DustInTheWind.ActiveTime.Ports.Persistence;

namespace DustInTheWind.ActiveTime;

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