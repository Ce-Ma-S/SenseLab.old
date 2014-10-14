using SenseLab.Common.Collections;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Environments
{
    /// <summary>
    /// Environment node with equipment like devices etc.
    /// </summary>
    public interface IEnvironmentNode :
        INode<IEnvironmentNode>
    {
        bool IsAvailable { get; }
        INotifyEnumerable<IRecordable> Recordables { get; }
    }
}
