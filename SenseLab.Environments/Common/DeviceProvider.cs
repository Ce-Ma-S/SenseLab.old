using CeMaS.Common;
using System;
using CeMaS.Common.Collections;
using System.Collections.Generic;

namespace SenseLab.Environments.Common
{
    public abstract class DeviceProvider :
        ItemInfo<Guid>,
        IDeviceProvider
    {
        public DeviceProvider(Guid id, string name, string description = null)
            : base(id, name, description)
        {
            Devices = new ObservableCollectionEx<IDevice, Guid>();
            Devices.Added.Subscribe(OnDevicesAdded);
            Devices.Removed.Subscribe(OnDevicesRemoved);
        }

        public INotifyList<IDevice, Guid> Devices { get; private set; }
        INotifyEnumerable<IDevice, Guid> IDeviceProvider.Devices
        {
            get { return Devices; }
        }

        protected virtual void OnDevicesAdded(IEnumerable<IDevice> items)
        {
        }
        protected virtual void OnDevicesRemoved(IEnumerable<IDevice> items)
        {
        }
    }
}
