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

namespace DustInTheWind.ActiveTime.Domain.Services;

/// <summary>
/// This service has only one method that closes the application.
/// Before the application is closed, an event is published to announce
/// all the modules of this action.
/// </summary>
public abstract class ApplicationServiceBase : IApplicationService
{
    private readonly Dwarfs dwarfs;

    public DateTime? StartTime { get; private set; }

    public TimeSpan RunTime => StartTime == null
        ? TimeSpan.Zero
        : DateTime.Now - StartTime.Value;

    /// <summary>
    /// Event raised just before existing the application.
    /// This is an occasion for every module to shut down itself.
    /// </summary>
    public event EventHandler Exiting;

    protected ApplicationServiceBase()
    {
        dwarfs = new Dwarfs();
    }

    public void Start()
    {
        StartTime = DateTime.Now;
        dwarfs.StartAll();
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
            PerformExit();
            dwarfs.StopAll();
        }
        finally
        {
            StartTime = null;
        }
    }

    protected abstract void PerformExit();

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
}