using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Lab project with selected nodes and recordables of <see cref="IEnvironment"/>.
    /// </summary>
    public interface IProject :
        IProjectNode
    {
        /// <summary>
        /// Records recorded.
        /// </summary>
        IEnumerable<IRecords> Records { get; }
    }
}
