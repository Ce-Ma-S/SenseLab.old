using SenseLab.Common.Commands;
using SenseLab.Common.Data;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using SenseLab.Common.Values;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace SenseLab.Common.Projects
{
    public class ProjectFileSerializingStorage :
        FileSerializingStorage<IProject, Guid>,
        IProjectStorage
    {
        public ProjectFileSerializingStorage(Guid id, string name, string description, bool isReadOnly, string folderPath)
            : base(id, name, description, isReadOnly, serializer.Value, folderPath, "slp")
        {
        }

        #region KnownTypes

        public static readonly IList<Type> KnownTypes = new List<Type>(new[] {
            typeof(Project),

            typeof(ValueRecord<>),
            typeof(StreamableRecord),
            typeof(CommandRecord<>),
            
            typeof(Time),

            typeof(SpatialTextLocation),
            typeof(SpatialLocationGroup<>),
            typeof(Point),
            
            typeof(RecordGroup)
            });

        #endregion

        public IRecordStorage CreateRecordStorage(Guid projectId, bool isReadOnly)
        {
            return new RecordFileSerializingStorage(this, projectId, isReadOnly);
        }

        internal static Lazy<XmlSerializer<T>> PrepareSerializer<T>()
        {
            return new Lazy<XmlSerializer<T>>(() =>
                new XmlSerializer<T>(
                    new DataContractJsonSerializer(typeof(T), KnownTypes)));
        }

        protected override Guid GetItemIdFromName(string name)
        {
            return Guid.Parse(name);
        }
        protected override void OnItemDeserialized(IProject item)
        {
            base.OnItemDeserialized(item);
            ((Project)item).Records = CreateRecordStorage(item.Id, false);
        }

        private static readonly Lazy<XmlSerializer<IProject>> serializer = PrepareSerializer<IProject>();
    }
}
