﻿// ActiveTime
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

namespace DustInTheWind.ActiveTime.Infrastructure.Watchman
{
    /// <summary>
    /// Specifies the level at which an instance of the <see cref="Guard"/> class will work.
    /// </summary>
    public enum GuardLevel
    {
        None,

        /// <summary>
        /// The instance of the <see cref="Guard"/> class will restrict the duplicates at the application level.
        /// That means that two instances with the same name can exists in two different applications running
        /// on the same machine.
        /// </summary>
        Application,

        /// <summary>
        /// The instance of the <see cref="Guard"/> class will restrict the duplicates at the machine level.
        /// That means that two instances with the same name are not allowed to exist in two different
        /// applications running on the same machine. But in the same application they are allowed.
        /// </summary>
        Machine
    }
}
