using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Cache of items comming from <see cref="IObservable"/>s which can be used as an items source.
    /// Allows to cache pushed items and optionally coordinate them with updated or removed items from supporting observables.
    /// This collection is not meant to be used for updating and underlining data source, but only for reading it.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public class ObservableCache<T> :
        ObservableCollection<T>,
        IDisposable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">See <see cref="Items"/>.</param>
        /// <param name="itemsRemoved">See <see cref="ItemsRemoved"/>.</param>
        /// <param name="capacity">See <see cref="Capacity"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
        public ObservableCache(
            IObservable<T> items,
            IObservable<T> itemsRemoved = null,
            int capacity = -1)
            : this(items, null, itemsRemoved, capacity)
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">See <see cref="Items"/>.</param>
        /// <param name="itemsUpdated">See <see cref="ItemsUpdated"/>.</param>
        /// <param name="itemsRemoved">See <see cref="ItemsRemoved"/>.</param>
        /// <param name="capacity">See <see cref="Capacity"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
        protected ObservableCache(
            IObservable<T> items,
            IObservable<T> itemsUpdated = null,
            IObservable<T> itemsRemoved = null,
            int capacity = -1)
        {
            items.ValidateNonNull("items");
            Capacity = capacity;
            Items = items;
            itemsSubscription = items.Subscribe(OnItemsNext, OnItemsCompleted);
            ItemsUpdated = itemsUpdated;
            if (itemsUpdated != null)
            {
                itemsUpdatedSubscription = itemsUpdated.Subscribe(OnItemsUpdatedNext, OnItemsUpdatedCompleted);
            }
            ItemsRemoved = itemsRemoved;
            if (itemsRemoved != null)
            {
                itemsRemovedSubscription = itemsRemoved.Subscribe(OnItemsRemovedNext, OnItemsRemovedCompleted);
            }
        }

        /// <summary>
        /// Cache capacity (size).
        /// </summary>
        /// <value>
        /// Positive means cache size limit of number of items in this collection - older items are removed when necessary.
        /// Negative value means unlimited - all cached.
        /// </value>
        /// <exception cref="ArgumentNullException">set: Value is 0.</exception>
        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                value.ValidateNonDefault("value");
                EnsureCapacity(value);
            }
        }
        /// <summary>
        /// Items which are added to this collection from a data source.
        /// </summary>
        public new IObservable<T> Items { get; private set; }
        /// <summary>
        /// Optional items which are updated in a data source so they replace older instances in this collection if they are found by <see cref="IndexOfItemDuplicate"/>. 
        /// </summary>
        public IObservable<T> ItemsUpdated { get; private set; }
        /// <summary>
        /// Optional items which are removed in a data source so they are removed from this collection as well. 
        /// </summary>
        public IObservable<T> ItemsRemoved { get; private set; }

        /// <summary>
        /// Disposes this object so that items observing is stopped.
        /// </summary>
        public virtual void Dispose()
        {
            itemsSubscription.Dispose();
            if (itemsUpdatedSubscription != null)
                itemsUpdatedSubscription.Dispose();
            if (itemsRemovedSubscription != null)
                itemsRemovedSubscription.Dispose();
        }

        /// <summary>
        /// Finds an index of an item in this collection which represents identifiable duplicate or older version of <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item duplicate or updated version in another instance.</param>
        /// <returns>
        /// Non/negative index if an item is found, otherwise negative one.
        /// This implementation always returns negative index.
        /// </returns>
        protected virtual int IndexOfItemDuplicate(T item)
        {
            return -1;
        }
        /// <summary>
        /// Called when this collection is changed to keep correct cache size.
        /// </summary>
        /// <param name="e">Arguments.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Reset:
                    EnsureCapacity(Capacity);
                    break;
                default:
                    break;
            }
        }

        private void EnsureCapacity(int capacity)
        {
            if (capacity < 0)
                return;
            for (int i = Count - capacity - 1; i >= 0; i--)
            {
                RemoveAt(0);
            }
        }
        private bool ReplaceByDuplicate(T item)
        {
            int index = IndexOfItemDuplicate(item);
            if (index >= 0)
            {
                this[index] = item;
                return true;
            }
            return false;
        }

        private void OnItemsNext(T item)
        {
            if (Contains(item) || ReplaceByDuplicate(item))
                return;
            EnsureCapacity(Capacity - 1);
            Add(item);
        }
        private void OnItemsCompleted()
        {
            itemsSubscription.Dispose();
        }
        private void OnItemsUpdatedNext(T item)
        {
            ReplaceByDuplicate(item);
        }
        private void OnItemsUpdatedCompleted()
        {
            itemsUpdatedSubscription.Dispose();
        }
        private void OnItemsRemovedNext(T item)
        {
            Remove(item);
        }
        private void OnItemsRemovedCompleted()
        {
            itemsRemovedSubscription.Dispose();
        }

        private int capacity;
        private IDisposable itemsSubscription;
        private IDisposable itemsUpdatedSubscription;
        private IDisposable itemsRemovedSubscription;
    }


    /// <summary>
    /// Cache of items comming from <see cref="IObservable"/>s which can be used as an items source.
    /// Allows to cache pushed items and optionally coordinate them with updated or removed items from supporting observables.
    /// This collection is not meant to be used for updating and underlining data source, but only for reading it.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public class ObservableCache<TItem, TId> :
        ObservableCache<TItem>
        where TItem : IId<TId>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">See <see cref="Items"/>.</param>
        /// <param name="itemsUpdated">See <see cref="ItemsUpdated"/>.</param>
        /// <param name="itemsRemoved">See <see cref="ItemsRemoved"/>.</param>
        /// <param name="capacity">See <see cref="Capacity"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
        public ObservableCache(
            IObservable<TItem> items,
            IObservable<TItem> itemsUpdated = null,
            IObservable<TItem> itemsRemoved = null,
            int capacity = -1)
            : base(items, itemsUpdated, itemsRemoved, capacity)
        {
        }

        /// <summary>
        /// Finds an index of an item in this collection which represents identifiable duplicate or older version of <paramref name="item"/>.
        /// Uses <see cref="IId{T}"/> to identify items by their identifiers.
        /// </summary>
        /// <param name="item">Item duplicate or updated version in another instance.</param>
        /// <returns>
        /// Non/negative index if an item is found, otherwise negative one.
        /// </returns>
        protected override int IndexOfItemDuplicate(TItem item)
        {
            if (item == null)
                return -1;
            TId id = item.Id;
            for (int i = Count - 1; i >= 0; i--)
            {
                var localItem = this[i];
                if (localItem == null)
                    continue;
                if (EqualityComparer<TId>.Default.Equals(localItem.Id, id))
                    return i;
            }
            return -1;
        }
    }
}
