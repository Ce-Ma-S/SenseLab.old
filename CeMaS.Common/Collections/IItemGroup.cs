namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Groups items.
    /// </summary>
    /// <typeparam name="TGroupId">Group identifier type.</typeparam>
    /// <typeparam name="TItem">Item type.</typeparam>
    public interface IItemGroup<TGroupId, TItem> :
        IItemInfo<TGroupId>
    {
        /// <summary>
        /// Subgroups.
        /// </summary>
        INotifyList<IItemGroup<TGroupId, TItem>> Children { get; }
        /// <summary>
        /// Items in this group.
        /// </summary>
        INotifyList<TItem> Items { get; }
    }
}
