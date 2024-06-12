﻿// ActiveTime
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
using DustInTheWind.ActiveTime.Common.ApplicationStatuses;

namespace DustInTheWind.ActiveTime.Application;

/// <summary>
/// A service that stores different status messages.
/// </summary>
public class StatusInfoService : IDisposable
{
    /// <summary>
    /// The default Text of the status.
    /// </summary>
    public const string DefaultStatusText = "Ready";

    private const int DefaultStatusTimeout = 5000;

    private bool isDisposed;
    private readonly Timer timer;
    private string statusText;

    /// <summary>
    /// Gets or sets the text representing the status.
    /// </summary>
    public string StatusText
    {
        get => statusText;
        set
        {
            statusText = value;
            OnStatusTextChanged(EventArgs.Empty);
        }
    }

    /// <summary>
    /// Event raised when the status text is changed.
    /// </summary>
    public event EventHandler StatusTextChanged;

    /// <summary>
    /// Raises the <see cref="StatusTextChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
    protected virtual void OnStatusTextChanged(EventArgs e)
    {
        StatusTextChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusInfoService"/> class.
    /// </summary>
    public StatusInfoService()
    {
        statusText = DefaultStatusText;
        timer = new Timer(HandleTimerElapsed);
    }

    /// <summary>
    /// The call-back method of the timer that resets the status to the default Text.
    /// </summary>
    /// <param name="o">Unused</param>
    private void HandleTimerElapsed(object o)
    {
        StatusText = DefaultStatusText;
    }

    /// <summary>
    /// Sets the status of the model to the specified Text and
    /// starts the timer that will reset it back to the default one.
    /// </summary>
    /// <param name="text">The Text to be set as status.</param>
    /// <param name="timeout">The Time in milliseconds after which the status will be reset to the default Text. If this Value is 0, the status will never be reset.</param>
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

    public void SetStatus(ApplicationStatus applicationStatus)
    {
        SetStatus(applicationStatus.Text, DefaultStatusTimeout);
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