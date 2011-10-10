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
using System.Collections.Generic;
using System.Windows;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.ShellNavigationModule.Services
{
    /// <summary>
    /// 
    /// </summary>
    class ShellNavigator : IShellNavigator
    {
        private readonly IUnityContainer unityContainer;
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The list of shell info objects indexed by the shell's name.
        /// These objects are used to create shells.
        /// </summary>
        private Dictionary<string, ShellInfo> shellInfos = new Dictionary<string, ShellInfo>();

        /// <summary>
        /// The list of existing shells.
        /// </summary>
        private Dictionary<string, Window> shells = new Dictionary<string, Window>();

        /// <summary>
        /// Initialize a new instance of the <see cref="ShellNavigator"/> class.
        /// </summary>
        public ShellNavigator(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            if (unityContainer == null)
                throw new ArgumentNullException("unityContainer");

            if (regionManager == null)
                throw new ArgumentNullException("regionManager");

            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
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
        public void Navigate(string shellName, Dictionary<string, object> parameters = null)
        {
            if (shellName == null)
                throw new ArgumentNullException("shellName");

            if (shells.ContainsKey(shellName))
            {
                NavigateInternal(shellName, parameters);
            }
            else if (shellInfos.ContainsKey(shellName))
            {
                CreateNewShell(shellInfos[shellName]);
                NavigateInternal(shellName, parameters);
            }
        }

        /// <summary>
        /// Creates a new shell using the provided <see cref="ShellInfo"/> object.
        /// </summary>
        /// <param name="shellInfo">The <see cref="ShellInfo"/> object containing the information about the shell that needs to be created.</param>
        private void CreateNewShell(ShellInfo shellInfo)
        {
            Window shell = (Window)unityContainer.Resolve(shellInfo.ShellType);
            RegionManager.SetRegionManager(shell, regionManager);
            RegionManager.UpdateRegions();

            if (!string.IsNullOrEmpty(shellInfo.OwnerName) && shells.ContainsKey(shellInfo.OwnerName))
                shell.Owner = shells[shellInfo.OwnerName];

            shell.Closed += (s, e) =>
                                {
                                    shells.Remove(shellInfo.ShellName);
                                    RegionManager.SetRegionManager(shell, null);
                                    RegionManager.UpdateRegions();
                                };

            shells.Add(shellInfo.ShellName, shell);
        }

        private void NavigateInternal(string shellName, Dictionary<string, object> parameters)
        {
            Window shell = shells[shellName];

            if (shell is IShell)
                ((IShell)shell).NavigationParameters = parameters;
            else if (shell.DataContext is IShell)
                ((IShell)shell.DataContext).NavigationParameters = parameters;

            shell.Show();
            shell.Activate();
        }
    }
}
