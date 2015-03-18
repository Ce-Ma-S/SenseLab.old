using CeMaS.Common;
using System;

namespace SenseLab.Common.Records
{
    public interface IRecordSource :
        IItemInfo<Guid>,
        IAvailable
    {
        /// <summary>
        /// Record type.
        /// </summary>
        IRecordType Type { get; }
    }
}
