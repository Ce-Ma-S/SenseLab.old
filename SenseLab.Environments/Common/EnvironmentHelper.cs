using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Environments.Common
{
    public static class EnvironmentHelper
    {
        public static IEnumerable<IDeviceProvider> AllDeviceProviders(this IEnvironment environment)
        {
            return environment.DeviceProviders.Concat(environment.AllDevices());
        }
        public static IEnumerable<IDevice> AllDevices(this IEnvironment environment)
        {
            return environment.DeviceProviders.AsParallel().
                SelectMany(dp => dp.AllDevices());
        }
        public static IEnumerable<IDevice> AllDevices(this IDeviceProvider deviceProvider)
        {
            foreach (var device in deviceProvider.Devices)
            {
                yield return device;
                foreach (var subdevice in device.AllDevices())
                    yield return subdevice;
            }
        }
    }
}
