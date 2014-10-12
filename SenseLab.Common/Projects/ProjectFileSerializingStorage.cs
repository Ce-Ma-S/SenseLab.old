using SenseLab.Common.Commands;
using SenseLab.Common.Data;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using SenseLab.Common.Values;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

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

        public async Task<IRecordStorage> CreateRecordStorage(Guid projectId)
        {
            ValidateWritable();
            return await Task.Run<IRecordStorage>(() => new RecordFileSerializingStorage(this, projectId, IsReadOnly));
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

        protected override async Task OnItemDeserialized(IProject item)
        {
            await base.OnItemDeserialized(item);
            ((Project)item).Records = await CreateRecordStorage(item.Id);
        }

        private static readonly Lazy<XmlSerializer<IProject>> serializer = PrepareSerializer<IProject>();
    }
}
