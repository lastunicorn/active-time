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

using System.Windows;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure.Watchman;

namespace DustInTheWind.ActiveTime.Infrastructure.Wpf;

public sealed class StartupGuard : IDisposable
{
    private Guard guard;

    public string Name { get; set; }

    public bool IsActiveInDebugMode { get; set; }

    public bool Start()
    {
        GuardLevel guardLevel = CalculateGuardLevel();

        try
        {
            guard = new Guard(Name, guardLevel);
        }
        catch (ActiveTimeException)
        {
            DisplayApplicationStartedErrorMessage();
            return false;
        }
        catch (Exception ex)
        {
            DisplayGenericErrorMessage(ex);
            return false;
        }

        return true;
    }

    private static void DisplayApplicationStartedErrorMessage()
    {
        const string message = "The application is already started. Current instance will not start.";
        MessageBox.Show(message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private static void DisplayGenericErrorMessage(Exception ex)
    {
        string message = $"Error creating the Guard.\nInternal error: {ex.Message}";
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private GuardLevel CalculateGuardLevel()
    {
#if DEBUG
        return IsActiveInDebugMode
            ? GuardLevel.Machine
            : GuardLevel.None;
#else
            return GuardLevel.Machine;
#endif
    }

    public void Dispose()
    {
        guard?.Dispose();
    }
}