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

using DustInTheWind.ActiveTime.Exporters;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Recording;

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
