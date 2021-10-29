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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Watchman;

namespace DustInTheWind.ActiveTime.Bootstrapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IDisposable
    {
        private Guard guard;
        private BootstrapperWithAutofac bootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (CreateTheGuard())
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

        private bool CreateTheGuard()
        {
            GuardLevel guardLevel = CalculateGuardLevel();

            try
            {
                guard = new Guard("DustInTheWind.ActiveTime", guardLevel);
            }
            catch (ActiveTimeException)
            {
                const string message = "The application is already started. Current instance will not start.";
                MessageBox.Show(message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                return false;
            }
            catch (Exception ex)
            {
                string message = string.Format("Error creating the unique instance.\nInternal error: {0}", ex.Message);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        private static GuardLevel CalculateGuardLevel()
        {
#if DEBUG
            return GuardLevel.None;
#else
            return GuardLevel.Machine;
#endif
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (bootstrapper != null)
                {
                    //bootstrapper.Dispose();
                    bootstrapper = null;
                }

                if (guard != null)
                {
                    guard.Dispose();
                    guard = null;
                }
            }

            disposed = true;
        }

        ~App()
        {
            Dispose(false);
        }
    }
}
