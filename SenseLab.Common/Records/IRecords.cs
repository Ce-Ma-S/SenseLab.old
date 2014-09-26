using SenseLab.Common.Data;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecords
    {
        IItemStorage<IIdValue<uint, IRecord>, uint> Storage { get; }
        IList<IRecordTransformer> WriteRecordTransformers { get; }
    }
}
