using SenseLab.Common.Events;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Storage base.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public abstract class ItemStorage<TItem, TId> :
        NotifyPropertyChange,
        IItemStorage<TItem, TId>
        where TItem : IId<TId>
    {
        public ItemStorage(Guid id, string name, string description, bool isReadOnly)
        {
            name.ValidateNonNullOrEmpty("name");
            Id = id;
            Name = name;
            Description = description;
            IsReadOnly = isReadOnly;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        #region Connection

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                SetProperty(() => IsConnected, ref isConnected, value);
            }
        }

        public async Task Connect()
        {
            if (IsConnected)
                throw new InvalidProgramException("Storage is already connected.");
            await DoConnect();
        }
        public async Task Disconnect()
        {
            if (!IsConnected)
                throw new InvalidProgramException("Storage is already disconnected.");
            await DoDisconnect();
        }

        protected abstract Task DoConnect();
        protected abstract Task DoDisconnect();

        private bool isConnected;

        #endregion
        
        #region Items

        public abstract IQbservable<TItem> Items { get; }

        public abstract Task<bool> Contains(TId itemId);
        public abstract Task<TItem> FirstOrDefault(TId itemId);

        #endregion

        #region Updates

        public bool IsReadOnly { get; private set; }

        public abstract IObservable<TItem> ItemsUpdated { get; }
        public abstract IObservable<TItem> ItemsRemoved { get; }

        public async Task Add(TItem item)
        {
            ValidateWritable();
            bool contains = await Contains(item.Id);
            if (contains)
                throw new ArgumentOutOfRangeException("item.Id", item.Id, "Item to be added is alredy in this storage.");
            await DoAdd(item);
        }
        public async Task Update(TItem item)
        {
            ValidateWritable();
            bool contains = await Contains(item.Id);
            if (!contains)
                throw new ArgumentOutOfRangeException("item.Id", item.Id, "Item to be updated is not in this storage yet.");
            await DoUpdate(item);
        }
        public abstract Task<bool> Remove(TId itemId);

        protected void ValidateWritable()
        {
            if (IsReadOnly)
                throw new AccessViolationException("Storage is read only and cannot be modified.");
        }
        protected abstract Task DoAdd(TItem item);
        protected abstract Task DoUpdate(TItem item); 

        #endregion
    }
}
