using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    public abstract class SerializingStorage<TItem, TId> :
        ItemStorage<TItem, TId>
        where TItem : IId<TId>
    {
        public SerializingStorage(Guid id, string name, string description, bool isReadOnly,
            XmlObjectSerializer serializer, string folderPath, string fileExtension)
            : base(id, name, description, isReadOnly)
        {
            serializer.ValidateNonNull("serializer");
            folderPath.ValidateNonNullOrEmpty("folderPath");
            fileExtension.ValidateNonNullOrEmpty("fileExtension");
            Serializer = serializer;
            FolderPath = folderPath;
            FileExtension = fileExtension;
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

        #region Files

        public string FolderPath { get; private set; }
        public string FileExtension { get; private set; }

        public string GetFilePath(TId itemId)
        {
            return Path.Combine(FolderPath, NameFromItemId(itemId));
        }

        protected virtual string NameFromItemId(TId itemId)
        {
            return itemId.ToString();
        }
        protected abstract TId ItemIdFromName(string name);

        protected async Task<Stream> GetItemStream(TId itemId, FileAccess access)
        {
            var filePath = GetFilePath(itemId);
            return await Task.Run(() => File.Open(filePath, FileMode.OpenOrCreate, access));
        }
        protected async Task<bool> RemoveItemStream(TId itemId)
        {
            var filePath = GetFilePath(itemId);
            return await Task.Run(
                () =>
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        return true;
                    }
                    return false;
                });
        }

        #endregion

        #region Items

        public IObservable<TId> ItemIds
        {
            get
            {
                var pattern = Path.ChangeExtension("*", FileExtension);
                return Directory.EnumerateFiles(FolderPath, pattern)
                    .Select(name => ItemIdFromName(Path.GetFileNameWithoutExtension(name)))
                    .ToObservable();
            }
        }
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

        protected XmlObjectSerializer Serializer { get; private set; }

        protected async Task SerializeItem(TItem item)
        {
            var itemStream = await GetItemStream(item.Id, FileAccess.Write);
            await SerializeItem(item, itemStream);
        }
        protected virtual async Task SerializeItem(TItem item, Stream itemStream)
        {
            await Task.Run(() =>
            {
                Serializer.WriteObject(itemStream, item);
                // truncate the rest if any
                itemStream.SetLength(itemStream.Position);
            });
        }
        protected async Task<TItem> DeserializeItem(TId itemId)
        {
            var itemStream = await GetItemStream(itemId, FileAccess.Read);
            return await DeserializeItem(itemStream);
        }
        protected virtual async Task<TItem> DeserializeItem(Stream itemStream)
        {
            return await Task.Run(() => (TItem)Serializer.ReadObject(itemStream));
        }

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
            return await RemoveItemStream(itemId);
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
