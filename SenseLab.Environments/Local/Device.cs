using CeMaS.Common.Collections;
using SenseLab.Common.Locations;
using System;
using System.Collections.Generic;

namespace SenseLab.Environments.Local
{
    public abstract class Device :
        Common.Device
    {
        private Device(Environment environment,
            Guid id, string name, string description, ISpatialLocation location = null)
            : base(id, name, description, location)
        {
            this.environment = environment;
        }

        #region IEnvironmentChangesService

        protected override void OnIsAvailableChanged()
        {
            base.OnIsAvailableChanged();
            environment.OnChange(service => service.Device_IsAvailableChanged(Id, IsAvailable));
        }
        protected override void OnIsConnectedChanged()
        {
            base.OnIsConnectedChanged();
            environment.OnChange(service => service.Device_IsConnectedChanged(Id, IsConnected));
        }
        protected override void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            base.OnLocationChanged(oldValue, newValue);
            environment.OnChange(service => service.Device_LocationChanged(Id, newValue));
        }
        protected override void OnLocationChanged(ISpatialLocation location)
        {
            base.OnLocationChanged(location);
            environment.OnChange(service => service.Environment_LocationChanged(location));
        }
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