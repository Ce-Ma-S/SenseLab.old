using SenseLab.Common.Nodes;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Environments
{
    public class EnvironmentNodeUnavailable :
        EnvironmentNode<IEnvironmentNode>
    {
        public EnvironmentNodeUnavailable(NodeInfo nodeInfo)
            : base(nodeInfo.Id, nodeInfo.Name, nodeInfo.Description)
        {
        }

        public override bool IsAvailable
        {
            get { return false; }
        }
        public IEnumerable<Guid> RecordableIds { get; set; }
    }
}
