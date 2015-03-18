using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Collection with change notifications.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public interface INotifyCollection<T> :
        ICollection<T>,
        INotifyEnumerable<T>
    {
        void Add(IEnumerable<T> items);
        bool Remove(IEnumerable<T> items);
    }


    public interface INotifyCollection<TItem, TId> :
        INotifyCollection<TItem>,
        INotifyEnumerable<TItem, TId>
        where TItem : IId<TId>
    {
        bool Remove(TId id);
        bool Remove(IEnumerable<TId> ids);
    }
}
