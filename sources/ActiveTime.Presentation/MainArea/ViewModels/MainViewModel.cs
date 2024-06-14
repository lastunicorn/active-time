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
using DustInTheWind.ActiveTime.Presentation.CalendarArea;
using DustInTheWind.ActiveTime.Presentation.Commands;
using DustInTheWind.ActiveTime.Presentation.MainMenuArea;
using DustInTheWind.ActiveTime.Presentation.OverviewArea;

namespace DustInTheWind.ActiveTime.Presentation.MainArea.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainMenuViewModel MainMenuViewModel { get; }

        public StatusInfoViewModel StatusInfoViewModel { get; }

        public FrontViewModel FrontViewModel { get; }

        public MainWindowTitle WindowTitle { get; } = new();

        public RefreshCommand RefreshCommand { get; }

        public OverviewViewModel OverviewViewModel { get; }

        public MainViewModel(MainMenuViewModel mainMenuViewModel, StatusInfoViewModel statusInfoViewModel, FrontViewModel frontViewModel,
            OverviewViewModel overviewViewModel, RefreshCommand refreshCommand)
        {
            MainMenuViewModel = mainMenuViewModel ?? throw new ArgumentNullException(nameof(mainMenuViewModel));
            StatusInfoViewModel = statusInfoViewModel ?? throw new ArgumentNullException(nameof(statusInfoViewModel));
            FrontViewModel = frontViewModel ?? throw new ArgumentNullException(nameof(frontViewModel));

            OverviewViewModel = overviewViewModel ?? throw new ArgumentNullException(nameof(overviewViewModel));

            RefreshCommand = refreshCommand ?? throw new ArgumentNullException(nameof(refreshCommand));
        }
    }
}