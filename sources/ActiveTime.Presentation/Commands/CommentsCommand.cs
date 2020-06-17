// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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
using DustInTheWind.ActiveTime.Common.Presentation;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.Presentation.Commands
{
    public class CommentsCommand : CommandBase
    {
        private readonly IRegionManager regionManager;

        public CommentsCommand(IRegionManager regionManager)
        {
            this.regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
        }

        public override void Execute(object parameter)
        {
            //ClearRegion(RegionNames.MainContentRegion);
            //ClearRegion(RegionNames.RecordsRegion);
            //regionManager.Regions.Remove(RegionNames.RecordsRegion);

            regionManager.RequestNavigate(RegionNames.RecordsRegion, ViewNames.CommentsView);
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