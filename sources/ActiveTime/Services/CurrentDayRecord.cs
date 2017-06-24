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

namespace DustInTheWind.ActiveTime.Services
{
    public class CurrentDayRecord : ICurrentDayRecord
    {
        private readonly IStatusInfoService statusInfoService;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IStateService stateService;

        private DayRecord value;
        public DayRecord Value
        {
            get { return value; }
            private set
            {
                this.value = value;
                OnValueChanged(EventArgs.Empty);
            }
        }

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        public CurrentDayRecord(IRecorderService recorder, IStatusInfoService statusInfoService, IUnitOfWorkFactory unitOfWorkFactory, IStateService stateService)
        {
            if (recorder == null) throw new ArgumentNullException(nameof(recorder));
            if (statusInfoService == null) throw new ArgumentNullException(nameof(statusInfoService));
            if (unitOfWorkFactory == null) throw new ArgumentNullException(nameof(unitOfWorkFactory));
            if (stateService == null) throw new ArgumentNullException(nameof(stateService));

            this.statusInfoService = statusInfoService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.stateService = stateService;

            recorder.Started += HandleRecorderStarted;
            recorder.Stopped += HandleRecorderStopped;
            recorder.Stamping += HandleRecorderStamping;
            recorder.Stamped += HandleRecorderStamped;

            stateService.CurrentDateChanged += HandleCurrentDateChanged;
        }

        public void Update()
        {
            UpdateValueFromRepository();
        }

        private void HandleCurrentDateChanged(object sender, EventArgs e)
        {
            UpdateValueFromRepository();
        }

        private void HandleRecorderStarted(object sender, EventArgs e)
        {
            UpdateValueFromRepository();
            statusInfoService.SetStatus("Recorder started.");
        }

        private void HandleRecorderStopped(object sender, EventArgs e)
        {
            UpdateValueFromRepository();
            statusInfoService.SetStatus("Recorder stopped.");
        }

        private void HandleRecorderStamping(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Updating the current record's time.");
        }

        private void HandleRecorderStamped(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Current record's time has been updated.");
            UpdateValueFromRepository();
        }

        private void UpdateValueFromRepository()
        {
            DateTime? currentDate = stateService.CurrentDate;

            if (currentDate != null)
            {
                using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
                {
                    ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;

                    IList<TimeRecord> timeRecords = timeRecordRepository.GetByDate(currentDate.Value);
                    DayRecord dayRecord = DayRecord.FromTimeRecords(timeRecords);
                    Value = dayRecord ?? new DayRecord(currentDate.Value);
                }
            }
            else
            {
                Value = null;
            }
        }
    }
}
