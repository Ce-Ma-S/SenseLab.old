using SenseLab.Common.Data;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecordStorage :
        IItemStorage<IRecord, KeyValuePair<Guid, uint>>        
    {
    }
}
