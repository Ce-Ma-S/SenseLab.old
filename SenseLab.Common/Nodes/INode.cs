using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface INode :
        ILocatable<ISpatialLocation>
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }

        INode Parent { get; }
        IEnumerable<INode> Children { get; }

        IEnumerable<IRecordable> Recordables { get; }
    }
}
