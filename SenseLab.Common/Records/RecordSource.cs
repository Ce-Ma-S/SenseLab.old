using SenseLab.Common.Environments;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public abstract class RecordSource :
        IRecordSource
    {
        public RecordSource(Guid id, IRecordType type, string name, string description = null)
        {
            type.ValidateNonNull("type");
            name.ValidateNonNullOrEmpty("name");
            Id = id;
            Type = type;
            Name = name;
            Description = description;
        }

        [DataMember]
        public Guid Id { get; private set; }
        [DataMember]
        public IRecordType Type { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        public abstract bool IsAvailable { get; }

        public static IRecordSource From(Guid sourceId, bool isRecordable)
        {
            IRecordSource source;
            if (!idToSource.TryGetValue(sourceId, out source))
            {
                if (isRecordable)
                    source = EnvironmentHelper.RecordableFromId(sourceId);
                else
                    source = RecordSourcesNonRecordable.Instance.TryGetFromId(sourceId);
                if (source != null)
                    idToSource.Add(sourceId, source);
            }
            return source;
        }

        private static readonly Dictionary<Guid, IRecordSource> idToSource = new Dictionary<Guid, IRecordSource>();
    }
}
