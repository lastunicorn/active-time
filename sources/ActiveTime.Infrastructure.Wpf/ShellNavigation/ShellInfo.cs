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

namespace DustInTheWind.ActiveTime.Domain.Presentation.ShellNavigation;

public class ShellInfo
{
    public Type ShellType { get; }

    public string ShellName { get; }

    public string OwnerName { get; }

    public ShellInfo(string shellName, Type shellType, string ownerName = null)
    {
        ShellName = shellName ?? throw new ArgumentNullException(nameof(shellName));
        ShellType = shellType ?? throw new ArgumentNullException(nameof(shellType));
        OwnerName = ownerName;
    }
}