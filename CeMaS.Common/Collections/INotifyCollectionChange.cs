using System;
using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Notifies about a collection changes.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public interface INotifyCollectionChange<T>
    {
        /// <summary>
        /// Notifies about added new items.
        /// </summary>
        IObservable<IEnumerable<T>> Added { get; }
        /// <summary>
        /// Notifies about removed items.
        /// </summary>
        IObservable<IEnumerable<T>> Removed { get; }
    }
}
