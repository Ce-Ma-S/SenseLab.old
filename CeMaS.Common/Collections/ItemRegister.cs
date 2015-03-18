namespace CeMaS.Common.Collections
{
    public class ItemRegister<TItem, TId> :
        ReadOnlyObservableCollectionEx<TItem, TId>,
        IItemRegister<TItem, TId>
        where TItem : IId<TId>
    {
        protected ItemRegister()
            : base(new ObservableCollectionEx<TItem, TId>())
        {
        }

        public void Register(TItem item)
        {
            Items.Add(item);
        }
    }
}
