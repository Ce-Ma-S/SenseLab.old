using CeMaS.Common.Collections;
using SenseLab.Common;
using SenseLab.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SenseLab.Environments.Service
{
    [DataContract]
    public class EnvironmentInfo :
        LocatableItemInfo<Guid, ISpatialLocation>
    {
        public EnvironmentInfo(Guid id, string name,
            string description = null,
            IEnumerable<Guid> deviceProviderIds = null,
            ISpatialLocation location = null)
            : base(id, name, description, location)
        {
            if (deviceProviderIds == null)
                deviceProviderIds = Enumerable.Empty<Guid>();
            DeviceProviderIds = deviceProviderIds;
        }
        public EnvironmentInfo(IEnvironment environment)
            : this(environment.Id, environment.Name, environment.Description,
                  environment.DeviceProviders.Ids(),
                  environment.Location)
        { }

        [DataMember]
        public IEnumerable<Guid> DeviceProviderIds { get; private set; }
    }
}
