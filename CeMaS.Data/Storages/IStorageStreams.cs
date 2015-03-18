using CeMaS.Data.Streams;

namespace CeMaS.Data.Storages
{
    /// <summary>
    /// Streams of a storage.
    /// </summary>
    public interface IStorageStreams :
        IStorageContent,
        IStreamManager
    {
    }
}
