using CeMaS.Common.Collections;
using SenseLab.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SenseLab.Environments.Service
{
    [DataContract]
    public class DeviceInfo :
        DeviceProviderInfo
    {
        public DeviceInfo(Guid id, string name,
            bool isAvailable, bool isConnected,
            string description = null,
            IEnumerable<Guid> deviceIds = null,
            IEnumerable<Guid> recordableIds = null,
            ISpatialLocation location = null)
            : base(id, name, description, deviceIds)
        {
            if (recordableIds == null)
                recordableIds = Enumerable.Empty<Guid>();
            IsAvailable = isAvailable;
            IsConnected = isConnected;
            RecordableIds = recordableIds;
            Location = location;
        }
        public DeviceInfo(IDevice device)
            : this(device.Id, device.Name,
                  device.IsAvailable, device.IsConnected,
                  device.Description,
                  device.Devices.Ids(),
                  device.Recordables.Ids(),
                  device.Location)
        { }

        [DataMember]
        public bool IsAvailable { get; private set; }
        [DataMember]
        public bool IsConnected { get; private set; }
        [DataMember]
        public IEnumerable<Guid> RecordableIds { get; private set; }
        [DataMember]
        public ISpatialLocation Location { get; private set; }
    }
}
