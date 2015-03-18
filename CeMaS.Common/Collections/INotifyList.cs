using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// List with change notifications.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public interface INotifyList<T> :
        IList<T>,
        INotifyCollection<T>
    {
        void Insert(int index, IEnumerable<T> items);
    }


    public interface INotifyList<TItem, TId> :
        INotifyList<TItem>,
        INotifyCollection<TItem, TId>
        where TItem : IId<TId>
    { }
}
