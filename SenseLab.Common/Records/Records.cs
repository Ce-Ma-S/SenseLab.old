using SenseLab.Common.Data;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public class Records :
        IRecords
    {
        public IItemStorage<IIdValue<uint, IRecord>, uint> Storage { get; private set; }
        public IList<IRecordTransformer> WriteRecordTransformers { get; private set; }
    }
}
