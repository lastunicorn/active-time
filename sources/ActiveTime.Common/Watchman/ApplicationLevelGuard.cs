using System;
using System.Collections;

namespace DustInTheWind.ActiveTime.Common.Watchman
{
    public class ApplicationLevelGuard : IGuard
    {
        /// <summary>
        /// Contains the names of the <see cref="Guard"/> instances running at the application level
        /// in the current application.
        /// </summary>
        private static readonly Hashtable LocalInstanceNames = new Hashtable();

        /// <summary>
        /// Gets the name of the current instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Guard"/> class with
        /// the name that identifies it.
        /// </summary>
        /// <param name="name">The name that identifies the instance that will be created.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ActiveTimeException"></exception>
        public ApplicationLevelGuard(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;

            CreateGuard();
        }

        private void CreateGuard()
        {
            lock (LocalInstanceNames)
            {
                if (LocalInstanceNames.ContainsKey(Name))
                {
                    string errorMessage = string.Format("Another instance with the name '{0}' already exists.", Name);
                    throw new ActiveTimeException(errorMessage);
                }

                LocalInstanceNames.Add(Name, Name);
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

                lock (LocalInstanceNames)
                {
                    LocalInstanceNames.Remove(Name);
                }
            }

            // Call the appropriate methods to clean up unmanaged resources here.
            // ...

            disposed = true;
        }

        ~ApplicationLevelGuard()
        {
            Dispose(false);
        }

        #endregion
    }
}