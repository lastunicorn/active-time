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

using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.ShellNavigation;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class MessageViewModel : ViewModelBase, IShell
    {
        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged("Message");
            }
        }

        private Dictionary<string, object> navigationParameters;
        public Dictionary<string, object> NavigationParameters
        {
            get { return navigationParameters; }
            set
            {
                navigationParameters = value;
                UpdateTextFromNavigationParameters();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageViewModel"/> class.
        /// </summary>
        public MessageViewModel()
        {
            message = "No message for now.\n\nPlease go back to what you were doing.";
        }

        private void UpdateTextFromNavigationParameters()
        {
            if (navigationParameters != null && navigationParameters.ContainsKey("Text") && navigationParameters["Text"] is string)
            {
                Message = navigationParameters["Text"] as string;
            }
        }
    }
}
