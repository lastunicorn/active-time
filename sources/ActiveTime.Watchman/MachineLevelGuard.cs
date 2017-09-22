// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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

namespace DustInTheWind.ActiveTime.Watchman
{
    public class MachineLevelGuard : IGuard
    {
        /// <summary>
        /// Gets the name of the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="Mutex"/> object used to ensure that only one instance
        /// of the class is created on the current machine. (Machine level)
        /// </summary>
        private Mutex mutex;

        public MachineLevelGuard(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            Name = name;

            CreateGuard();
        }

        private void CreateGuard()
        {
            // Create the mutex.
            mutex = new Mutex(false, Name);

            // Gain exclusive access to the mutex.
            bool access = mutex.WaitOne(0, true);

            if (!access)
            {
                string errorMessage = string.Format("Another instance with the name '{0}' already exists.", Name);
                throw new ActiveTimeException(errorMessage);
            }
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        /// <remarks>
        /// <para>Dispose(bool disposing) executes in two distinct scenarios.</para>
        /// <para>If the method has been called directly or indirectly by a user's code managed and unmanaged resources can be disposed.</para>
        /// <para>If the method has been called by the runtime from inside the finalizer you should not reference other objects. Only unmanaged resources can be disposed.</para>
        /// </remarks>
        /// <param name="disposing">Specifies if the method has been called by a user's code (true) or by the runtime from inside the finalizer (false).</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (disposed)
                return;

            // If disposing equals true, dispose all managed resources.
            if (disposing)
            {
                // Dispose managed resources.
                // ...

                if (mutex != null)
                    mutex.Close();
            }

            // Call the appropriate methods to clean up unmanaged resources here.
            // ...

            disposed = true;
        }

        ~MachineLevelGuard()
        {
            Dispose(false);
        }

        #endregion
    }
}