using System;

namespace SenseLab.Common.Nodes
{
    public class NodeInfo :
        IdNameDescription<Guid>
    {
        public NodeInfo(INode node)
            : base(node.Id, node.Name, node.Description)
        {

        }
    }
}
