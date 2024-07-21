// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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

namespace DustInTheWind.ActiveTime.Infrastructure.Watchman
{
    /// <summary>
    /// This class ensures that an instance of itself with the same name
    /// can not be created twice.
    /// </summary>
    public class Guard : IGuard
    {
        /// <summary>
        /// Gets the name of the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value that specifies the level at which the current instance of the
        /// <see cref="Guard"/> class will have effect.
        /// </summary>
        public GuardLevel GuardLevel { get; }

        private readonly IGuard guard;

        /// <summary>
        /// Initializes a new instance of the <see cref="Guard"/> class with
        /// the name that identifies it and 
        /// the level at which will have effect.
        /// </summary>
        /// <param name="name">The name that identifies the instance that will be created.</param>
        /// <param name="guardLevel">The level at which the new instance will have efect.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ActiveTimeException"></exception>
        public Guard(string name, GuardLevel guardLevel)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            GuardLevel = guardLevel;

            switch (guardLevel)
            {
                case GuardLevel.None:
                    guard = new IncompetentGuard();
                    break;

                case GuardLevel.Application:
                    guard = new ApplicationLevelGuard(name);
                    break;

                case GuardLevel.Machine:
                    guard = new MachineLevelGuard(name);
                    break;

                default:
                    throw new ArgumentException("Invalid guard level.", nameof(guardLevel));
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

                guard?.Dispose();
            }

            // Call the appropriate methods to clean up unmanaged resources here.
            // ...

            disposed = true;
        }

        ~Guard()
        {
            Dispose(false);
        }

        #endregion
    }
}