using CeMaS.Common.Collections;
using SenseLab.Common;
using SenseLab.Common.Locations;
using System;
using System.Linq;
using System.Collections.Generic;
using SenseLab.Common.Records;

namespace SenseLab.Environments.Common
{
    public abstract class Environment :
        LocatableItemInfo<Guid, ISpatialLocation>,
        IEnvironment
    {
        public Environment(Guid id, string name, string description = null, ISpatialLocation location = null)
            : base(id, name, description, location)
        {
            DeviceProviders = new ObservableCollectionEx<IDeviceProvider, Guid>();
            DeviceGroups = new ObservableCollectionEx<IItemGroup<Guid, IDevice>, Guid>();
            DeviceProviders.Added.Subscribe(OnDeviceProvidersAdded);
            DeviceProviders.Removed.Subscribe(OnDeviceProvidersRemoved);
        }

        public INotifyList<IItemGroup<Guid, IDevice>, Guid> DeviceGroups { get; private set; }

        #region Device providers

        public INotifyList<IDeviceProvider, Guid> DeviceProviders { get; private set; }
        INotifyEnumerable<IDeviceProvider, Guid> IEnvironment.DeviceProviders
        {
            get { return DeviceProviders; }
        }

        IDeviceProvider IItemLookup<IDeviceProvider, Guid>.GetItem(Guid id)
        {
            IDeviceProvider item;
            if (DeviceProviders.TryGetItem(id, out item))
                return item;
            return ((IItemLookup<IDevice, Guid>)this).GetItem(id);
        }
        bool IItemLookup<IDeviceProvider, Guid>.TryGetItem(Guid id, out IDeviceProvider item)
        {
            if (DeviceProviders.TryGetItem(id, out item))
                return true;
            IDevice device;
            if (((IItemLookup<IDevice, Guid>)this).TryGetItem(id, out device))
            {
                item = device;
                return true;
            }
            return false;
        }
        bool IItemLookup<IDeviceProvider, Guid>.Contains(Guid id)
        {
            if (DeviceProviders.Contains(id))
                return true;
            return ((IItemLookup<IDevice, Guid>)this).Contains(id);
        }

        protected virtual void OnDeviceProvidersAdded(IEnumerable<IDeviceProvider> items)
        {
        }
        protected virtual void OnDeviceProvidersRemoved(IEnumerable<IDeviceProvider> items)
        {
        }

        #endregion

        #region Devices

        IDevice IItemLookup<IDevice, Guid>.GetItem(Guid id)
        {
            foreach (var deviceProvider in DeviceProviders)
            {
                IDevice device;
                if (deviceProvider.Devices.TryGetItem(id, out device, d => d.Devices))
                    return device;
            }
            throw new KeyNotFoundException();
        }
        bool IItemLookup<IDevice, Guid>.TryGetItem(Guid id, out IDevice item)
        {
            foreach (var deviceProvider in DeviceProviders)
            {
                IDevice device;
                if (deviceProvider.Devices.TryGetItem(id, out device, d => d.Devices))
                {
                    item = device;
                    return true;
                }
            }
            item = null;
            return false;
        }
        bool IItemLookup<IDevice, Guid>.Contains(Guid id)
        {
            foreach (var deviceProvider in DeviceProviders)
            {
                if (deviceProvider.Devices.Contains(id, d => d.Devices))
                    return true;
            }
            return false;
        }

        #endregion

        #region Recordables

        IRecordable IItemLookup<IRecordable, Guid>.GetItem(Guid id)
        {
            return this.AllDevices().
                Select(d => d.Recordables).
                GetItem(id);
        }
        bool IItemLookup<IRecordable, Guid>.TryGetItem(Guid id, out IRecordable item)
        {
            return this.AllDevices().
                Select(d => d.Recordables).
                TryGetItem(id, out item);
        }
        bool IItemLookup<IRecordable, Guid>.Contains(Guid id)
        {
            return this.AllDevices().
                Select(d => d.Recordables).
                Contains(id);
        }

        #endregion
    }
}
