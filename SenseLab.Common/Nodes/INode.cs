using System;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface INode
    {
        Guid Id { get; }
        bool IsEnabled { get; }
        string Name { get; }
        string Description { get; }

        INode Parent { get; }
        IEnumerable<INode> Children { get; }
    }
}
