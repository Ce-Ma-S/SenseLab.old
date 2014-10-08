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
        IId<Guid>,
        IStreamManager
        where TItem : IId<TId>
    {
        string Name { get; }
        string Description { get; }

        #region Connection

        bool IsConnected { get; }

        Task Connect();
        Task Disconnect();

        #endregion

        #region Items

        IQbservable<TItem> Items { get; }

        Task<bool> Contains(TId itemId);
        Task<TItem> FirstOrDefault(TId itemId);

        #endregion

        #region Updates

        bool IsReadOnly { get; }

        IObservable<TItem> ItemsUpdated { get; }
        IObservable<TItem> ItemsRemoved { get; }
        
        Task Add(TItem item);
        Task Update(TItem item);
        Task<bool> Remove(TId itemId);

        #endregion
    }
}
