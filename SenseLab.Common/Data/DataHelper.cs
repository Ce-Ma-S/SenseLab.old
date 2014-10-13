using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    public static class DataHelper
    {
        public static ObservableCache<TItem, TId> ToObservableCache<TItem, TId>(this IItemStorage<TItem, TId> itemStorage, int capacity = -1)
                    where TItem : IId<TId>
        {
            return new ObservableCache<TItem, TId>(itemStorage.Items, itemStorage.ItemsUpdated, itemStorage.ItemsRemoved, capacity);
        }

        public static bool IsWritableAndConnected<TItem, TId>(this IItemStorage<TItem, TId> itemStorage)
            where TItem : IId<TId>
        {
            return itemStorage != null && !itemStorage.IsReadOnly && itemStorage.IsConnected;
        }
        public static async Task CopyTo<TItem, TId>(this IItemStorage<TItem, TId> sourceItemStorage, IItemStorage<TItem, TId> targetItemStorage)
            where TItem : IId<TId>
        {
            sourceItemStorage.ValidateNonNull("sourceItemStorage");
            targetItemStorage.ValidateNonNull("targetItemStorage");
            await sourceItemStorage.Items.Do(item => targetItemStorage.Save(item));
        }
    }
}
