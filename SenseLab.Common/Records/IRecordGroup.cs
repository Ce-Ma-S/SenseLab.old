using System;

namespace SenseLab.Common.Records
{
    public interface IRecordGroup :
        IId<Guid>
    {
        string Name { get; }
        string Description { get; }
    }
}
