using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using CeMaS.Common;
using CeMaS.Common.Collections;

namespace SenseLab.Environments.Service
{
    [DataContract]
    public class DeviceProviderInfo :
        ItemInfo<Guid>
    {
        public DeviceProviderInfo(Guid id, string name, string description = null,
            IEnumerable < Guid> deviceIds = null)
            : base(id, name, description)
        {
            if (deviceIds == null)
                deviceIds = Enumerable.Empty<Guid>();
            DeviceIds = deviceIds;
        }
        public DeviceProviderInfo(IDeviceProvider deviceProvider)
            : this(deviceProvider.Id, deviceProvider.Name, deviceProvider.Description,
                  deviceProvider.Devices.Ids())
        { }

        [DataMember]
        public IEnumerable<Guid> DeviceIds { get; private set; }
    }
}
