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

using System;
using System.Windows;

namespace DustInTheWind.ActiveTime.Bootstrapper;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : IDisposable
{
    private StartupGuard startupGuard;
    private BootstrapperWithAutofac bootstrapper;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        startupGuard = new StartupGuard();

        if (startupGuard.Start())
            StartApp();
        else
            Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        Dispose();
    }

    private void StartApp()
    {
        bootstrapper = new BootstrapperWithAutofac();
        bootstrapper.Run();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            startupGuard?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}