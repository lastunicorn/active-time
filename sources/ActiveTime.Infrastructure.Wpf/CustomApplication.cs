// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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

using System.Reflection;
using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using DustInTheWind.ActiveTime.Infrastructure.Watchman;
using DustInTheWind.ActiveTime.Infrastructure.Wpf.ShellEngine;

namespace DustInTheWind.ActiveTime.Infrastructure.Wpf;

public class CustomApplication : IApplication, IDisposable
{
    private StartupGuard startupGuard;

    public GuardConfiguration GuardConfiguration { get; } = new();
    
    public JobCollection Jobs { get; } = new();

    public IShellNavigator ShellNavigator { get; set; }

    public DateTime? StartTime { get; private set; }

    public TimeSpan RunTime => StartTime == null
        ? TimeSpan.Zero
        : DateTime.Now - StartTime.Value;

    /// <summary>
    /// Event raised just before existing the application.
    /// This is an occasion for every module to shut down itself.
    /// </summary>
    public event EventHandler Exiting;

    public void Start()
    {
        StartTime = DateTime.Now;

        if (GuardConfiguration.IsEnabled)
        {
            startupGuard = new StartupGuard
            {
                Name = GuardConfiguration.Name,
                IsActiveInDebugMode = GuardConfiguration.IsActiveInDebugMode
            };

            bool guardStartedSuccessfully = startupGuard.Start();

            if (!guardStartedSuccessfully)
            {
                System.Windows.Application.Current.Shutdown();
                return;
            }
        }

        Jobs.StartAll();
    }

    public void Exit()
    {
        try
        {
            OnExiting(EventArgs.Empty);
        }
        catch
        {
        }

        try
        {
            System.Windows.Application.Current.Shutdown();
        }
        finally
        {
            StartTime = null;
        }
    }

    public Version GetVersion()
    {
        Assembly assembly = Assembly.GetEntryAssembly();
        AssemblyName assemblyName = assembly.GetName();
        return assemblyName.Version;
    }

    /// <summary>
    /// Raises the Exiting event.
    /// </summary>
    /// <param name="e">An EventArgs that contains the event data.</param>
    protected virtual void OnExiting(EventArgs e)
    {
        Exiting?.Invoke(this, e);
    }

    public void Dispose()
    {
        startupGuard?.Dispose();
    }
}