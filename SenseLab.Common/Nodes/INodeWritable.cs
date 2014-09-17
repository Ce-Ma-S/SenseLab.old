using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface INodeWritable :
        INode
    {
        void AddChild(INode child);
        bool RemoveChild(INode child);
    }


    public interface INodeWritable<P, C> :
        INodeWritable,
        INode<P, C>
        where P : INode
        where C : INode
    {
        void AddChild(C child);
        bool RemoveChild(C child);
    }
}
