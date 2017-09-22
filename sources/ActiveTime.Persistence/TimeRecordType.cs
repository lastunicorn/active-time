// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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

namespace DustInTheWind.ActiveTime.Persistence
{
    /// <summary>
    /// Specifies the type of a TimeRecord.
    /// </summary>
    public enum TimeRecordType
    {
        Unknown = 0,

        /// <summary>
        /// The TimeRecord was created normally, by the Recorder.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// The TimeRecord was created manually.
        /// </summary>
        Fake = 2
    }
}
