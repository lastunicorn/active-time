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
using DustInTheWind.ActiveTime.MainGuiModule.ViewModels;

namespace DustInTheWind.ActiveTime.MainGuiModule.Views
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        private readonly MessageViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWindow"/> class.
        /// </summary>
        public MessageWindow(MessageViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            this.viewModel = viewModel;

            InitializeComponent();
        }

        private void buttonSnooze_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MessageWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = viewModel;
        }
    }
}
