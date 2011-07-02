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
        /// When implemented by a derrived class, it initializes the exporter.
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
