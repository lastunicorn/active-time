using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;

namespace DustInTheWind.ActiveTime.Exporters
{
    public class ExportersManager
    {
        private const string EXPORTERS_DIRECTORY_NAME = "Exporters";

        [ImportMany]
        private List<IExporter> exporters = new List<IExporter>();

        private Dictionary<string, IExporter> exportersByName = new Dictionary<string, IExporter>();

        public IExporter this[string name]
        {
            get { return exportersByName[name]; }
        }


        public void LoadExporters()
        {
            //string appDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //string[] exporterPluginFileNames = Directory.GetFiles(appDirectory);
            //foreach (string FileName in exporterPluginFileNames)
            //{

            //}

            //Assembly assembly = Assembly.LoadFrom(Path.Combine(EXPORTERS_DIRECTORY_NAME, "MinimalExporter.dll"));
            //AssemblyCatalog catalog = new AssemblyCatalog(assembly);

            DirectoryCatalog catalog = new DirectoryCatalog(EXPORTERS_DIRECTORY_NAME, "*.dll");

            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);


            foreach (IExporter exporter in exporters)
            {
                exportersByName.Add(exporter.Name, exporter);
            }
        }

        public IEnumerable<IExporter> GetExporters()
        {
            return exporters.ToArray();
        }
    }
}
