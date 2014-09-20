using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Environments
{
    /// <summary>
    /// Environment node with equipment like devices etc.
    /// </summary>
    public interface IEnvironmentNode :
        INode<INode, IEnvironmentNode>
    {
        IEnumerable<IRecordable> Recordables { get; }
    }
}
