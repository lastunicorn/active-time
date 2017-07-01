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

using System;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Services;

namespace DustInTheWind.ActiveTime.ViewModels
{
    public class DayRecordsViewModel : ViewModelBase
    {
        private readonly ICurrentDayRecord currentDayRecord;
        
        private DayTimeInterval[] records;
        public DayTimeInterval[] Records
        {
            get { return records; }
            private set
            {
                records = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayRecordsViewModel"/> class.
        /// </summary>
        public DayRecordsViewModel(ICurrentDayRecord currentDayRecord)
        {
            if (currentDayRecord == null) throw new ArgumentNullException(nameof(currentDayRecord));

            this.currentDayRecord = currentDayRecord;

            currentDayRecord.ValueChanged += HandleCurrentDayRecordChanged;
            currentDayRecord.Update();
        }

        private void HandleCurrentDayRecordChanged(object sender, EventArgs eventArgs)
        {
            UpdateDisplayedData();
        }

        private void UpdateDisplayedData()
        {
            Records = currentDayRecord.Value?.GetTimeRecords(false);
        }
    }
}
