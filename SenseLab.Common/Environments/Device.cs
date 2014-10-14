using System;

namespace SenseLab.Common.Environments
{
    public abstract class Device<T> :
        DeviceProvider<T>,
        IDevice
        where T : IDevice
    {
        public Device(Guid id, string name, string description = null)
            : base(id, name, description)
        {
        }
    }


    public abstract class Device :
        Device<IDevice>,
        IDevice
    {
        public Device(Guid id, string name, string description = null)
            : base(id, name, description)
        {
        }
    }
}
