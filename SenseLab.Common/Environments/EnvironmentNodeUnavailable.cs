using System;
using System.Collections.Generic;

namespace SenseLab.Common.Environments
{
    public class EnvironmentNodeUnavailable :
        EnvironmentNode<IEnvironmentNode>
    {
        public EnvironmentNodeUnavailable(Guid id, string name, string description)
            : base(id, name, description)
        {
        }

        public override bool IsAvailable
        {
            get { return false; }
        }
        public IEnumerable<Guid> RecordableIds { get; set; }
    }
}
