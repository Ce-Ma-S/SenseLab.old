using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

namespace CeMaS.Common.Collections
{
    public static class CollectionHelper
    {
        public static TItem GetItem<TItem, TId>(this INotifyEnumerable<TItem, TId> items, TId id, Func<TItem, INotifyEnumerable<TItem, TId>> getSubitems)
            where TItem : IId<TId>
        {
            TItem item;
            if (items.TryGetItem(id, out item, getSubitems))
                return item;
            throw new KeyNotFoundException();
        }
        public static bool TryGetItem<TItem, TId>(this INotifyEnumerable<TItem, TId> items, TId id, out TItem item, Func<TItem, INotifyEnumerable<TItem, TId>> getSubitems)
            where TItem : IId<TId>
        {
            if (items.TryGetItem(id, out item))
                return true;
            foreach (var localItem in items)
            {
                var subitems = getSubitems(localItem);
                if (subitems != null && subitems.TryGetItem(id, out item, getSubitems))
                    return true;
            }
            return false;
        }
        public static bool Contains<TItem, TId>(this INotifyEnumerable<TItem, TId> items, TId id, Func<TItem, INotifyEnumerable<TItem, TId>> getSubitems)
            where TItem : IId<TId>
        {
            if (items.Contains(id))
                return true;
            foreach (var localItem in items)
            {
                var subitems = getSubitems(localItem);
                if (subitems != null && subitems.Contains(id, getSubitems))
                    return true;
            }
            return false;
        }

        public static TItem GetItem<TItem, TId>(this IEnumerable<IItemLookup<TItem, TId>> items, TId id)
            where TItem : IId<TId>
        {
            TItem item;
            if (!items.TryGetItem(id, out item))
                throw new KeyNotFoundException();
            return item;
        }
        public static bool TryGetItem<TItem, TId>(this IEnumerable<IItemLookup<TItem, TId>> items, TId id, out TItem item)
            where TItem : IId<TId>
        {
            item = items.
                Select(
                    @is =>
                    {
                        TItem i;
                        @is.TryGetItem(id, out i);
                        return i;
                    }).
                SingleOrDefault(i => i != null);
            return item != null;
        }
        public static bool Contains<TItem, TId>(this IEnumerable<IItemLookup<TItem, TId>> items, TId id)
            where TItem : IId<TId>
        {
            return items.
                Any(@is => @is.Contains(id));
        }

        public static IEnumerable<T> Ids<T>(this IEnumerable<IId<T>> items)
        {
            return items.
                Select(i => i.Id);
        }

        public static IObservable<Unit> Changed<T>(this INotifyEnumerable<T> items)
        {
            return items.Added.Merge(items.Removed).Select(_ => Unit.Default);
        }
    }
}
