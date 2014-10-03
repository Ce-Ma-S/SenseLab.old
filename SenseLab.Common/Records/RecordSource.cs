using System;
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
    }
}
