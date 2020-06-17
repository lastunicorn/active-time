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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace DustInTheWind.ActiveTime.Presentation.Commands
{
    /// <summary>
    /// Behavior that allows <see cref="MenuItem"/>s to hook up with <see cref="ICommand"/> objects.
    /// </summary>
    public class MenuItemClickCommandBehavior : CommandBehaviorBase<MenuItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemClickCommandBehavior"/> class and hooks up the Click event of 
        /// <paramref name="menuItem"/> to the ExecuteCommand() method. 
        /// </summary>
        /// <param name="menuItem">The menu item object.</param>
        public MenuItemClickCommandBehavior(MenuItem menuItem)
            : base(menuItem)
        {
            menuItem.Click += HandleMenuItemClick;
        }

        private void HandleMenuItemClick(object sender, RoutedEventArgs e)
        {
            ExecuteCommand();
        }
    }
}
