using SenseLab.Common.Collections;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface INode :
        IId<Guid>
    {
        string Name { get; }
        string Description { get; }

        INotifyEnumerable<INode> Children { get; }
    }


    public interface INode<out T> :
        INode
        where T : INode
    {
        new IEnumerable<T> Children { get; }
    }
}
