namespace CeMaS.Data.Storages
{
    /// <summary>
    /// Storage aware object.
    /// </summary>
    public interface IStorageAware
    {
        /// <summary>
        /// Storage associated with this object.
        /// </summary>
        /// <value>non-null</value>
        IStorage Storage { get; }
    }
}
