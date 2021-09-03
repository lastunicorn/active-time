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
using System.Reflection;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainMenuViewModel MainMenuViewModel { get; }
        
        public StatusInfoViewModel StatusInfoViewModel { get; }
        
        public FrontViewModel FrontViewModel { get; }

        private string windowTitle;

        public string WindowTitle
        {
            get => windowTitle;
            set
            {
                windowTitle = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(MainMenuViewModel mainMenuViewModel, StatusInfoViewModel statusInfoViewModel, FrontViewModel frontViewModel)
        {
            MainMenuViewModel = mainMenuViewModel ?? throw new ArgumentNullException(nameof(mainMenuViewModel));
            StatusInfoViewModel = statusInfoViewModel ?? throw new ArgumentNullException(nameof(statusInfoViewModel));
            FrontViewModel = frontViewModel ?? throw new ArgumentNullException(nameof(frontViewModel));

            windowTitle = BuildWindowTitle();
        }

        private static string BuildWindowTitle()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            AssemblyName assemblyName = assembly.GetName();

            return $"ActiveTime {assemblyName.Version.ToString(3)}";
        }
    }
}