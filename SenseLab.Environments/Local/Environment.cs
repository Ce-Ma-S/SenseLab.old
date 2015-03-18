using CeMaS.Common.Collections;
using CeMaS.Data.Streams;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using SenseLab.Common.Values;
using SenseLab.Environments.Properties;
using SenseLab.Environments.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace SenseLab.Environments.Local
{
    [ServiceBehavior(
        Namespace = "CeMaS",
        Name = "SenseLab.Environment",
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true
        )]
    public class Environment :
        Common.Environment,
        IEnvironmentService
    {
        #region Init

        public Environment()
            : this(GetId(), GetName(), GetDescription(), GetLocation())
        {
        }
        private Environment(Guid id, string name, string description = null, ISpatialLocation location = null)
            : base(id, name, description, location)
        {
            recordProviders = new Dictionary<int, IRecordProvider>();
            recordSubscriptions = new Dictionary<int, IDisposable>();
            changesServices = new List<IEnvironmentChangesService>();
        }

        public static ServiceHost CreateHost(params System.Uri[] baseAddresses)
        {
            var environment = new Environment();
            // service host
            var host = new ServiceHost(environment, baseAddresses);
            // local endpoint uris as location
            if (environment.Location == null)
            {
                var endpoints = host.Description.Endpoints;
                Debug.Assert(endpoints.Count > 0);
                environment.Location = endpoints.Count > 1 ?
                    new SpatialLocationGroup(endpoints.
                        Select(ep => new SenseLab.Common.Locations.Uri(ep.Address.Uri)).
                        ToArray()) :
                    (ISpatialLocation)new SenseLab.Common.Locations.Uri(endpoints[0].Address.Uri);
            }
            return host;
        }

        private static Guid GetId()
        {
            Guid id = Settings.Default.EnvironmentId;
            // generate and remember id
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
                Settings.Default.EnvironmentId = id;
                Settings.Default.Save();
            }
            return id;
        }
        private static string GetName()
        {
            string name = Settings.Default.EnvironmentName;
            if (string.IsNullOrEmpty(name))
                name = System.Environment.MachineName;
            return name;
        }
        private static string GetDescription()
        {
            var description = Settings.Default.EnvironmentDescription;
            if (string.IsNullOrEmpty(description))
                description = System.Environment.UserDomainName;
            return description == GetName() ?
                null :
                description;
        }
        private static ISpatialLocation GetLocation()
        {
            string locationText = Settings.Default.EnvironmentLocation;
            return string.IsNullOrEmpty(locationText) ?
                null :
                new SpatialTextLocation(locationText);
        }

        #endregion

        #region IEnvironmentService

        #region Environment

        Task<EnvironmentInfo> IEnvironmentService.Environment()
        {
            return Task.FromResult(new EnvironmentInfo(this));
        }

        #endregion

        #region Device providers

        Task<DeviceProviderInfo> IEnvironmentService.DeviceProvider(Guid id)
        {
            IDeviceProvider item;
            ((IItemLookup<IDeviceProvider, Guid>)this).TryGetItem(id, out item);
            return Task.FromResult(new DeviceProviderInfo(item));
        }

        #endregion

        #region Devices

        Task<DeviceInfo> IEnvironmentService.Device(Guid id)
        {
            IDevice item;
            ((IItemLookup<IDevice, Guid>)this).TryGetItem(id, out item);
            return Task.FromResult(new DeviceInfo(item));
        }
        Task IEnvironmentService.Device_Connect(Guid id)
        {
            var device = ((IItemLookup<IDevice, Guid>)this).GetItem(id);
            device.Connect();
            return Task.FromResult<object>(null);
        }
        Task IEnvironmentService.Device_Disconnect(Guid id)
        {
            var device = ((IItemLookup<IDevice, Guid>)this).GetItem(id);
            device.Disconnect();
            return Task.FromResult<object>(null);
        }

        #endregion

        #region Recordables

        Task<RecordSourceInfo> IEnvironmentService.Recordable(Guid id)
        {
            IRecordable item;
            ((IItemLookup<IRecordable, Guid>)this).TryGetItem(id, out item);
            return Task.FromResult(new RecordSourceInfo(item));
        }

        async Task<RecordProviderInfo> IEnvironmentService.Recordable_CreateRecordProvider(Guid id)
        {
            var recordProvider = await Recordable(id).CreateRecordProvider();
            int recordProviderId = Interlocked.Increment(ref lastRecordProviderId);
            recordProviders.Add(recordProviderId, recordProvider);
            return RecordProviderInfo.Create(recordProviderId, recordProvider);
        }
        Task<RecordProviderInfo> IEnvironmentService.RecordProvider(int id)
        {
            var recordProvider = RecordProvider(id);
            return Task.FromResult(RecordProviderInfo.Create(id, recordProvider));
        }
        async Task IEnvironmentService.RecordProvider_Dispose(int id)
        {
            var recordProvider = RecordProvider(id);
            await Task.Run(() => recordProvider.Dispose());
            recordProviders.Remove(id);
        }
        async Task IEnvironmentService.RecordProvider_Start(int id)
        {
            var recordProvider = RecordProvider(id);
            if (recordProvider.IsStarted)
                return;
            var recordSubscription = recordProvider.Subscribe(record => OnNextRecord(id, record));
            recordSubscriptions.Add(id, recordSubscription);
            await recordProvider.Start();
        }
        async Task IEnvironmentService.RecordProvider_Pause(int id)
        {
            var recordProvider = RecordProvider(id);
            await recordProvider.Pause();
        }
        async Task IEnvironmentService.RecordProvider_Unpause(int id)
        {
            var recordProvider = RecordProvider(id);
            await recordProvider.Unpause();
        }
        async Task IEnvironmentService.RecordProvider_Stop(int id)
        {
            var recordProvider = RecordProvider(id);
            if (!recordProvider.IsStarted)
                return;
            var recordSubscription = recordSubscriptions[id];
            recordSubscription.Dispose();
            await recordProvider.Stop();
        }

        async Task<IRecord> IEnvironmentService.SamplingRecordProvider_AddRecord(int id)
        {
            var recordProvider = (ISamplingRecordProvider)RecordProvider(id);
            return await Task.Run(() => recordProvider.AddRecord());
        }
        async Task IEnvironmentService.SamplingRecordProvider_SetRecordPeriodEnabled(int id, bool value)
        {
            var recordProvider = (ISamplingRecordProvider)RecordProvider(id);
            await Task.Run(() => recordProvider.RecordPeriodEnabled = value);
        }
        async Task IEnvironmentService.SamplingRecordProvider_SetRecordPeriod(int id, TimeSpan value)
        {
            var recordProvider = (ISamplingRecordProvider)RecordProvider(id);
            await Task.Run(() => recordProvider.RecordPeriod = value);
        }

        async Task IEnvironmentService.ValueRecordProvider_SetBatchSize(int id, int value)
        {
            var recordProvider = (IValueRecordProvider)RecordProvider(id);
            await Task.Run(() => recordProvider.BatchSize = value);
        }
        async Task IEnvironmentService.ValueRecordProvider_SetBatchPeriod(int id, TimeSpan? value)
        {
            var recordProvider = (IValueRecordProvider)RecordProvider(id);
            await Task.Run(() => recordProvider.BatchPeriod = value);
        }

        async Task IEnvironmentService.StreamingRecordProvider_SetStreamManagerId(int id, Guid value)
        {
            var recordProvider = (IStreamingRecordProvider)RecordProvider(id);
            var streamManager = StreamManagers.Instance.GetItem(value);
            await recordProvider.SetStreamManager(streamManager);
        }

        private IRecordable Recordable(Guid id)
        {
            return ((IItemLookup<IRecordable, Guid>)this).GetItem(id);
        }
        private IRecordProvider RecordProvider(int id)
        {
            return recordProviders[id];
        }

        private readonly Dictionary<int, IRecordProvider> recordProviders;
        private readonly Dictionary<int, IDisposable> recordSubscriptions;
        private int lastRecordProviderId;

        #endregion

        #region Changes

        async Task IEnvironmentService.SubscribeToChanges()
        {
            var changesService = OperationContext.Current.GetCallbackChannel<IEnvironmentChangesService>();
            if (changesServices.Contains(changesService))
                throw new InvalidOperationException("Changes service is already subscribed to changes.");
            changesServices.Add(changesService);
            await Task.Yield();
        }
        async Task IEnvironmentService.UnsubscribeFromChanges()
        {
            var changesService = OperationContext.Current.GetCallbackChannel<IEnvironmentChangesService>();
            if (!changesServices.Contains(changesService))
                throw new InvalidOperationException("Changes service is already unsubscribed from changes.");
            changesServices.Remove(changesService);
            await Task.Yield();
        }

        #endregion

        #endregion

        #region IEnvironmentChangesService

        protected internal virtual void OnChange(Action<IEnvironmentChangesService> onChange)
        {
            if (changesServices != null)
                Parallel.ForEach(changesServices, onChange);
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            OnChange(service => service.Environment_NameChanged(Name));
        }
        protected override void OnDescriptionChanged()
        {
            base.OnDescriptionChanged();
            OnChange(service => service.Environment_DescriptionChanged(Description));
        }
        protected override void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            base.OnLocationChanged(oldValue, newValue);
            OnChange(service => service.Environment_LocationChanged(newValue));
        }
        protected override void OnLocationChanged(ISpatialLocation location)
        {
            base.OnLocationChanged(location);
            OnChange(service => service.Environment_LocationChanged(location));
        }
        protected override void OnDeviceProvidersAdded(IEnumerable<IDeviceProvider> items)
        {
            base.OnDeviceProvidersAdded(items);
            OnChange(service => service.Environment_DeviceProvidersAdded(items.Ids()));
        }
        protected override void OnDeviceProvidersRemoved(IEnumerable<IDeviceProvider> items)
        {
            base.OnDeviceProvidersRemoved(items);
            OnChange(service => service.Environment_DeviceProvidersRemoved(items.Ids()));
        }
        protected virtual void OnNextRecord(int id, IRecord record)
        {
            OnChange(service => service.RecordProvider_NextRecord(id, record));
        }

        private readonly List<IEnvironmentChangesService> changesServices;

        #endregion
    }
}
