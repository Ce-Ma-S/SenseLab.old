using SenseLab.Common.Events;
using System;

namespace SenseLab.Common.Locations
{
    /// <summary>
    /// Allows specification of a changeable location of a locatable object.
    /// </summary>
    /// <typeparam name="T">Location type.</typeparam>
    public interface ILocatableChangeable<T> :
        ILocatable<T>
        where T : ILocation
    {
        /// <summary>
        /// Fired when <see cref="ILocatable{T}.Location"/> is changed.
        /// </summary>
        event EventHandler<ValueChangeEventArgs<T>> LocationChanged;
    }
}
