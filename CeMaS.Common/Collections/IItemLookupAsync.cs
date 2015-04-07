using System.Threading.Tasks;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Provides access to identifiable items.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public interface IItemLookupAsync<TItem, TId>
        where TItem : IId<TId>
    {
        /// <summary>
        /// Finds an item with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <exception cref="KeyNotFoundException">No item wit <paramref name="id"/> was found.</exception>
        Task<TItem> GetItem(TId id);
        /// <summary>
        /// Finds an item with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="item">Found item.</param>
        /// <returns>Item if found.</returns>
        Task<OptionalValue<TItem>> TryGetItem(TId id);
        /// <summary>
        /// Whether an item with <paramref name="id"/> is available.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        Task<bool> Contains(TId id);
    }
}
