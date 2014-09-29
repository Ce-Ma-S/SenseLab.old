using SenseLab.Common.Events;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    public abstract class ItemStorage<TItem, TId> :
        NotifyPropertyChange,
        IItemStorage<TItem, TId>
        where TItem : IId<TId>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public bool IsReadOnly { get; private set; }
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                SetProperty(() => IsConnected, ref isConnected, value);
            }
        }

        public abstract IQbservable<TItem> Items { get; }
        public abstract IObservable<TItem> ItemsUpdated { get; }
        public abstract IObservable<TItem> ItemsRemoved { get; }

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

        public async Task Add(TItem item)
        {
            ValidateWritable();
            await DoAdd(item);
        }
        public async Task<bool> Update(TItem item)
        {
            ValidateWritable();
            bool contains = await Items.Contains(item.Id);
            if (!contains)
                return false;
            return await DoUpdate(item);
        }
        public abstract Task<bool> Remove(TId itemId);
        
        protected abstract Task DoConnect();
        protected abstract Task DoDisconnect();

        protected void ValidateWritable()
        {
            if (IsReadOnly)
                throw new AccessViolationException("Storage is read only and cannot be modified.");
        }
        protected abstract Task DoAdd(TItem item);
        protected abstract Task<bool> DoUpdate(TItem item);

        private bool isConnected;
    }
}
