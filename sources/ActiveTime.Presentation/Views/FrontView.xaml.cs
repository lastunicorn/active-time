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
using System.Windows;
using System.Windows.Controls;
using DustInTheWind.ActiveTime.Presentation.ViewModels;

namespace DustInTheWind.ActiveTime.Presentation.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class FrontView : UserControl
    {
        public FrontView()
        {
            InitializeComponent();
        }

        //private void MainView_OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    DataContext = viewModel;
        //    viewModel.RefreshCommand.Execute(null);
        //}
    }
}
