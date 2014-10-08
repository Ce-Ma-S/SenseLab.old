using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Storage with items serialized to stream(s).
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public abstract class SerializingStorage<TItem, TId> :
        ItemStorage<TItem, TId>
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

        protected virtual string GetNameFromItemId(TId itemId)
        {
            return itemId.ToString();
        }
        protected virtual TId GetItemIdFromName(string name)
        {
            return (TId)idTypeConverter.ConvertFromString(name);
        }

        protected async Task SerializeItem(TItem item)
        {
            using (var itemStream = await CreateStreamForWriting(null, GetNameFromItemId(item.Id)))
            {
                await Serializer.Serialize(item, itemStream);
            }
        }
        protected async Task<TItem> DeserializeItem(TId itemId)
        {
            using (var itemStream = await OpenStreamForReading(null, GetNameFromItemId(itemId)))
            {
                return await Serializer.Deserialize(itemStream); 
            }
        }

        [DataMember]
        private Guid SerializerId
        {
            get { return Serializers<TItem>.Instance.GetId(Serializer); }
            set { Serializer = Serializers<TItem>.Instance.GetFromId(value); }
        }

        private static readonly TypeConverter idTypeConverter = TypeDescriptor.GetConverter(typeof(TId));

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

        public override async Task<bool> Remove(TId itemId)
        {
            return await RemoveStream(null, GetNameFromItemId(itemId));
        }

        protected override async Task DoAdd(TItem item)
        {
            await SerializeItem(item);
        }
        protected override async Task DoUpdate(TItem item)
        {
            await SerializeItem(item);
        }

        #endregion
    }
}
