using CeMaS.Common;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class RecordSourceInfo :
        ItemInfo<Guid>
    {
        public RecordSourceInfo(IRecordSource source)
            : base(source.Id, source.Name, source.Description)
        {
            IsAvailable = source.IsAvailable;
            IsRecordable = source is IRecordable;
            Type = new RecordType(source.Type);
        }

        [DataMember]
        public bool IsAvailable { get; private set; }
        [DataMember]
        public bool IsRecordable { get; private set; }
        [DataMember]
        public RecordType Type { get; private set; }
    }
}
