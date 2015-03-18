using System;
using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Enumeration with change notifications.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public interface INotifyEnumerable<T> :
        IEnumerable<T>,
        INotifyCollectionChange<T>
    {
    }


    public interface INotifyEnumerable<TItem, TId> :
        INotifyEnumerable<TItem>,
        IItemLookup<TItem, TId>
        where TItem : IId<TId>
    { }
}
