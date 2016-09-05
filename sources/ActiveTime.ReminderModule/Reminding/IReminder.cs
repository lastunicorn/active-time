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

namespace DustInTheWind.ActiveTime.ReminderModule.Reminding
{
    public interface IReminder
    {
        /// <summary>
        /// Gets the time when the remainder has been started last time.
        /// If the current instance has never been started, it is equal with <see cref="DateTime.MinValue"/> value.
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Gets the status of the current instance of the <see cref="IReminder"/>.
        /// </summary>
        ReminderStatus Status { get; }

        /// <summary>
        /// Gets or sets the time used to postpone the ring.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        TimeSpan DefaultSnoozeTime { get; set; }

        /// <summary>
        /// Event raised when the timer elapsed.
        /// </summary>
        event EventHandler<RingEventArgs> Ring;

        /// <summary>
        /// Event raised when the current instance is stopped. After the Ring event or after
        /// a call of the <see cref="Stop"/> or <see cref="Reset"/> methods.
        /// </summary>
        event EventHandler Stopped;

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds to delay before the clock will ring.</param>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        void Start(int milliseconds);

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds to delay before the clock will ring.</param>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        void Start(long milliseconds);

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds to delay before the clock will ring.</param>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        void Start(uint milliseconds);

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="time">A <see cref="TimeSpan"/> representing the amount of time to delay before the clock will ring.</param>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        void Start(TimeSpan time);

        /// <summary>
        /// Resets the status to "NotStarted" Value. If the timer is running, it is automatically stopped without triggering the Ring event.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        void Reset();

        /// <summary>
        /// Alias method for Reset(). Stops the timer without triggering the Ring event.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        void Stop();

        /// <summary>
        /// Blocks the current thread until the clock rangs or it is stopped.
        /// </summary>
        /// <returns>true if the clock has rang; false if it has been stopped by other causes.</returns>
        /// <remarks>Before calling this method you should check the status of the Reminder to see if it is running.
        /// If the Reminder has rang a lot of time ago, this method returns immediately, but it still returns true.</remarks>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        bool WaitUntilRing();

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        void Dispose();
    }
}