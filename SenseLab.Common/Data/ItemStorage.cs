using SenseLab.Common.Events;
using System;
using System.IO;
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

        public async Task Save(TItem item)
        {
            ValidateWritableAndConnected();
            await DoSave(item);
        }
        public async Task<bool> Remove(TId itemId)
        {
            ValidateWritableAndConnected();
            return await DoRemove(itemId);
        }

        protected void ValidateWritableAndConnected()
        {
            if (IsReadOnly)
                throw new AccessViolationException("Storage is read only and cannot be modified.");
            if (!IsConnected)
                throw new AccessViolationException("Storage is not connected.");
        }
        protected abstract Task DoSave(TItem item);
        protected abstract Task<bool> DoRemove(TId itemId);

        #endregion

        #region Streams

        public abstract Task<Stream> OpenStreamForReading(string namespaceName, string name);
        public abstract Task<Stream> CreateStreamForWriting(string namespaceName, string name);
        public abstract Task<bool> RemoveStream(string namespaceName, string name);

        #endregion
    }
}
