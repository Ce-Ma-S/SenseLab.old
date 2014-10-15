using SenseLab.Common.Environments;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class RecordSourceInfo :
        IdNameDescription<Guid>
    {
        public RecordSourceInfo(IRecordSource source)
            : base(source.Id, source.Name, source.Description)
        {
            IsRecordable = source is IRecordable;
            Type = new IdNameDescription<Guid>(source.Type.Id, source.Type.Name, source.Type.Description);
        }

        public bool IsRecordable { get; private set; }
        public IdNameDescription<Guid> Type { get; private set; }

        public IRecordSource ToSource()
        {
            var source = RecordSource.From(Id, IsRecordable);
            if (source == null)
                source = new RecordSourceUnavailable(this);
            return source;
        }
    }
}
