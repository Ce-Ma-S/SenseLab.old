using SenseLab.Common.Values;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public interface IDevice : ILocatableNode
    {
        IEnumerable<IValue> Values { get; }
    }
}
