using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Subjects;
using CeMaS.Common.Validation;

namespace CeMaS.Common.Collections
{
    public class ObservableCollectionEx<T> :
        ObservableCollection<T>,
        INotifyList<T>
    {
        public ObservableCollectionEx()
        {
        }
        public ObservableCollectionEx(IEnumerable<T> items)
            : base(items)
        {
        }

        public IObservable<IEnumerable<T>> Added
        {
            get
            {
                return added;
            }
        }
        public IObservable<IEnumerable<T>> Removed
        {
            get
            {
                return removed;
            }
        }

        public void Add(IEnumerable<T> items)
        {
            Insert(Count, items);
        }
        public void Insert(int index, IEnumerable<T> items)
        {
            items.ValidateNonNull(nameof(items));
            var added = items.ToArray();
            foreach (var item in added)
                Items.Insert(index, item);
            if (added.Length > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnAdded(added);
            }
        }
        public bool Remove(IEnumerable<T> items)
        {
            items.ValidateNonNull(nameof(items));
            var removed = new List<T>();
            foreach (var item in items)
            {
                if (Items.Remove(item))
                    removed.Add(item);
            }
            if (removed.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnRemoved(removed);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnAdded(item);
        }
        protected override void SetItem(int index, T item)
        {
            T oldItem = this[index];
            base.SetItem(index, item);
            OnRemoved(oldItem);
            OnAdded(item);
        }
        protected override void RemoveItem(int index)
        {
            T oldItem = this[index];
            base.RemoveItem(index);
            OnRemoved(oldItem);
        }
        protected override void ClearItems()
        {
            var oldItems = this.ToArray();
            base.ClearItems();
            OnRemoved(oldItems);
        }

        protected virtual void OnAdded(IEnumerable<T> items)
        {
            if (items != null && items.Any())
                added.OnNext(items);
        }
        protected void OnAdded(T item)
        {
            OnAdded(new[] { item });
        }
        protected virtual void OnRemoved(IEnumerable<T> items)
        {
            if (items != null && items.Any())
                removed.OnNext(items);
        }
        protected void OnRemoved(T item)
        {
            OnRemoved(new[] { item });
        }

        private Subject<IEnumerable<T>> added = new Subject<IEnumerable<T>>();
        private Subject<IEnumerable<T>> removed = new Subject<IEnumerable<T>>();
    }


    public class ObservableCollectionEx<TItem, TId> :
        ObservableCollectionEx<TItem>,
        INotifyList<TItem, TId>
        where TItem : IId<TId>
    {
        public ObservableCollectionEx()
        {
        }
        public ObservableCollectionEx(IEnumerable<TItem> items)
            : base(items)
        {
            foreach (var item in items)
                idToItem.Add(item.Id, item);
        }

        public TItem GetItem(TId itemId)
        {
            return idToItem[itemId];
        }
        public bool TryGetItem(TId itemId, out TItem item)
        {
            return idToItem.TryGetValue(itemId, out item);
        }
        public bool Contains(TId itemId)
        {
            return idToItem.ContainsKey(itemId);
        }

        public bool Remove(TId id)
        {
            TItem item;
            if (!TryGetItem(id, out item))
                return false;
            return Remove(item);
        }
        public bool Remove(IEnumerable<TId> ids)
        {
            bool result = false;
            foreach (var id in ids)
            {
                if (Remove(id))
                    result = true;
            }
            return result;
        }

        protected override void OnAdded(IEnumerable<TItem> items)
        {
            foreach (var item in items)
                idToItem.Add(item.Id, item);
            base.OnAdded(items);
        }
        protected override void OnRemoved(IEnumerable<TItem> items)
        {
            foreach (var item in items)
                idToItem.Remove(item.Id);
            base.OnRemoved(items);
        }

        private Dictionary<TId, TItem> idToItem = new Dictionary<TId, TItem>();
    }
}
