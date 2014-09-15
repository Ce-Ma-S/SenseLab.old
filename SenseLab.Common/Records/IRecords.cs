using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecords
    {
        IEnumerable<IRecord> Records { get; }
    }
}
