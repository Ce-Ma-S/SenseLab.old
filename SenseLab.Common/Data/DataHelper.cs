using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace SenseLab.Common.Data
{
    public static class DataHelper
    {
        //public static IObservable<bool> Contains<TItem, TId>(this IQbservable<TItem> items, TId itemId)
        //    where TItem : IId<TId>
        //{
        //    return items.Any(item => EqualityComparer<TId>.Default.Equals(item.Id, itemId));
        //}
        //public static IObservable<TItem> FirstOrDefault<TItem, TId>(this IQbservable<TItem> items, TId itemId)
        //    where TItem : IId<TId>
        //{
        //    return items.FirstOrDefaultAsync(item => EqualityComparer<TId>.Default.Equals(item.Id, itemId));
        //}

        public static ObservableCache<TItem, TId> ToObservableCache<TItem, TId>(this IItemStorage<TItem, TId> itemStorage, int capacity = -1)
            where TItem : IId<TId>
        {
            return new ObservableCache<TItem, TId>(itemStorage.Items, itemStorage.ItemsUpdated, itemStorage.ItemsRemoved, capacity);
        }
    }
}
