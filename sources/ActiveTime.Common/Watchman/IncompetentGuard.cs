using System;

namespace DustInTheWind.ActiveTime.Common.Watchman
{
    public class IncompetentGuard : IGuard
    {
        /// <summary>
        /// Gets the name of the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompetentGuard"/> class.
        /// </summary>
        public IncompetentGuard()
        {
            Name = "Incompetent Guard";
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

            }

            // Call the appropriate methods to clean up unmanaged resources here.
            // ...

            disposed = true;
        }

        ~IncompetentGuard()
        {
            Dispose(false);
        }

        #endregion
    }
}