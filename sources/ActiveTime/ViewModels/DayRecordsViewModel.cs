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

using System;
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;

namespace DustInTheWind.ActiveTime.ViewModels
{
    public class DayRecordsViewModel : ViewModelBase
    {
        private readonly ITimeRecordRepository timeRecordRepository;
        private readonly IStateService stateService;

        private DayRecord dayRecord;

        public DayTimeInterval[] Records
        {
            get { return dayRecord == null ? null : dayRecord.GetTimeRecords(false); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontViewModel"/> class.
        /// </summary>
        public DayRecordsViewModel(ITimeRecordRepository timeRecordRepository, IStateService stateService)
        {
            if (timeRecordRepository == null)
                throw new ArgumentNullException("timeRecordRepository");

            if (stateService == null)
                throw new ArgumentNullException("stateService");

            this.timeRecordRepository = timeRecordRepository;
            this.stateService = stateService;
        }

        public void Initialize()
        {
            UpdateDisplayedData();
        }

        private void UpdateDisplayedData()
        {
            DateTime? currentDate = stateService.CurrentDate;

            if (currentDate != null)
            {
                IList<TimeRecord> timeRecords = timeRecordRepository.GetByDate(currentDate.Value);
                DayRecord newDayRecord = DayRecord.FromTimeRecords(timeRecords);
                dayRecord = newDayRecord ?? new DayRecord(currentDate.Value);
            }
            else
            {
                dayRecord = null;
            }

            NotifyPropertyChanged("Records");
        }
    }
}
