using CeMaS.Common.Collections;
using CeMaS.Common.Validation;
using SenseLab.Environments.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenseLab.Environments.Remote
{
    public class DeviceProvider :
        Common.DeviceProvider
    {
        private DeviceProvider(Environment environment, IEnvironmentService service,
            Guid id, string name, string description)
            : base(id, name, description)
        {
            this.environment = environment;
            this.service = service;
        }

        internal static async Task<DeviceProvider> Create(Guid id, Environment environment, IEnvironmentService service)
        {
            service.ValidateNonNull(nameof(service));
            var info = await service.DeviceProvider(id);
            var result = new DeviceProvider(environment, service,
                id, info.Name, info.Description);
            await FillDevices(environment, service, info, result.Devices);
            return result;
        }

        internal static async Task FillDevices(Environment environment, IEnvironmentService service, DeviceProviderInfo info, INotifyList<IDevice, Guid> devices)
        {
            IEnumerable<IDevice> localDevices = null;
            await Task.Run(() =>
            {
                localDevices = info.DeviceIds.
                  AsParallel().AsOrdered().
                  Select(did => Device.Create(did, environment, service).Result);
            });
            devices.Add(localDevices);
        }

        private readonly Environment environment;
        private readonly IEnvironmentService service;
    }
}