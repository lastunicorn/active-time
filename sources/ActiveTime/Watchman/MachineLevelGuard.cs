using System;
using System.Threading;

namespace DustInTheWind.ActiveTime.Watchman
{
    public class MachineLevelGuard : IGuard
    {
        /// <summary>
        /// Gets the name of the current instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The <see cref="Mutex"/> object used to ensure that only one instance
        /// of the class is created on the current machine. (Machine level)
        /// </summary>
        private Mutex mutex;

        public MachineLevelGuard(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

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

        private bool disposed = false;

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