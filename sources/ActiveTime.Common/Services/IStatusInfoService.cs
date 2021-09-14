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
using DustInTheWind.ActiveTime.Common.ApplicationStatuses;

namespace DustInTheWind.ActiveTime.Common.Services
{
    /// <summary>
    /// A service that stores different status messages.
    /// </summary>
    public interface IStatusInfoService
    {
        /// <summary>
        /// Event raised when the status text is changed.
        /// </summary>
        event EventHandler StatusTextChanged;

        /// <summary>
        /// Gets or sets the text representing the status.
        /// </summary>
        string StatusText { get; set; }

        /// <summary>
        /// Sets the status of the model to the specified Text and
        /// starts the timer that will reset it back to the default one.
        /// </summary>
        /// <param name="text">The Text to be set as status.</param>
        /// <param name="timeout">The Time in milliseconds after which the status will be reset to the default Text. If this Value is 0, the status will never be reset.</param>
        void SetStatus(string text, int timeout);

        void SetStatus(string text);

        void SetStatus(ApplicationStatus applicationStatus);
    }
}
