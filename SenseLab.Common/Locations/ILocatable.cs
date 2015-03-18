using CeMaS.Common.Events;
using System;

namespace SenseLab.Common.Locations
{
    /// <summary>
    /// Allows specification of a location of an object.
    /// </summary>
    /// <typeparam name="T">Location type.</typeparam>
    public interface ILocatable<T>
        where T : ILocation
    {
        /// <summary>
        /// Location.
        /// </summary>
        /// <exception cref="ArgumentNullException">set: <see cref="LocationIsRequired"/> is true and value is null.</exception>
        T Location { get; set; }
        bool LocationIsRequired { get; }
        /// <summary>
        /// Fired when <see cref="Location"/> is changed.
        /// </summary>
        event EventHandler<ValueChangeEventArgs<T>> LocationChanged;
    }
}
