using SenseLab.Common.Environments;
using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Wraps an environment node to be in a project.
    /// </summary>
    public interface IProjectNode :
        INode<INode, IProjectNode>,
        ILocatable<ISpatialLocation>
    {
        /// <summary>
        /// Node this wrapper works with.
        /// </summary>
        IEnvironmentNode Node { get; }
        /// <summary>
        /// Whether <see cref="Node"/> is selected.
        /// </summary>
        bool IsSelected { get; }
        /// <summary>
        /// Selected recordables of <see cref="Node"/>.
        /// </summary>
        IEnumerable<IRecordable> SelectedRecordables { get; }
    }
}
