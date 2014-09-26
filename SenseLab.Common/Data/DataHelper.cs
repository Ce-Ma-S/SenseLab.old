namespace SenseLab.Common.Data
{
    public static class DataHelper
    {
        public static ObservableCache<T> ToObservableCache<T>(this IItemStorage<T> itemStorage, int capacity = -1)
        {
            return new ObservableCache<T>(itemStorage.Items, itemStorage.ItemsRemoved, capacity);
        }
        public static ObservableCache<TItem, TId> ToObservableCache<TItem, TId>(this IItemStorage<TItem, TId> itemStorage, int capacity = -1)
            where TItem : IId<TId>
        {
            return new ObservableCache<TItem, TId>(itemStorage.Items, itemStorage.ItemsUpdated, itemStorage.ItemsRemoved, capacity);
        }
    }
}
