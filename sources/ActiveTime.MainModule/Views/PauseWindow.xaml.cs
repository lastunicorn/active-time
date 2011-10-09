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

using System.Windows;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using System.Collections.Generic;

namespace DustInTheWind.ActiveTime.MainModule.Views
{
    /// <summary>
    /// Interaction logic for PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window, IShell
    {
        //public PauseWindow()
        //{
        //    InitializeComponent();
        //}

        public PauseWindow()
        {
            InitializeComponent();

            textBlockMessage.Text = "No message for now.\n\nPlease go back to what you were doing.";
        }

        private void buttonSnooze_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void UpdateTextFromNavigationParameters()
        {
            if (navigationParameters != null && navigationParameters.ContainsKey("Text") && navigationParameters["Text"] is string)
            {
                textBlockMessage.Text = navigationParameters["Text"] as string;
            }
        }
    }
}
