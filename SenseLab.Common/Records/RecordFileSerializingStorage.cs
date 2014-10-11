using SenseLab.Common.Data;
using SenseLab.Common.Projects;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    internal class RecordFileSerializingStorage :
        FileSerializingStorage<IRecord, KeyValuePair<Guid, uint>>,
        IRecordStorage
    {
        public RecordFileSerializingStorage(ProjectFileSerializingStorage projectStorage, Guid projectId, bool isReadOnly)
            : base(Guid.NewGuid(), recordsName, null, isReadOnly, recordSerializer.Value,
                projectStorage.FileFolderPath, projectStorage.FileExtension, projectStorage.GetNameFromItemId(projectId), recordsName)
        {
        }

        protected override ISerializingStorage GetItemStorage(string itemName, Type itemType)
        {
            if (itemType == typeof(RecordSourceInfo))
            {
                return GetItemStorage<RecordSourceInfo, Guid>(itemName, recordSourceInfoSerializer.Value);
            }
            else if (itemType == typeof(IRecordGroup))
            {
                return GetItemStorage<IRecordGroup, Guid>(itemName, recordGroupInfoSerializer.Value);
            }
            return base.GetItemStorage(itemName, itemType);
        }

        private const string recordsName = "Records";
        private static readonly Lazy<XmlSerializer<IRecord>> recordSerializer = ProjectFileSerializingStorage.PrepareSerializer<IRecord>();
        private static readonly Lazy<XmlSerializer<RecordSourceInfo>> recordSourceInfoSerializer = ProjectFileSerializingStorage.PrepareSerializer<RecordSourceInfo>();
        private static readonly Lazy<XmlSerializer<IRecordGroup>> recordGroupInfoSerializer = ProjectFileSerializingStorage.PrepareSerializer<IRecordGroup>();
    }
}
