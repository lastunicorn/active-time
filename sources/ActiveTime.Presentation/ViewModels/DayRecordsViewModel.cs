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
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Presentation.Services;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class DayRecordsViewModel : ViewModelBase
    {
        private readonly CurrentDay currentDayRecord;

        private DayTimeInterval[] records;

        public DayTimeInterval[] Records
        {
            get => records;
            private set
            {
                records = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayRecordsViewModel"/> class.
        /// </summary>
        public DayRecordsViewModel(CurrentDay currentDayRecord)
        {
            this.currentDayRecord = currentDayRecord ?? throw new ArgumentNullException(nameof(currentDayRecord));

            Records = currentDayRecord.Records;

            currentDayRecord.DatesChanged += HandleCurrentDayDatesChanged;
            currentDayRecord.ReloadDayRecord();
        }

        private void HandleCurrentDayDatesChanged(object sender, EventArgs eventArgs)
        {
            Records = currentDayRecord.Records;
        }
    }
}
