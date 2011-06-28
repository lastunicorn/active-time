using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Exporters
{
    internal class ExporterCreator
    {
        public static IExporter CreateExporter(ExportType exportType)
        {
            switch (exportType)
            {
                case ExportType.Normal:
                    return new SafeExporter();

                case ExportType.Full:
                    return new FullExporter();

                default:
                    throw new ArgumentException("Invalid export type value.", "exportType");
            }
        }
    }
}
