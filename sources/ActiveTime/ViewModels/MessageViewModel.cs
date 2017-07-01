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

using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.Properties;

namespace DustInTheWind.ActiveTime.ViewModels
{
    public class MessageViewModel : ViewModelBase, IShell
    {
        private string message;
        private Dictionary<string, object> navigationParameters;

        public string Message
        {
            get { return message; }
            private set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, object> NavigationParameters
        {
            get { return navigationParameters; }
            set
            {
                navigationParameters = value;
                UpdateTextFromNavigationParameters();
            }
        }

        public MessageViewModel()
        {
            message = Resources.MessageWindow_DefaultText;
        }

        private void UpdateTextFromNavigationParameters()
        {
            if (navigationParameters == null)
                return;

            bool containsTextKey = navigationParameters.ContainsKey("Text") && navigationParameters["Text"] is string;

            if (containsTextKey)
                Message = (string)navigationParameters["Text"];
        }
    }
}
