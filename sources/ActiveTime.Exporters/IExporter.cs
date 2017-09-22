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

using System.Collections.Generic;
using System.IO;
using DustInTheWind.ActiveTime.Recording;

namespace DustInTheWind.ActiveTime.Exporters
{
    /// <summary>
    /// Represents a class that exports the provided records to an external format.
    /// </summary>
    public interface IExporter
    {
        /// <summary>
        /// Gets a string that identifies the exporter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a friendly name to be displayed to the user.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets a short description of the exporter.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// When implemented by a derived class, it initializes the exporter.
        /// </summary>
        /// <param name="parameters">A list of parameters needed to initialize the exporter.</param>
        void Initialize(Dictionary<string, object> parameters);

        /// <summary>
        /// Resets the exporter and prepares it for a new export process.
        /// </summary>
        void Reset();

        /// <summary>
        /// Writes the header for the new export.
        /// </summary>
        /// <param name="sw"></param>
        void ExportHeader(StreamWriter sw);

        /// <summary>
        /// Writes the footer of the export.
        /// </summary>
        /// <param name="sw"></param>
        void ExportFooter(StreamWriter sw);

        /// <summary>
        /// Writes a day record.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="dayRecord">The record that should be exported.</param>
        void ExportDayRecord(StreamWriter sw, DayRecord dayRecord);
    }
}
