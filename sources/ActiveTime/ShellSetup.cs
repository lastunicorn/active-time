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

using DustInTheWind.ActiveTime.Domain.Presentation;
using DustInTheWind.ActiveTime.Infrastructure.Wpf.ShellEngine;
using DustInTheWind.ActiveTime.Presentation.AboutArea;
using DustInTheWind.ActiveTime.Presentation.MainArea;

namespace DustInTheWind.ActiveTime;

internal static class ShellSetup
{
    public static IEnumerable<ShellInfo> EnumerateShellInfo()
    {
        return new[]
        {
            new ShellInfo(ShellNames.MainShell, typeof(MainWindow)),
            new ShellInfo(ShellNames.MessageShell, typeof(MessageWindow), ShellNames.MainShell),
            new ShellInfo(ShellNames.AboutShell, typeof(AboutWindow), ShellNames.MainShell)
        };
    }
}