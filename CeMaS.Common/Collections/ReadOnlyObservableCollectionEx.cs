using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CeMaS.Common.Collections
{
    public class ReadOnlyObservableCollectionEx<T> :
        ReadOnlyObservableCollection<T>,
        INotifyList<T>
    {
        public ReadOnlyObservableCollectionEx(ObservableCollectionEx<T> list)
            : base(list)
        {
        }

        public IObservable<IEnumerable<T>> Added
        {
            get { return ((ObservableCollectionEx<T>)Items).Added; }
        }
        public IObservable<IEnumerable<T>> Removed
        {
            get { return ((ObservableCollectionEx<T>)Items).Removed; }
        }

        public void Add(IEnumerable<T> items)
        {
            throw new InvalidOperationException();
        }
        public void Insert(int index, IEnumerable<T> items)
        {
            throw new InvalidOperationException();
        }
        public bool Remove(IEnumerable<T> items)
        {
            throw new InvalidOperationException();
        }
    }


    public class ReadOnlyObservableCollectionEx<TItem, TId> :
        ReadOnlyObservableCollectionEx<TItem>,
        INotifyList<TItem, TId>
        where TItem : IId<TId>
    {
        public ReadOnlyObservableCollectionEx(ObservableCollectionEx<TItem, TId> list)
            : base(list)
        {
        }

        public bool Contains(TId key)
        {
            return ((ObservableCollectionEx<TItem, TId>)Items).Contains(key);
        }
        public TItem GetItem(TId key)
        {
            return ((ObservableCollectionEx<TItem, TId>)Items).GetItem(key);
        }
        public bool TryGetItem(TId key, out TItem item)
        {
            return ((ObservableCollectionEx<TItem, TId>)Items).TryGetItem(key, out item);
        }

        public bool Remove(TId id)
        {
            throw new InvalidOperationException();
        }
        public bool Remove(IEnumerable<TId> ids)
        {
            throw new InvalidOperationException();
        }
    }
}
