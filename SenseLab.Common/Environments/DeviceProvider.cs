using SenseLab.Common.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Common.Environments
{
    public abstract class DeviceProvider<T> :
        EnvironmentNode<T>,
        IDeviceProvider
        where T : IDevice
    {
        public DeviceProvider(Guid id, string name, string description = null)
            : base(id, name, description)
        {
        }

        IEnumerable<IDevice> INode<IDevice>.Children
        {
            get { return Children.Cast<IDevice>(); }
        }
    }
}
