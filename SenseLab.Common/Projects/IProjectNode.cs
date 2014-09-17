using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Wraps another node to be in a project.
    /// </summary>
    public interface IProjectNode :
        INodeWritable<IProjectNode, IProjectNode>
    {
        /// <summary>
        /// Node this wrapper works with.
        /// </summary>
        INode Node { get; }
        /// <summary>
        /// Whether <see cref="Node"/> is selected.
        /// </summary>
        bool IsSelected { get; }
        /// <summary>
        /// Selects or unselects <paramref name="recordables"/>.
        /// </summary>
        /// <param name="select">true to select, false to unselect.</param>
        /// <param name="recordables">
        /// <see cref="IRecordable"/>s of <see cref="Node"/> to be selected or unselected.
        /// null means all recordables.
        /// </param>
        void ChangeRecordablesSelection(bool select, IEnumerable<IRecordable> recordables = null);
    }
}
