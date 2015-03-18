using CeMaS.Common;
using CeMaS.Common.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CeMaS.Data.Storages
{
    /// <summary>
    /// Items of a storage.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public interface IStorageItems<TItem, TId> :
        IId<string>,
        IStorageContent,
        IItemLookupAsync<TItem, TId>
        where TItem : IId<TId>
    {
        #region Items

        /// <summary>
        /// Available items.
        /// </summary>
        IQueryable<TItem> Items { get; }

        #endregion

        #region Changes

        /// <summary>
        /// Notifies about added new items.
        /// </summary>
        IObservable<TItem> ItemsAdded { get; }
        /// <summary>
        /// Notifies about updated items.
        /// </summary>
        IObservable<TItem> ItemsUpdated { get; }
        /// <summary>
        /// Notifies about removed items.
        /// </summary>
        IObservable<TItem> ItemsRemoved { get; }

        /// <summary>Saves <paramref name="item"/> by adding (if new) or updating (if already existing) it. 
        /// </summary>
        /// <param name="item">Item.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        Task Save(TItem item);
        /// <summary>
        /// Remove an item with <paramref name="itemId"/>.
        /// </summary>
        /// <param name="itemId">Item identifier.</param>
        /// <returns>Whether an item was found and removed.</returns>
        Task<bool> Remove(TId itemId);

        #endregion
    }
}