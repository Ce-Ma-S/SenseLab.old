using System;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Storage of items.
    /// Local or remote.
    /// </summary>
    public interface IItemStorage<T> :
        IId<Guid>
    {
        string Name { get; }
        string Description { get; }

        bool IsReadOnly { get; }
        bool IsConnected { get; }

        IObservable<T> Items { get; }
        IObservable<T> ItemsUpdated { get; }
        IObservable<T> ItemsRemoved { get; }

        void Connect();
        void Disconnect();

        void Add(T item);
        bool Update(T item);
        bool Remove(T item);
    }


    /// <summary>
    /// Storage of items.
    /// Local or remote.
    /// </summary>
    public interface IItemStorage<TItem, TId> :
        IItemStorage<TItem>
        where TItem : IId<TId>
    {
        OptionalValue<TItem> this[TId itemId] { get; }

        bool Contains(TId itemId);
        bool Remove(TId itemId);
    }
}
