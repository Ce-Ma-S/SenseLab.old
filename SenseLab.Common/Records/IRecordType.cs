using System;

namespace SenseLab.Common.Records
{
    public interface IRecordType :
        IId<Guid>
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }
    }
}
