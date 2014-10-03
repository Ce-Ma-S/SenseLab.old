using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class RecordType :
        IdNameDescription<Guid>,
        IRecordType
    {
        public RecordType(Guid id, string name, string description = null)
            : base(id, name, description)
        {
        }
    }
}
