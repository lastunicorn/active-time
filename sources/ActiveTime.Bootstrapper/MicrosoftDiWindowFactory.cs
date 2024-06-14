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

using System;
using System.Windows;
using DustInTheWind.ActiveTime.Presentation.Services;

namespace DustInTheWind.ActiveTime.Bootstrapper;

internal class MicrosoftDiWindowFactory : IWindowFactory
{
    private readonly IServiceProvider context;

    public MicrosoftDiWindowFactory(IServiceProvider context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Window Create(Type type)
    {
        return (Window)context.GetService(type);
    }
}