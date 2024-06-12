﻿// ActiveTime
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
using System.Windows;
using System.Windows.Input;
using DustInTheWind.ActiveTime.Presentation.MainArea.ViewModels;

namespace DustInTheWind.ActiveTime.Presentation.MainArea.Views
{
    /// <summary>
    /// Interaction logic for CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window
    {
        public CalendarWindow(CalendarViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            InitializeComponent();

            DataContext = viewModel;
        }

        private void calendar_OnGotMouseCapture(object sender, MouseEventArgs e)
        {
            UIElement originalElement = e.OriginalSource as UIElement;
            originalElement?.ReleaseMouseCapture();
        }
    }
}
