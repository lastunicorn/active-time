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
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.ShellNavigationModule.Services
{
    class ShellNavigator : IShellNavigator
    {
        private IUnityContainer unityContainer;
        private Dictionary<string, ShellInfo> shellInfos = new Dictionary<string, ShellInfo>();
        private Dictionary<string, Window> shells = new Dictionary<string, Window>();

        public ShellNavigator(IUnityContainer unityContainer)
        {
            if (unityContainer == null)
                throw new ArgumentNullException("unityContainer");

            this.unityContainer = unityContainer;
        }

        public void RegisterShell(ShellInfo shellInfo)
        {
            if (shellInfos.ContainsKey(shellInfo.ShellName))
                throw new Exception("Another shell with the same name already exists.");

            shellInfos.Add(shellInfo.ShellName, shellInfo);
        }

        public void Navigate(string shellName)
        {
            if (shellName == null)
                throw new ArgumentNullException("shellName");

            if (shells.ContainsKey(shellName))
            {
                NavigateInternal(shellName);
            }
            else if (shellInfos.ContainsKey(shellName))
            {
                CreateNewShell(shellInfos[shellName]);
                NavigateInternal(shellName);
            }
        }

        private void CreateNewShell(ShellInfo shellInfo)
        {
            Window shell = (Window)unityContainer.Resolve(shellInfo.ShellType);

            if (!string.IsNullOrEmpty(shellInfo.OwnerName) && shells.ContainsKey(shellInfo.OwnerName))
                shell.Owner = shells[shellInfo.OwnerName];

            shell.Closed += (s, e) => shells.Remove(shellInfo.ShellName);

            shells.Add(shellInfo.ShellName, shell);
        }

        private void NavigateInternal(string shellName)
        {
            Window shell = shells[shellName];
            shell.Show();
            shell.Activate();
        }
    }
}
