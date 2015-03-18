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
            await DeviceProvider.FillDevices(environment, service, info, result.Devices);
            IEnumerable<IRecordable> recordables = null;
            await Task.Run(() =>
            {
                recordables = info.DeviceIds.
                  AsParallel().AsOrdered().
                  Select(rid => Recordable.Create(rid, environment, service).Result);
            });
            return result;
        }

        public new bool IsAvailable
        {
            get { return isAvailable; }
            internal set
            {
                SetProperty(() => IsAvailable, ref isAvailable, value, OnIsAvailableChanged);
            }
        }
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

        protected override bool GetIsAvailable()
        {
            return IsAvailable;
        }

        private readonly Environment environment;
        private readonly IEnvironmentService service;
        private bool isAvailable;
        private bool isConnected;
    }
}