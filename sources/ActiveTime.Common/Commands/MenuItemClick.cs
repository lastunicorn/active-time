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
using System.Windows.Controls;
using System.Windows.Input;

namespace DustInTheWind.ActiveTime.Common.Commands
{
    /// <summary>
    /// Static Class that holds all Dependency Properties and Static methods to allow 
    /// the Click event of the MenuItem class to be attached to a Command. 
    /// </summary>
    public static class MenuItemClick
    {
        #region Command

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(MenuItemClick), new PropertyMetadata(OnSetCommandCallback));

        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            MenuItem menuItem = dependencyObject as MenuItem;

            if (menuItem != null)
            {
                MenuItemClickCommandBehavior behavior = GetOrCreateBehavior(menuItem);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        public static ICommand GetCommand(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new System.ArgumentNullException("menuItem");

            return menuItem.GetValue(CommandProperty) as ICommand;
        }

        public static void SetCommand(MenuItem menuItem, ICommand command)
        {
            if (menuItem == null)
                throw new System.ArgumentNullException("menuItem");

            menuItem.SetValue(CommandProperty, command);
        }

        #endregion

        #region CommandParameter

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter", typeof(object), typeof(MenuItemClick), new PropertyMetadata(OnSetCommandParameterCallback));

        private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            MenuItem menuItem = dependencyObject as MenuItem;

            if (menuItem != null)
            {
                MenuItemClickCommandBehavior behavior = GetOrCreateBehavior(menuItem);
                behavior.CommandParameter = e.NewValue;
            }
        }

        public static object GetCommandParameter(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new System.ArgumentNullException("menuItem");

            return menuItem.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(MenuItem menuItem, object parameter)
        {
            if (menuItem == null)
                throw new System.ArgumentNullException("menuItem");

            menuItem.SetValue(CommandParameterProperty, parameter);
        }

        #endregion

        #region CommandBehavior

        private static readonly DependencyProperty ClickCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "ClickCommandBehavior", typeof(MenuItemClickCommandBehavior), typeof(MenuItemClick), null);

        private static MenuItemClickCommandBehavior GetOrCreateBehavior(MenuItem menuItem)
        {
            MenuItemClickCommandBehavior behavior = menuItem.GetValue(ClickCommandBehaviorProperty) as MenuItemClickCommandBehavior;

            if (behavior == null)
            {
                behavior = new MenuItemClickCommandBehavior(menuItem);
                menuItem.SetValue(ClickCommandBehaviorProperty, behavior);
            }

            return behavior;
        }

        #endregion
    }

}
