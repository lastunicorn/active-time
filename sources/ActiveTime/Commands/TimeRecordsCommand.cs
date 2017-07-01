// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.Commands
{
    internal class TimeRecordsCommand : ICommand
    {
        private readonly IRegionManager regionManager;
        private readonly IStateService stateService;
        public event EventHandler CanExecuteChanged;

        public TimeRecordsCommand(IRegionManager regionManager, IStateService stateService)
        {
            if (regionManager == null) throw new ArgumentNullException(nameof(regionManager));
            if (stateService == null) throw new ArgumentNullException(nameof(stateService));

            this.regionManager = regionManager;
            this.stateService = stateService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (stateService.CurrentDate == null)
                return;

            //ClearRegion(RegionNames.MainContentRegion);
            //ClearRegion(RegionNames.RecordsRegion);
            //regionManager.Regions.Remove(RegionNames.RecordsRegion);

            regionManager.RequestNavigate(RegionNames.RecordsRegion, ViewNames.DayRecordsView);
        }

        private void ClearRegion(string regionName)
        {
            IRegion mainContentRegion = regionManager.Regions[regionName];
            IViewsCollection activeViewsInRegion = mainContentRegion.ActiveViews;

            foreach (object view in activeViewsInRegion)
            {
                mainContentRegion.Remove(view);
            }
        }
    }
}
