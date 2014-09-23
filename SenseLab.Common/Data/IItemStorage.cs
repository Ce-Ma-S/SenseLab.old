using System.Linq;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Storage of items.
    /// Local or remote.
    /// </summary>
    public interface IItemStorage<T>
    {
        bool IsReadOnly { get; }
        bool IsConnected { get; }

        IQueryable<T> Items { get; }

        void Connect();
        void Disconnect();

        void AddOrReplace(T item);
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
        bool Remove(TId itemId);
    }
}
