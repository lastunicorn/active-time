// ActiveTime
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

using System.Reflection;
using System.Windows;
using Autofac;
using DustInTheWind.ActiveTime.Presentation.Services;

namespace DustInTheWind.ActiveTime;

internal class AutofacWindowFactory : IWindowFactory
{
    private readonly IComponentContext context;

    public AutofacWindowFactory(IComponentContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Window Create(Type type)
    {
        ILifetimeScope parentLifetimeScope = context.Resolve<ILifetimeScope>();
        return (Window)parentLifetimeScope.Resolve(type);
    }
}