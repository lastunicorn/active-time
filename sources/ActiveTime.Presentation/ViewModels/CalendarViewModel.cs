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

using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.CurrentDate.ChangeDate;
using DustInTheWind.ActiveTime.Application.CurrentDate.DecrementDate;
using DustInTheWind.ActiveTime.Application.CurrentDate.PresentCalendar;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels;

public class CalendarViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;

    private DateTime date;

    public DateTime Date
    {
        get => date;
        set
        {
            if (date == value)
                return;

            date = value;
            OnPropertyChanged();

            if (!IsInitializing)
                UpdateDate(value).Wait();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarViewModel"/> class.
    /// </summary>
    public CalendarViewModel(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        _ = Initialize();

        eventBus.Subscribe<CurrentDateChangedEvent>(HandleCurrentDateChanged);
    }

    private Task HandleCurrentDateChanged(CurrentDateChangedEvent ev, CancellationToken cancellationToken)
    {
        RunAsInitialization(() => Date = ev.Date);
        return Task.CompletedTask;
    }

    private async Task Initialize()
    {
        PresentCalendarRequest request = new();
        PresentCalendarResponse response = await requestBus.Send<PresentCalendarRequest, PresentCalendarResponse>(request);

        RunAsInitialization(() => Date = response.Date);
    }

    private async Task UpdateDate(DateTime? value)
    {
        ChangeDateRequest request = new()
        {
            Date = value
        };
        await requestBus.Send(request);
    }
}