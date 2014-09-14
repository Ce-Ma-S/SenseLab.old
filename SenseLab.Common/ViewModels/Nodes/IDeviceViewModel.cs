using SenseLab.Common.Values;
using System.Collections.Generic;

namespace SenseLab.Common.ViewModels.Nodes
{
    public interface IDeviceViewModel : ILocatableNodeViewModel
    {
        IEnumerable<IValue> Values { get; }
    }
}
