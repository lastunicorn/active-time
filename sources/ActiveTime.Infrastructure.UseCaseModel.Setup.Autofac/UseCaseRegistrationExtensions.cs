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
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine.MediatR.Setup.Autofac;

public static class UseCaseRegistrationExtensions
{
    public static void RegisterUseCases(this ContainerBuilder containerBuilder, params Assembly[] assemblies)
    {
        // MediatR
        MediatRConfiguration mediatRConfiguration = MediatRConfigurationBuilder.Create(assemblies)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
        containerBuilder.RegisterMediatR(mediatRConfiguration);

        // Services
        containerBuilder.RegisterType<EventBus>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<MediatrRequestBus>().As<IRequestBus>().SingleInstance();
    }
}