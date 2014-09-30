using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Storage with items serialized to zip file(s).
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public class FileSerializingStorage<TItem, TId> :
        SerializingStorage<TItem, TId>
        where TItem : IId<TId>
    {
        public FileSerializingStorage(Guid id, string name, string description, bool isReadOnly,
            ISerializer<TItem> serializer,
            string folderPath, string fileExtension,
            string fileName = null,
            string entryFolderPath = null, string entryName = null,
            CompressionLevel compressionLevel = CompressionLevel.Optimal)
            : base(id, name, description, isReadOnly, serializer)
        {
            folderPath.ValidateNonNullOrEmpty("folderPath");
            fileExtension.ValidateNonNullOrEmpty("fileExtension");
            FileFolderPath = folderPath;
            FileExtension = fileExtension;
            FileName = fileName;
            EntryFolderPath = entryFolderPath;
            EntryName = entryName;
            CompressionLevel = compressionLevel;
        }

        #region Items

        public override IObservable<TId> ItemIds
        {
            get
            {
                string filePath, entryPath;
                const string wildCard = "*";
                GetItemFileAndEntryPath(default(TId), out filePath, out entryPath, itemId => wildCard);
                // one item per file
                // file name is item id specific
                if (filePath.Contains(wildCard))
                {
                    return Directory.EnumerateFiles(FileFolderPath, filePath)
                        .Select(name => GetItemIdFromName(Path.GetFileNameWithoutExtension(name)))
                        .ToObservable();
                }
                // multiple items per file
                // file name is fixed
                else
                {
                    // entry name is item id specific
                    if (entryPath.Contains(wildCard))
                    {
                        using (var zip = ZipFile.OpenRead(filePath))
                        {
                            IEnumerable<ZipArchiveEntry> entries = zip.Entries;
                            if (string.IsNullOrEmpty(EntryFolderPath))
                                entries = entries.Where(entry => entry.FullName == entry.Name);
                            else
                                entries = entries.Where(entry => entry.FullName.StartsWith(EntryFolderPath));
                            return entries.Select(entry => GetItemIdFromName(entry.Name))
                                .ToObservable();
                        }
                    }
                    // entry name is fixed
                    else
                    {
                        // this should not be
                        return Observable.Empty<TId>();
                    }
                }
            }
        }

        #endregion

        #region Files

        /// <summary>
        /// Folder path of item file(s).
        /// </summary>
        /// <value>Non-empty.</value>
        public string FileFolderPath { get; private set; }
        /// <summary>
        /// Item file name.
        /// </summary>
        /// <value>
        /// Non-empty means all items are stored in one file with this name.
        /// Otherwise each item has its own file with <see cref="GetNameFromItemId"/> name.
        /// </value>
        public string FileName { get; private set; }
        /// <summary>
        /// Item file(s) extension (with or without leading dot).
        /// </summary>
        /// <value>Non-empty.</value>
        public string FileExtension { get; private set; }
        /// <summary>
        /// Item entry folder path in its file.
        /// </summary>
        /// <value>
        /// Non-empty means entry folder is created for an item.
        /// Otherwise no entry folder is created and the entry is at the root of its file.
        /// </value>
        public string EntryFolderPath { get; private set; }
        /// <summary>
        /// Item entry name in its file.
        /// </summary>
        /// <value>
        /// Non-empty means item is stored with this entry name.
        /// Otherwise entry name is <see cref="GetNameFromItemId"/>.
        /// </value>
        public string EntryName { get; private set; }
        /// <summary>
        /// Compression level for item entry added to its file.
        /// </summary>
        public CompressionLevel CompressionLevel
        {
            get { return compressionLevel; }
            set
            {
                SetProperty(() => CompressionLevel, ref compressionLevel, value);
            }
        }

        public void GetItemFileAndEntryPath(TId itemId, out string filePath, out string entryPath)
        {
            GetItemFileAndEntryPath(itemId, out filePath, out entryPath, GetNameFromItemId);
        }

        protected virtual string GetNameFromItemId(TId itemId)
        {
            return itemId.ToString();
        }
        protected virtual TId GetItemIdFromName(string name)
        {
            return (TId)idTypeConverter.ConvertFromString(name);
        }

        protected override async Task<Stream> OpenItemStreamForReading(TId itemId)
        {
            return await Task.Run(
                () =>
                {
                    string filePath, entryPath;
                    GetItemFileAndEntryPath(itemId, out filePath, out entryPath);
                    var zip = ZipFile.OpenRead(filePath);
                    var entry = zip.GetEntry(entryPath);
                    return entry.Open();
                });
        }
        protected override async Task<Stream> CreateItemStreamForWriting(TId itemId)
        {
            return await Task.Run(
                () =>
                {
                    string filePath, entryPath;
                    GetItemFileAndEntryPath(itemId, out filePath, out entryPath);
                    var zip = ZipFile.Open(filePath, ZipArchiveMode.Update);
                    var entry = zip.GetEntry(entryPath);
                    if (entry != null)
                        entry.Delete();
                    entry = zip.CreateEntry(entryPath, CompressionLevel);
                    return entry.Open();
                });
        }
        protected override async Task<bool> RemoveItemStream(TId itemId)
        {
            return await Task.Run(
                () =>
                {
                    string filePath, entryPath;
                    GetItemFileAndEntryPath(itemId, out filePath, out entryPath);
                    using (var zip = ZipFile.Open(filePath, ZipArchiveMode.Update))
                    {
                        var entry = zip.GetEntry(entryPath);
                        if (entry != null)
                        {
                            entry.Delete();
                            // remove file if empty
                            if (zip.Entries.Count == 0)
                            {
                                zip.Dispose();
                                File.Delete(filePath);
                            }
                            return true;
                        }
                        return false;
                    }
                });
        }

        private void GetItemFileAndEntryPath(TId itemId, out string filePath, out string entryPath, Func<TId, string> getNameFromItemId)
        {
            string fileName;
            string nameFromItemId = null;
            if (string.IsNullOrEmpty(FileName))
            {
                nameFromItemId = getNameFromItemId(itemId);
                fileName = nameFromItemId;
            }
            else
            {
                fileName = FileName;
            }
            fileName = Path.ChangeExtension(fileName, FileExtension);
            filePath = Path.Combine(FileFolderPath, fileName);

            string entryName;
            if (string.IsNullOrEmpty(EntryName))
            {
                if (nameFromItemId == null)
                {
                    nameFromItemId = getNameFromItemId(itemId);
                }
                entryName = nameFromItemId;
            }
            else
            {
                entryName = EntryName;
            }
            entryPath = Path.Combine(EntryFolderPath, entryName);
        }

        private static readonly TypeConverter idTypeConverter = TypeDescriptor.GetConverter(typeof(TId));

        private CompressionLevel compressionLevel;

        #endregion
    }
}
