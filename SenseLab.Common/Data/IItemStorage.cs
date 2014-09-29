using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Storage of items.
    /// Local or remote.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public interface IItemStorage<TItem, TId> :
        IId<Guid>
        where TItem : IId<TId>
    {
        string Name { get; }
        string Description { get; }

        bool IsReadOnly { get; }
        bool IsConnected { get; }

        IQbservable<TItem> Items { get; }
        IObservable<TItem> ItemsUpdated { get; }
        IObservable<TItem> ItemsRemoved { get; }

        Task Connect();
        Task Disconnect();

        Task Add(TItem item);
        Task<bool> Update(TItem item);
        Task<bool> Remove(TId itemId);
    }
}
