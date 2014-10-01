using SenseLab.Common.Records;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Environments
{
    public abstract class Device<T> :
        DeviceProvider<T>,
        IDevice
        where T : IDevice
    {
        public Device(Guid id, string name, string description = null/*,
            INode parent = null*/)
            : base(id, name, description/*, parent*/)
        {
        }

        public abstract bool IsAvailable { get; }

        protected virtual void OnIsAvailableChanged()
        {
            OnPropertyChanged(() => IsAvailable);
        }
    }


    public abstract class Device :
        Device<IDevice>,
        IDevice
    {
        public Device(Guid id, string name, string description = null/*,
            INode parent = null*/)
            : base(id, name, description/*, parent*/)
        {
        }
    }
}
