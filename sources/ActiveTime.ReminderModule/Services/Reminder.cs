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
using System.Threading;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Reminding;

namespace DustInTheWind.ActiveTime.Reminding.Services
{
    /// <summary>
    /// It is a timer that "rings" after a specified time.
    /// </summary>
    public class Reminder : IDisposable, IReminder
    {
        /// <summary>
        /// Lock object used when the status needs to be changed.
        /// </summary>
        private object lockStatus = new object();

        #region StartTime

        /// <summary>
        /// The time when the remainder has been started last time.
        /// </summary>
        private DateTime startTime;

        /// <summary>
        /// Gets the time when the remainder has been started last time.
        /// If the current instance has never been started, it is equal with <see cref="DateTime.MinValue"/> value.
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
        }

        #endregion

        #region Status

        /// <summary>
        /// The status of the current instance of the <see cref="Reminder"/>.
        /// </summary>
        private ReminderStatus status = ReminderStatus.NotStarted;

        /// <summary>
        /// Gets the status of the current instance of the <see cref="Reminder"/>.
        /// </summary>
        public ReminderStatus Status
        {
            get { return status; }
        }

        #endregion


        #region Event Ring

        /// <summary>
        /// Event raised when the timer elapsed.
        /// </summary>
        public event EventHandler<RingEventArgs> Ring;

        /// <summary>
        /// Raises the Ring event.
        /// </summary>
        /// <param name="e">An <see cref="RingEventArgs"/> that contains the event data.</param>
        protected virtual void OnRing(RingEventArgs e)
        {
            if (Ring != null)
            {
                Ring(this, e);
            }
        }

        #endregion

        #region Event Stopped

        /// <summary>
        /// Event raised when the current instance is stopped. After the Ring event or after
        /// a call of the <see cref="M:Stop"/> or <see cref="M:Reset"/> methods.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Raises the Stopped event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStopped(EventArgs e)
        {
            if (Stopped != null)
            {
                Stopped(this, e);
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instace of the <see cref="Reminder"/> class.
        /// </summary>
        public Reminder()
        {
            flagFinished = new ManualResetEvent(false);
            timer = new Timer(new TimerCallback(TimerElapsed), null, Timeout.Infinite, Timeout.Infinite);
        }

        #endregion


        #region Start

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds to delay before the clock will ring.</param>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        public void Start(int milliseconds)
        {
            if (disposed)
                throw new ObjectDisposedException("Reminder");

            if (milliseconds >= 0)
            {
                lock (lockStatus)
                {
                    flagFinished.Reset();
                    startTime = DateTime.Now;
                    timer.Change(milliseconds, Timeout.Infinite);
                    status = ReminderStatus.Running;
                }
            }
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds to delay before the clock will ring.</param>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        public void Start(long milliseconds)
        {
            if (disposed)
                throw new ObjectDisposedException("Reminder");

            if (milliseconds >= 0)
            {
                lock (lockStatus)
                {
                    flagFinished.Reset();
                    startTime = DateTime.Now;
                    timer.Change(milliseconds, Timeout.Infinite);
                    status = ReminderStatus.Running;
                }
            }
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds to delay before the clock will ring.</param>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        public void Start(uint milliseconds)
        {
            if (disposed)
                throw new ObjectDisposedException("Reminder");

            if (milliseconds >= 0)
            {
                lock (lockStatus)
                {
                    flagFinished.Reset();
                    startTime = DateTime.Now;
                    timer.Change(milliseconds, Timeout.Infinite);
                    status = ReminderStatus.Running;
                }
            }
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="Time">A <see cref="Timespan"/> representing the amount of time to delay before the clock will ring.</param>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        public void Start(TimeSpan time)
        {
            if (disposed)
                throw new ObjectDisposedException("Reminder");

            if (time != TimeSpan.Zero)
            {
                lock (lockStatus)
                {
                    flagFinished.Reset();
                    startTime = DateTime.Now;
                    timer.Change(time, TimeSpan.FromMilliseconds(-1));
                    status = ReminderStatus.Running;
                }
            }
        }

        #endregion

        #region Reset

        /// <summary>
        /// Resets the status to "NotStarted" Value. If the timer is running, it is automatically stopped without triggering the Ring event.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        public void Reset()
        {
            if (disposed)
                throw new ObjectDisposedException("Reminder");

            lock (lockStatus)
            {
                if (status == ReminderStatus.Running || status == ReminderStatus.Snooze)
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                }

                status = ReminderStatus.NotStarted;
                OnStopped(EventArgs.Empty);
                flagFinished.Set();
            }
        }

        #endregion

        #region Stop

        /// <summary>
        /// Alias method for Reset(). Stops the timer without triggering the Ring event.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        public void Stop()
        {
            Reset();
        }

        #endregion

        #region Snooze

        /// <summary>
        /// The time used to postpone the ring.
        /// </summary>
        private TimeSpan snoozeTime = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Gets or sets the time used to postpone the ring.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        public TimeSpan SnoozeTime
        {
            get { return snoozeTime; }
            set
            {
                if (disposed)
                    throw new ObjectDisposedException("Reminder");

                snoozeTime = value;
            }
        }

        /// <summary>
        /// Sets the timer to ring after the snoose time.
        /// </summary>
        private void Snooze(TimeSpan? snoozeTime)
        {
            lock (lockStatus)
            {
                if (status == ReminderStatus.Stopping)
                {
                    flagFinished.Reset();

                    if (snoozeTime != null)
                    {
                        if (snoozeTime.Value >= TimeSpan.Zero)
                            timer.Change(snoozeTime.Value, TimeSpan.FromTicks(-1));
                    }
                    else if (this.snoozeTime >= TimeSpan.Zero)
                    {
                        timer.Change(this.snoozeTime, TimeSpan.FromTicks(-1));
                    }
                }

                status = ReminderStatus.Snooze;
            }
        }

        #endregion

        #region Timer

        /// <summary>
        /// The internal timer.
        /// </summary>
        private Timer timer = null;

        /// <summary>
        /// Call back function triggered when the timer elapsed.
        /// </summary>
        /// <param name="o"></param>
        private void TimerElapsed(object o)
        {
            lock (lockStatus)
            {
                if (status == ReminderStatus.Running || status == ReminderStatus.Snooze)
                {
                    status = ReminderStatus.Stopping;

                    RingEventArgs e = new RingEventArgs();
                    OnRing(e);

                    if (e.Snooze)
                    {
                        Snooze(e.SnoozeTime);
                    }
                    else
                    {
                        status = ReminderStatus.Finished;
                        OnStopped(EventArgs.Empty);

                        flagFinished.Set();
                    }
                }
            }
        }

        #endregion

        #region WaitUntilRing

        /// <summary>
        /// A semaphore that turns green (true) when the clock rings or is stopped.
        /// That means it is red (false) only if the clock is running.
        /// </summary>
        private ManualResetEvent flagFinished = null;

        /// <summary>
        /// Blocks the current thread until the clock rangs or it is stopped.
        /// </summary>
        /// <returns>true if the clock has rang; false if it has been stopped by other causes.</returns>
        /// <remarks>Before calling this method you should check the status of the Reminder to see if it is running.
        /// If the Reminder has rang a lot of time ago, this method returns immediately, but it still returns true.</remarks>
        /// <exception cref="ObjectDisposedException">The current instance was disposed.</exception>
        public bool WaitUntilRing()
        {
            if (disposed)
                throw new ObjectDisposedException("Reminder");

            flagFinished.WaitOne();

            return status == ReminderStatus.Finished;
        }

        #endregion

        #region IDisposable Members

        private bool disposed = false;

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        /// <remarks>
        /// <para>This method is executed in two distinct scenarios.</para>
        /// <para>If the method has been called directly or indirectly by a user's code managed and unmanaged resources can be disposed.</para>
        /// <para>If the method has been called by the runtime from inside the finalizer you should not reference other objects. Only unmanaged resources can be disposed.</para>
        /// </remarks>
        /// <param name="disposing">Specifies if the method has been called by a user's code (true) or by the runtime from inside the finalizer (false).</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed resources.
                if (disposing)
                {
                    // Stop the current instance.

                    try { Stop(); }
                    catch { }

                    // Dispose managed resources.

                    if (timer != null)
                    {
                        timer.Dispose();
                        timer = null;
                    }

                    if (flagFinished != null)
                    {
                        flagFinished.Close();
                        flagFinished = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources here.
                // ...

                disposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Reminder()
        {
            Dispose(false);
        }

        #endregion
    }
}

