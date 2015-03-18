using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Provides access to identifiable items.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public interface IItemRegister<TItem, TId> :
        IItemLookup<TItem, TId>
        where TItem : IId<TId>
    {
        /// <summary>
        /// Registers <paramref name="item"/> in this register.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> withe the same identifier is already registered.</exception>
        void Register(TItem item);
    }
}
