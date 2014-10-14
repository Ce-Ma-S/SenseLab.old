using SenseLab.Common.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SenseLab.Common.Collections
{
    public class ReadOnlyObservableCollectionEx<T> :
        ReadOnlyObservableCollection<T>,
        INotifyEnumerable<T>
    {
        public ReadOnlyObservableCollectionEx(ObservableCollectionEx<T> list)
            : base(list)
        {
            list.ItemContainmentChanged += OnItemContainmentChanged;
        }

        public event EventHandler<ValueChangeEventArgs<IEnumerable<T>>> ItemContainmentChanged;

        protected virtual void OnItemContainmentChanged(ValueChangeEventArgs<IEnumerable<T>> e)
        {
            if (ItemContainmentChanged != null)
                ItemContainmentChanged(this, e);
        }
        private void OnItemContainmentChanged(object sender, ValueChangeEventArgs<IEnumerable<T>> e)
        {
            OnItemContainmentChanged(e);
        }
    }


    public class ReadOnlyObservableCollectionEx<TItem, TEnum> :
        ReadOnlyObservableCollectionEx<TItem>,
        INotifyEnumerable<TEnum>
        where TItem : TEnum
    {
        public ReadOnlyObservableCollectionEx(ObservableCollectionEx<TItem> list)
            : base(list)
        {
        }

        event EventHandler<ValueChangeEventArgs<IEnumerable<TEnum>>> INotifyItemContainmentChanged<TEnum>.ItemContainmentChanged
        {
            add { itemContainmentChanged += value; }
            remove { itemContainmentChanged -= value; }
        }

        IEnumerator<TEnum> IEnumerable<TEnum>.GetEnumerator()
        {
            foreach (var item in this)
                yield return item;
        }

        protected override void OnItemContainmentChanged(ValueChangeEventArgs<IEnumerable<TItem>> e)
        {
            base.OnItemContainmentChanged(e);
            if (itemContainmentChanged != null)
            {
                itemContainmentChanged(this,
                    new ValueChangeEventArgs<IEnumerable<TEnum>>(
                        e.OldValue.Value.Cast<TEnum>(),
                        e.NewValue.Cast<TEnum>()));
            }
        }

        private event EventHandler<ValueChangeEventArgs<IEnumerable<TEnum>>> itemContainmentChanged;
    }
}
