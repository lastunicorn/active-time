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
using System.Windows;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Watchman;

namespace DustInTheWind.ActiveTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private Guard guard;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if !DEBUG
            if (CreateTheGuard())
                new Bootstrapper().Run();
            else
                Shutdown();
#else
            new Bootstrapper().Run();
#endif
        }

        private bool CreateTheGuard()
        {
            try
            {
                // Ensure that the application is started only once on the current machine.
                guard = new Guard("DustInTheWind.ActiveTime", GuardLevel.Machine);
            }
            catch (ActiveTimeException)
            {
                string message = "The application is already started. Current instance will not start.";
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
    }
}
