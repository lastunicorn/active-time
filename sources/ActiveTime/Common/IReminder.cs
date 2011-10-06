using System;
using DustInTheWind.ActiveTime.Common.Reminding;
using DustInTheWind.ActiveTime.Main.Services;
using DustInTheWind.ActiveTime.Reminding.Services;

namespace DustInTheWind.ActiveTime.Common
{
    public interface IReminder
    {
        /// <summary>
        /// Gets the time when the remainder has been started last time.
        /// If the current instance has never been started, it is equal with <see cref="DateTime.MinValue"/> value.
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Gets the status of the current instance of the <see cref="Reminder"/>.
        /// </summary>
        ReminderStatus Status { get; }

        /// <summary>
        /// Gets or sets the time used to postpone the ring.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        TimeSpan SnoozeTime { get; set; }

        /// <summary>
        /// Event raised when the timer elapsed.
        /// </summary>
        event EventHandler<RingEventArgs> Ring;

        /// <summary>
        /// Event raised when the current instance is stopped. After the Ring event or after
        /// a call of the <see cref="M:Stop"/> or <see cref="M:Reset"/> methods.
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
        /// <param name="Time">A <see cref="Timespan"/> representing the amount of time to delay before the clock will ring.</param>
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