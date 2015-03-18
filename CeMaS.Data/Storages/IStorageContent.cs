using System;

namespace CeMaS.Data.Storages
{
    /// <summary>
    /// Content of a storage.
    /// </summary>
    public interface IStorageContent :
        IStorageAware,
        IDisposable
    {
        /// <summary>
        /// Whether this storage content is only for reading.
        /// </summary>
        bool IsReadOnly { get; }
    }
}
