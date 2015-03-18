using CeMaS.Common.Collections;
using SenseLab.Common.Records;
using System;

namespace SenseLab.Environments
{
    /// <summary>
    /// Environments.
    /// </summary>
    public interface IEnvironments :
        IItemLookup<IDevice, Guid>,
        IItemLookup<IRecordable, Guid>
    {
        INotifyList<IEnvironment, Guid> Items { get; }
    }
}
