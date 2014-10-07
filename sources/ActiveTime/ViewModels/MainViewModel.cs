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

using System.Reflection;
using DustInTheWind.ActiveTime.Common.UI;

namespace DustInTheWind.ActiveTime.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string windowTitle;

        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                windowTitle = value;
                NotifyPropertyChanged("WindowTitle");
            }
        }

        public MainViewModel()
        {
            windowTitle = BuildWindowTitle();
        }

        private static string BuildWindowTitle()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            AssemblyName assemblyName = assembly.GetName();

            return string.Format("ActiveTime {0}", assemblyName.Version.ToString(2));
        }
    }
}
