using SenseLab.Common.Data;
using System;

namespace SenseLab.Common.Environments
{
    public interface IEnvironmentStorage :
        IItemStorage<IEnvironment, Guid>
    {
    }
}
