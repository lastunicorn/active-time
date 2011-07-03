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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace DustInTheWind.ActiveTime.Exporters
{
    /// <summary>
    /// This class loads the exporters and keeps a list with the instances for later usage.
    /// </summary>
    public class ExportersManager
    {
        /// <summary>
        /// The name of the directory where the exporter plug-ins are stored.
        /// </summary>
        private const string EXPORTERS_DIRECTORY_NAME = "Exporters";

        /// <summary>
        /// The list of exporters.
        /// </summary>
        [ImportMany]
        private List<IExporter> exporters = new List<IExporter>();

        /// <summary>
        /// The exporters indexed by name.
        /// </summary>
        private Dictionary<string, IExporter> exportersByName = new Dictionary<string, IExporter>();

        /// <summary>
        /// Gets the exporter with the specified name.
        /// </summary>
        /// <param name="name">The name of the exporter to return.</param>
        /// <returns>An instance of <see cref="IExporter"/> or null if no exporter exists with the specified name.</returns>
        public IExporter this[string name]
        {
            get { return exportersByName[name]; }
        }

        /// <summary>
        /// Loads the <see cref="IExporter"/> instances from the dlls.
        /// </summary>
        public void LoadExporters()
        {
            // Clear the existing exporters.
            exporters.Clear();
            exportersByName.Clear();
            
            // Prepare the catalog from which to load the exporters.
            DirectoryCatalog catalog = new DirectoryCatalog(EXPORTERS_DIRECTORY_NAME, "*.dll");

            // Load the exporters.
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            // Index the exporters by name.
            foreach (IExporter exporter in exporters)
            {
                exportersByName.Add(exporter.Name, exporter);
            }
        }

        /// <summary>
        /// Returns an array of exporters managed by the current instance of the <see cref="ExportersManager"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the exporters.</returns>
        public IEnumerable<IExporter> GetExporters()
        {
            return exporters.ToArray();
        }
    }
}
