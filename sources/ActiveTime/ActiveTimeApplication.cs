using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Goose;
using System.ComponentModel.Composition;
using DustInTheWind.ActiveTime.Exporters;
using System.ComponentModel.Composition.Hosting;

namespace DustInTheWind.ActiveTime
{
    public class ActiveTimeApplication
    {
        private Dal dal;
        public Dal Dal
        {
            get { return dal; }
        }
        
        private Recorder recorder;
        public Recorder Recorder
        {
            get { return recorder; }
        }

        private Reminder reminder;
        public Reminder Reminder
        {
            get { return reminder; }
        }

        private ExportersManager exporters;
        public ExportersManager Exporters
        {
            get { return exporters; }
        }

        public ActiveTimeApplication()
        {
            dal = new Dal();
            reminder = new Reminder();
            recorder = new Recorder(dal);
            exporters = new ExportersManager();
            exporters.LoadExporters();
        }
    }
}
