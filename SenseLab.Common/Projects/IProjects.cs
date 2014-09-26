using SenseLab.Common.Data;
using System;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Projects.
    /// </summary>
    public interface IProjects
    {
        IItemStorage<IProject, Guid> Storage { get; }
    }
}
