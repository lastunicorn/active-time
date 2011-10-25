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
using System.Windows;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Events;
using Microsoft.Practices.Prism.Events;

namespace DustInTheWind.ActiveTime.MainModule.Services
{
    /// <summary>
    /// This service has only one method that closes the application.
    /// Before the application is closed, an event is published to announce
    /// all the modules of this action.
    /// </summary>
    class ApplicationService : IApplicationService
    {
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationService"/> class.
        /// </summary>
        /// <param name="eventAggregator"></param>
        public ApplicationService(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Publishes the <see cref="ApplicationExitEvent"/> and then exits the application.
        /// </summary>
        public void Exit()
        {
            ApplicationExitEvent applicationExitEvent = eventAggregator.GetEvent<ApplicationExitEvent>();
            if (applicationExitEvent != null)
                applicationExitEvent.Publish(null);

            Application.Current.Shutdown();
        }
    }
}
