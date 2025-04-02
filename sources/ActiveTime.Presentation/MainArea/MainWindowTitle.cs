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

using System.Reflection;

namespace DustInTheWind.ActiveTime.Presentation.MainArea;

public class MainWindowTitle
{
    private readonly string value;

    public MainWindowTitle()
    {
        value = BuildWindowTitle();
    }

    private static string BuildWindowTitle()
    {
        Assembly assembly = Assembly.GetEntryAssembly();
        AssemblyName assemblyName = assembly.GetName();

        return $"ActiveTime {assemblyName.Version.ToString(3)}";
    }

    public override string ToString()
    {
        return value;
    }
}