using SenseLab.Common.Data;
using System;

namespace SenseLab.Common.Environments
{
    /// <summary>
    /// Environments.
    /// </summary>
    public interface IEnvironments        
    {
        IItemStorage<IEnvironment, Guid> Storage { get; }
    }
}
