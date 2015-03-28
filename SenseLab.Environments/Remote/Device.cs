using CeMaS.Common.Validation;
using System;
using System.Threading.Tasks;
using SenseLab.Environments.Service;
using System.Collections.Generic;
using SenseLab.Common.Records;
using SenseLab.Common.Locations;
using System.Linq;

namespace SenseLab.Environments.Remote
{
    public class Device :
        Common.Device
    {
        private Device(Environment environment, IEnvironmentService service,
            Guid id, string name, string description, ISpatialLocation location,
            bool isAvailable, bool isConnected)
            : base(id, name, description, location)
        {
            this.environment = environment;
            this.service = service;
        }

        internal static async Task<Device> Create(Guid id, Environment environment, IEnvironmentService service)
        {
            service.ValidateNonNull(nameof(service));
            var info = await service.Device(id);
            var result = new Device(environment, service,
                id, info.Name, info.Description, info.Location,
                info.IsAvailable, info.IsConnected);
            await DeviceProvider.AddDevices(environment, service, info.DeviceIds, result.Devices);
            await AddRecordables(environment, service, info.RecordableIds, result);
            return result;
        }
        internal static async Task AddRecordables(Environment environment, IEnvironmentService service,
            IEnumerable<Guid> recordableIds, Device device)
        {
            IEnumerable<IRecordable> recordables = null;
            await Task.Run(() =>
            {
                recordables = recordableIds.
                  AsParallel().AsOrdered().
                  Select(rid => Recordable.Create(rid, environment, service).Result);

            });
            device.Recordables.Add(recordables);
        }

        #region IsAvailable

        public new bool IsAvailable
        {
            get { return isAvailable; }
            internal set
            {
                SetProperty(() => IsAvailable, ref isAvailable, value, OnIsAvailableChanged);
            }
        }

        protected override bool GetIsAvailable()
        {
            return IsAvailable;
        }

        private bool isAvailable;

        #endregion

        #region IsConnected

        public new bool IsConnected
        {
            get { return isConnected; }
            internal set
            {
                SetProperty(() => IsConnected, ref isConnected, value, OnIsConnectedChanged);
            }
        }
        protected override Task DoConnect()
        {
            return service.Device_Connect(Id);
        }
        protected override Task DoDisconnect()
        {
            return service.Device_Disconnect(Id);
        }
        protected override bool GetIsConnected()
        {
            return IsConnected;
        }

        private bool isConnected;

        #endregion

        private readonly Environment environment;
        private readonly IEnvironmentService service;
    }
}