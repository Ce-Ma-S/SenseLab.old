using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface INode
    {
        string Name { get; }
        string Description { get; }

        INode Parent { get; }
        IEnumerable<INode> Children { get; }
    }
}
