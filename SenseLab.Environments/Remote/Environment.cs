using SenseLab.Environments.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SenseLab.Common.Locations;
using CeMaS.Common.Collections;
using SenseLab.Common.Records;
using System.ServiceModel;

namespace SenseLab.Environments.Remote
{
    [CallbackBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true
        )]
    public class Environment :
        Common.Environment,
        IEnvironmentChangesService
    {
        #region Init

        private Environment()
            : base(Guid.Empty, " ")
        {
            recordProviders = new Dictionary<int, RecordProvider>();
        }

        public static async Task<Environment> Create(
            string endpointConfigurationName = null,
            System.Uri endpointAddress = null)
        {
            var environment = new Environment();
            // client channel
            var serviceFactory = endpointConfigurationName == null ?
                new DuplexChannelFactory<IEnvironmentService>(environment) :
                new DuplexChannelFactory<IEnvironmentService>(environment, endpointConfigurationName);
            var service = endpointAddress == null ?
                serviceFactory.CreateChannel() :
                serviceFactory.CreateChannel(new EndpointAddress(endpointAddress));
            environment.service = service;
            // info
            var info = await service.Environment();
            environment.Id = info.Id;
            environment.Name = info.Name;
            if (endpointAddress == null)
                endpointAddress = serviceFactory.Endpoint.Address.Uri;
            environment.Description = info.Description;
            // replace local endpoint uris with current one as location
            if (info.Location is SenseLab.Common.Locations.Uri ||
                info.Location is SpatialLocationGroup &&
                ((SpatialLocationGroup)info.Location).Locations.
                    All(l => l is SenseLab.Common.Locations.Uri))
            {
                environment.Location = new SenseLab.Common.Locations.Uri(endpointAddress);
            }
            else
            {
                environment.Location = info.Location;
            }
            // device providers
            IEnumerable<IDeviceProvider> deviceProviders = null;
            await Task.Run(() =>
            {
                deviceProviders = info.DeviceProviderIds.
                  AsParallel().AsOrdered().
                  Select(dpid => Remote.DeviceProvider.Create(dpid, environment, service).Result);
            });
            environment.DeviceProviders.Add(deviceProviders);

            return environment;
        }

        #endregion

        #region Uninit

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                ((IDisposable)service).Dispose();
        }

        #endregion

        #region IEnvironmentService

        private IEnvironmentService service;
        
        #endregion

        #region IEnvironmentChangesService

        #region Environment

        void IEnvironmentChangesService.Environment_NameChanged(string value)
        {
            Name = value;
        }
        void IEnvironmentChangesService.Environment_DescriptionChanged(string value)
        {
            Description = value;
        }
        void IEnvironmentChangesService.Environment_LocationChanged(ISpatialLocation value)
        {
            Location = value;
        }
        void IEnvironmentChangesService.Environment_DeviceProvidersAdded(IEnumerable<Guid> ids)
        {
            var deviceProviders = ids.
                AsParallel().AsOrdered().
                Select(id => Remote.DeviceProvider.Create(id, this, service).Result);
            DeviceProviders.Add(deviceProviders);
        }
        void IEnvironmentChangesService.Environment_DeviceProvidersRemoved(IEnumerable<Guid> ids)
        {
            DeviceProviders.Remove(ids);
        }

        #endregion

        #region Device providers / Devices

        void IEnvironmentChangesService.DeviceProvider_NameChanged(Guid deviceProviderId, string value)
        {
            DeviceProvider(deviceProviderId).Name = value;
        }
        void IEnvironmentChangesService.DeviceProvider_DescriptionChanged(Guid deviceProviderId, string value)
        {
            DeviceProvider(deviceProviderId).Description = value;
        }
        void IEnvironmentChangesService.DeviceProvider_DevicesAdded(Guid deviceProviderId, IEnumerable<Guid> ids)
        {
            var deviceProvider = DeviceProvider(deviceProviderId);
            var devices = ids.
                AsParallel().AsOrdered().
                Select(id => Remote.Device.Create(id, this, service).Result);
            deviceProvider.Devices.Add(devices);
        }
        void IEnvironmentChangesService.DeviceProvider_DevicesRemoved(Guid deviceProviderId, IEnumerable<Guid> ids)
        {
            DeviceProvider(deviceProviderId).Devices.Remove(ids);
        }

        private DeviceProvider DeviceProvider(Guid deviceProviderId)
        {
            return (DeviceProvider)((IItemLookup<IDeviceProvider, Guid>)this).GetItem(deviceProviderId);
        }

        #endregion

        #region Devices

        void IEnvironmentChangesService.Device_IsAvailableChanged(Guid deviceId, bool value)
        {
            Device(deviceId).IsAvailable = value;
        }
        void IEnvironmentChangesService.Device_IsConnectedChanged(Guid deviceId, bool value)
        {
            Device(deviceId).IsConnected = value;
        }
        void IEnvironmentChangesService.Device_LocationChanged(Guid deviceId, ISpatialLocation value)
        {
            Device(deviceId).Location = value;
        }

        private Device Device(Guid id)
        {
            return (Device)((IItemLookup<IDevice, Guid>)this).GetItem(id);
        }

        #endregion

        #region Recordables

        async void IEnvironmentChangesService.RecordProvider_NextRecord(int id, IRecord record)
        {
            var recordProvider = recordProviders[id];
            await recordProvider.AddRecord(record);
        }

        internal void AddRecordProvider(RecordProvider recordProvider)
        {
            recordProviders.Add(recordProvider.Id, recordProvider);
        }
        internal void RemoveRecordProvider(RecordProvider recordProvider)
        {
            recordProviders.Remove(recordProvider.Id);
        }

        private readonly Dictionary<int, RecordProvider> recordProviders;

        #endregion

        #endregion
    }
}
