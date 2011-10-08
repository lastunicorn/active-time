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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using DustInTheWind.ActiveTime.Common;

namespace DustInTheWind.ActiveTime.PersistenceModule.ModuleDefinitions
{
    public class PersistenceModule : IModule
    {
        private IUnityContainer unityContainer;

        public PersistenceModule(IUnityContainer unityContainer)
        {
            if (unityContainer == null)
                throw new ArgumentNullException("unityContainer");

            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            //unityContainer.RegisterType<ITimeRecordRepository, TimeRecordRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<ITimeRecordRepository>(new TimeRecordRepository(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IDayCommentRepository>(new DayCommentRepository(), new ContainerControlledLifetimeManager());
        }
    }
}
