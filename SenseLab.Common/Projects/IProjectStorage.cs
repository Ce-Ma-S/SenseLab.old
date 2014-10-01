using SenseLab.Common.Data;
using System;

namespace SenseLab.Common.Projects
{
    public interface IProjectStorage :
        IItemStorage<IProject, Guid>
    {
    }
}
