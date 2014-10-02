using System;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface INode :
        IId<Guid>
    {
        string Name { get; }
        string Description { get; }

        IEnumerable<INode> Children { get; }
    }


    public interface INode<T> :
        INode
        where T : INode
    {
        new IEnumerable<T> Children { get; }
    }
}
