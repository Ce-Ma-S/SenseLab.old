using SenseLab.Common.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SenseLab.Common.Collections
{
    public class ObservableCollectionEx<T> :
        ObservableCollection<T>,
        INotifyEnumerable<T>
    {
        public ObservableCollectionEx()
        {
        }
        public ObservableCollectionEx(IEnumerable<T> items)
            : base(items)
        {
        }

        public event EventHandler<ValueChangeEventArgs<IEnumerable<T>>> ItemContainmentChanged;

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnItemContainmentChanged(item, true);
        }
        protected override void SetItem(int index, T item)
        {
            T oldItem = this[index];
            base.SetItem(index, item);
            OnItemContainmentChanged(oldItem, item);
        }
        protected override void RemoveItem(int index)
        {
            T oldItem = this[index];
            base.RemoveItem(index);
            OnItemContainmentChanged(oldItem, false);
        }
        protected override void ClearItems()
        {
            var oldItems = this.ToArray();
            base.ClearItems();
            OnItemContainmentChanged(oldItems, Enumerable.Empty<T>());
        }

        protected virtual void OnItemContainmentChanged(IEnumerable<T> oldItems, IEnumerable<T> newItems)
        {
            if (ItemContainmentChanged != null)
            {
                if (oldItems == null)
                    oldItems = Enumerable.Empty<T>();
                if (newItems == null)
                    newItems = Enumerable.Empty<T>();
                ItemContainmentChanged(this, new ValueChangeEventArgs<IEnumerable<T>>(oldItems, newItems));
            }
        }
        protected void OnItemContainmentChanged(T oldItem, T newItem)
        {
            var oldItems = new[] { oldItem };
            var newItems = new[] { newItem };
            OnItemContainmentChanged(oldItems, newItems);
        }
        protected void OnItemContainmentChanged(T item, bool isNew)
        {
            var oldItems = isNew ? Enumerable.Empty<T>() : new[] { item };
            var newItems = isNew ? new[] { item } : Enumerable.Empty<T>();
            OnItemContainmentChanged(oldItems, newItems);
        }
    }


    public class ObservableCollectionEx<TItem, TId> :
        ObservableCollectionEx<TItem>
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

        protected override void OnItemContainmentChanged(IEnumerable<TItem> oldItems, IEnumerable<TItem> newItems)
        {
            foreach (var item in oldItems)
                idToItem.Remove(item.Id);
            foreach (var item in newItems)
                idToItem.Add(item.Id, item);
            base.OnItemContainmentChanged(oldItems, newItems);
        }

        private Dictionary<TId, TItem> idToItem = new Dictionary<TId, TItem>();
    }
}
