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
using System.Collections.Generic;
using System.Windows;
using DustInTheWind.ActiveTime.Domain.Presentation.ShellNavigation;

namespace DustInTheWind.ActiveTime.Presentation.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Need to make this class thread safe.
    /// </remarks>
    public class ShellNavigator : IShellNavigator
    {
        private readonly IWindowFactory windowFactory;
        private readonly DispatcherService dispatcherService;

        private readonly Dictionary<string, ShellInfo> shellInfos = new Dictionary<string, ShellInfo>();
        private readonly Dictionary<string, Window> windows = new Dictionary<string, Window>();

        /// <summary>
        /// Initialize a new instance of the <see cref="ShellNavigator"/> class.
        /// </summary>
        public ShellNavigator(IWindowFactory windowFactory, DispatcherService dispatcherService)
        {
            this.windowFactory = windowFactory ?? throw new ArgumentNullException(nameof(windowFactory));
            this.dispatcherService = dispatcherService ?? throw new ArgumentNullException(nameof(dispatcherService));
        }

        /// <summary>
        /// Adds a new <see cref="ShellInfo"/> object containing information about a shell.
        /// This object is necessary when a new shell is created.
        /// </summary>
        /// <param name="shellInfo">The <see cref="ShellInfo"/> object containing the information about a shell.</param>
        public void RegisterShell(ShellInfo shellInfo)
        {
            if (shellInfos.ContainsKey(shellInfo.ShellName))
                throw new Exception("Another shell with the same name already exists.");

            shellInfos.Add(shellInfo.ShellName, shellInfo);
        }

        /// <summary>
        /// Navigates to the shell with the specified name. If the shell does not exists,
        /// a new instance is attempted to be created using the <see cref="ShellInfo"/> object
        /// previously registered.
        /// </summary>
        /// <param name="shellName">The name of the shell to navigate to.</param>
        /// <param name="parameters">The list of parameters to be passed to the destination shell.</param>
        public void Navigate(string shellName, Dictionary<string, object> parameters = null)
        {
            if (shellName == null) throw new ArgumentNullException(nameof(shellName));

            if (windows.ContainsKey(shellName))
            {
                NavigateInternal(shellName, parameters);
            }
            else if (shellInfos.ContainsKey(shellName))
            {
                CreateNewShell(shellInfos[shellName]);
                NavigateInternal(shellName, parameters);
            }
            else
            {
                throw new ArgumentException("Invalid shell name.", nameof(shellName));
            }
        }

        private void CreateNewShell(ShellInfo shellInfo)
        {
            Window window = null;

            dispatcherService.Dispatch(() =>
            {
                window = windowFactory.Create(shellInfo.ShellType);

                bool existsOwnerWindow = !string.IsNullOrEmpty(shellInfo.OwnerName) && windows.ContainsKey(shellInfo.OwnerName);

                if (existsOwnerWindow)
                    window.Owner = windows[shellInfo.OwnerName];
            });

            window.Closed += (s, e) =>
            {
                windows.Remove(shellInfo.ShellName);
            };

            windows.Add(shellInfo.ShellName, window);
        }

        private void NavigateInternal(string shellName, Dictionary<string, object> parameters)
        {
            bool existsWindow = windows.ContainsKey(shellName);

            if (!existsWindow)
                return;

            Window window = windows[shellName];

            dispatcherService.Dispatch(() =>
            {
                window.Show();
                window.Activate();

                IShell shell = (window as IShell) ?? (window.DataContext as IShell);

                if (shell != null)
                    shell.NavigationParameters = parameters;
            });
        }
    }
}