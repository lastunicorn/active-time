// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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

using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using DustInTheWind.ActiveTime.Ports.SystemAccess;

namespace DustInTheWind.ActiveTime.Application.Recording2;

/// <summary>
/// Keeps track of a current record and updates it in the database when requested.
/// </summary>
/// <remarks>
/// The current record can be obtained in two ways: 1) from the database or 2) by creating a new one.
/// </remarks>
public class Scribe
{
    private readonly ISystemClock systemClock;
    private readonly IUnitOfWork unitOfWork;
    private readonly CurrentDay currentDay;

    public Scribe(ISystemClock systemClock, IUnitOfWork unitOfWork, CurrentDay currentDay)
    {
        this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
    }

    /// <summary>
    /// Creates a new time record and saves it in the repository.
    /// </summary>
    public void StampNew()
    {
        DateTime now = systemClock.GetCurrentTime();
        CreateNewTimeRecord(now);
    }

    private void CreateNewTimeRecord(DateTime now)
    {
        TimeRecord newTimeRecord = new(now);
        unitOfWork.TimeRecordRepository.Add(newTimeRecord);
        currentDay.TimeRecordId = newTimeRecord.Id;
    }

    /// <summary>
    /// Updates the current time record with the current time.
    /// If there is no record a new one is automatically created.
    /// </summary>
    public void Stamp()
    {
        TimeRecord currentTimeRecord = RetrieveCurrentTimeRecord();

        DateTime now = systemClock.GetCurrentTime();

        if (currentTimeRecord == null)
        {
            CreateNewTimeRecord(now);
        }
        else
        {
            bool isSameDay = currentTimeRecord.Date == now.Date;

            if (isSameDay)
            {
                currentTimeRecord.EndTime = now.TimeOfDay;
                unitOfWork.TimeRecordRepository.Update(currentTimeRecord);
            }
            else
            {
                currentTimeRecord.EndAtMidnight();
                unitOfWork.TimeRecordRepository.Update(currentTimeRecord);

                TimeRecord newTimeRecord = TimeRecord.NewFromMidnight(now);
                unitOfWork.TimeRecordRepository.Add(newTimeRecord);
                currentDay.TimeRecordId = newTimeRecord.Id;
            }
        }
    }

    /// <summary>
    /// Deletes from the database the current time record.
    /// If no time record was created, nothing happens.
    /// </summary>
    public void DeleteCurrentTimeRecord()
    {
        TimeRecord currentTimeRecord = RetrieveCurrentTimeRecord();

        if (currentTimeRecord == null)
            return;

        unitOfWork.TimeRecordRepository.Delete(currentTimeRecord);

        currentDay.TimeRecordId = null;
    }

    private TimeRecord RetrieveCurrentTimeRecord()
    {
        if (currentDay.TimeRecordId == null)
            return null;

        int currentTimeRecordId = currentDay.TimeRecordId.Value;
        return unitOfWork.TimeRecordRepository.GetById(currentTimeRecordId);
    }
}