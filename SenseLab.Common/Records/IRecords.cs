using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecords :
        ICollection<IRecord>
    {
        string Name { get; }
        string Description { get; }
    }
}
