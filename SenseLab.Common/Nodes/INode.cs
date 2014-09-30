using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface INode :
        IId<Guid>
    {
        string Name { get; }
        string Description { get; }

        //INode Parent { get; }
        IEnumerable<INode> Children { get; }
    }


    public interface INode</*P,*/ C> :
        INode
        //where P : INode
        where C : INode
    {
        //new P Parent { get; }
        new IEnumerable<C> Children { get; }
    }
}
