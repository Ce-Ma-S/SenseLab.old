using System;

namespace CeMaS.Common
{
    /// <summary>
    /// Allows tracking of availability of a resource this object represents.
    /// </summary>
    public interface IAvailable
    {
        /// <summary>
        /// Whether a resource this object represents is available.
        /// </summary>
        bool IsAvailable { get; }
        /// <summary>
        /// Fired when <see cref="IsAvailable"/> changes.
        /// </summary>
        event EventHandler IsAvailableChanged;
    }
}
