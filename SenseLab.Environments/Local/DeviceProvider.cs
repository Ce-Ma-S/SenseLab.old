using CeMaS.Common.Collections;
using System;
using System.Collections.Generic;

namespace SenseLab.Environments.Local
{
    public abstract class DeviceProvider :
        Common.DeviceProvider
    {
        private DeviceProvider(Environment environment,
            Guid id, string name, string description)
            : base(id, name, description)
        {
            this.environment = environment;
            Devices.Added.Subscribe(OnDevicesAdded);
            Devices.Removed.Subscribe(OnDevicesRemoved);
        }

        #region IEnvironmentChangesService

        protected override void OnDevicesAdded(IEnumerable<IDevice> items)
        {
            base.OnDevicesAdded(items);
            environment.OnChange(service => service.DeviceProvider_DevicesAdded(Id, items.Ids()));
        }
        protected override void OnDevicesRemoved(IEnumerable<IDevice> items)
        {
            base.OnDevicesRemoved(items);
            environment.OnChange(service => service.DeviceProvider_DevicesRemoved(Id, items.Ids()));
        }

        private readonly Environment environment;

        #endregion
    }
}