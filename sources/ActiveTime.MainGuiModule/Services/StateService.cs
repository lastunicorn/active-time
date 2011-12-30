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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.MainGuiModule.Services
{
    class StateService : IStateService
    {
        private readonly IStatusInfoService statusInfoService;

        private DateTime? currentDate;
        public DateTime? CurrentDate
        {
            get { return currentDate; }
            set
            {
                currentDate = value;
                statusInfoService.SetStatus("Date changed.");
                OnCurrentDateChanged(EventArgs.Empty);
            }
        }


        public event EventHandler CurrentDateChanged;

        protected virtual void OnCurrentDateChanged(EventArgs e)
        {
            if (CurrentDateChanged != null)
                CurrentDateChanged(this, e);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="StateService"/> class.
        /// </summary>
        public StateService(IStatusInfoService statusInfoService)
        {
            if (statusInfoService == null)
                throw new ArgumentNullException("statusInfoService");

            this.statusInfoService = statusInfoService;

            currentDate = DateTime.Today;
        }
    }
}
