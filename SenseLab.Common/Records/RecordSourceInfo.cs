using CeMaS.Common;
using SenseLab.Common.Commands;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class RecordSourceInfo :
        ItemInfo<Guid>
    {
        protected RecordSourceInfo(IRecordSource source)
            : base(source.Id, source.Name, source.Description)
        {
            IsAvailable = source.IsAvailable;
            IsRecordable = source is IRecordable;
            Type = new RecordType(source.Type);
        }

        public static RecordSourceInfo Create(IRecordSource source)
        {
            if (source is IRecordableCommand)
                return new RecordableCommandInfo((IRecordableCommand)source);
            return new RecordSourceInfo(source);
        }

        [DataMember]
        public bool IsAvailable { get; private set; }
        [DataMember]
        public bool IsRecordable { get; private set; }
        [DataMember]
        public RecordType Type { get; private set; }
    }
}
