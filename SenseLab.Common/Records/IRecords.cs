using SenseLab.Common.Data;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecords
    {
        string Name { get; }
        string Description { get; }

        IItemStorage<IRecord> Storage { get; }
        IList<IRecordTransformer> WriteRecordTransformers { get; }
    }
}
