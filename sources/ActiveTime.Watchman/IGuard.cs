﻿using System;

namespace DustInTheWind.ActiveTime.Watchman
{
    public interface IGuard : IDisposable
    {
        /// <summary>
        /// Gets the name of the current instance.
        /// </summary>
        string Name { get; }
    }
}