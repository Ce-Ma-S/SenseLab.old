using System;

namespace SenseLab.Common.Locations
{
    /// <summary>
    /// Location.
    /// </summary>
    /// <remarks><see cref="Events.IChangeable"/> can be implemented as well.</remarks>
    public interface ILocation
    {
        /// <summary>
        /// Text representation.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Clones this location.
        /// </summary>
        /// <returns>Independent clone.</returns>
        ILocation Clone();
    }
}
