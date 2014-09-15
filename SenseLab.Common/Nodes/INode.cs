using SenseLab.Common.Locations;
using SenseLab.Common.Values;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface INode :
        ILocatable
    {
        Guid Id { get; }
        bool IsEnabled { get; }
        string Name { get; }
        string Description { get; }

        INode Parent { get; }
        IEnumerable<INode> Children { get; }
        IEnumerable<IValue> Values { get; }
    }
}
