using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Storage with items serialized to stream(s).
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public abstract class SerializingStorage<TItem, TId> :
        ItemStorage<TItem, TId>,
        ISerializingStorage
        where TItem : IId<TId>
    {
        public SerializingStorage(Guid id, string name, string description, bool isReadOnly,
            ISerializer<TItem> serializer)
            : base(id, name, description, isReadOnly)
        {
            serializer.ValidateNonNull("serializer");
            Serializer = serializer;
        }

        #region Connection

        protected override Task DoConnect()
        {
            throw new NotImplementedException();
        }
        protected override Task DoDisconnect()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Items

        public abstract IObservable<TId> ItemIds { get; }
        public override IQbservable<TItem> Items
        {
            get
            {
                return ItemIds.Select(itemId => DeserializeItem(itemId).Result)
                    .AsQbservable();
            }
        }

        public override async Task<bool> Contains(TId itemId)
        {
            return await ItemIds.Contains(itemId);
        }
        public override async Task<TItem> FirstOrDefault(TId itemId)
        {
            if (await Contains(itemId))
                return await DeserializeItem(itemId);
            return default(TItem);
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Item serializer.
        /// </summary>
        /// <value>Non-null.</value>
        public ISerializer<TItem> Serializer { get; private set; }

        public async Task SerializeItem(TItem item)
        {
            using (var itemStream = await CreateStreamForWriting(null, GetNameFromItemId(item.Id)))
            {
                await Serializer.Serialize(item, itemStream);
            }
            var itemSerializable = item as ISerializableSubitems;
            if (itemSerializable == null)
                return;
            foreach (var subitemInfo in itemSerializable.SubitemInfos)
            {
                var subitemStorage = ValidateItemStorage(subitemInfo);
                var subitem = itemSerializable[subitemInfo];
                await subitemStorage.SerializeItem(subitem);
            }
        }
        public async Task SerializeItem(object item)
        {
            await SerializeItem((TItem)item);
        }
        public async Task<TItem> DeserializeItem(TId itemId)
        {
            TItem item;
            using (var itemStream = await OpenStreamForReading(null, GetNameFromItemId(itemId)))
            {
                item = await Serializer.Deserialize(itemStream);
            }
            var itemSerializable = item as ISerializableSubitems;
            if (itemSerializable == null)
                return item;
            foreach (var subitemInfo in itemSerializable.SubitemInfos)
            {
                var subitemStorage = ValidateItemStorage(subitemInfo);
                var subitem = await subitemStorage.DeserializeItem(subitemInfo.Id);
                itemSerializable[subitemInfo] = subitem;
            }
            await OnItemDeserialized(item);
            return item;
        }
        public async Task<object> DeserializeItem(object itemId)
        {
            return await DeserializeItem((TId)itemId);
        }
        public virtual string GetNameFromItemId(TId itemId)
        {
            return itemId.ToString();
        }
        
        protected virtual ISerializingStorage GetItemStorage(string itemName, Type itemType)
        {
            return null;
        }
        protected ISerializingStorage ValidateItemStorage(SerializableItemInfo itemInfo)
        {
            var itemStorage = GetItemStorage(itemInfo.Name, itemInfo.Type);
            if (itemStorage == null)
                throw new NotSupportedException(string.Format("No item storage available for {0}.", itemInfo));
            return itemStorage;
        }

#pragma warning disable 1998
        protected virtual async Task OnItemDeserialized(TItem item)
        {
        }
#pragma warning restore

        #endregion

        #region Updates

        public override IObservable<TItem> ItemsUpdated
        {
            get { throw new NotImplementedException(); }
        }
        public override IObservable<TItem> ItemsRemoved
        {
            get { throw new NotImplementedException(); }
        }

        protected override async Task DoSave(TItem item)
        {
            await SerializeItem(item);
        }
        protected override async Task<bool> DoRemove(TId itemId)
        {
            return await RemoveStream(null, GetNameFromItemId(itemId));
        }

        #endregion
    }
}
