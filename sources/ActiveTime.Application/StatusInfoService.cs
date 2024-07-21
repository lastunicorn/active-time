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
using DustInTheWind.ActiveTime.Domain.ApplicationStatuses;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;

namespace DustInTheWind.ActiveTime.Application;

public class StatusInfoService : IDisposable
{
    public const string DefaultStatusText = "Ready";
    private const int DefaultStatusTimeout = 5000;

    private readonly EventBus eventBus;

    private bool isDisposed;
    private readonly Timer timer;
    private string statusText;

    public string StatusText
    {
        get => statusText;
        set
        {
            statusText = value;
            OnStatusTextChanged(EventArgs.Empty);
        }
    }

    protected virtual void OnStatusTextChanged(EventArgs e)
    {
        ApplicationStatusChangedEvent applicationStatusChangedEvent = new()
        {
            StatusText = StatusText
        };
        _ = eventBus.Publish(applicationStatusChangedEvent);
    }

    public StatusInfoService(EventBus eventBus)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        statusText = DefaultStatusText;
        timer = new Timer(HandleTimerElapsed);
    }

    private void HandleTimerElapsed(object o)
    {
        StatusText = DefaultStatusText;
    }

    public void SetStatus(string text, int timeout)
    {
        StatusText = text;

        if (timeout > 0)
            timer.Change(timeout, -1);
    }

    public void SetStatus(string text)
    {
        SetStatus(text, DefaultStatusTimeout);
    }

    public void SetStatus(StatusMessage statusMessage)
    {
        SetStatus(statusMessage.Text, DefaultStatusTimeout);
    }

    public void SetStatus<T>()
        where T : StatusMessage
    {
        T statusMessage = Activator.CreateInstance<T>();
        SetStatus(statusMessage.Text, DefaultStatusTimeout);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (isDisposed)
            return;

        if (disposing)
        {
            timer.Dispose();
        }

        isDisposed = true;
    }

    ~StatusInfoService()
    {
        Dispose(false);
    }
}