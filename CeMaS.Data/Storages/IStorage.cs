using CeMaS.Common;
using System;

namespace CeMaS.Data.Storages
{
    /// <summary>
    /// Storage of a content.
    /// Local or remote.
    /// </summary>
    public interface IStorage :
        IId<Guid>,
        IConnectable,
        IDisposable
    {
        /// <summary>
        /// Whether this storage is for reading only.
        /// </summary>
        bool IsReadOnly { get; }

        string Name { get; }
        string Description { get; }

        /// <summary>
        /// Items of this storage.
        /// </summary>
        /// <typeparam name="TItem">Item type.</typeparam>
        /// <typeparam name="TId">Item identifier type.</typeparam>
        /// <returns>Items if supported by this storage, otherwise null.</returns>
        IStorageItems<TItem, TId> Items<TItem, TId>(string id = null)
            where TItem : IId<TId>;
        /// <summary>
        /// Streams of this storage.
        /// </summary>
        /// <value>Streams if supported by this storage, otherwise null.</value>
        IStorageStreams Streams { get; }
    }
}