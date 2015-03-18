using CeMaS.Common;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class RecordType :
        ItemInfo<Guid>,
        IRecordType
    {
        public RecordType(Guid id, string name, string description = null)
            : base(id, name, description)
        {
        }
        public RecordType(IRecordType type)
            : base(type.Id, type.Name, type.Description)
        {
        }
    }
}
