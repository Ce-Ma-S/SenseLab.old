using System;

namespace SenseLab.Common.Records
{
    public interface IRecordType :
        IId<Guid>
    {
        string Name { get; }
        string Description { get; }
    }
}
