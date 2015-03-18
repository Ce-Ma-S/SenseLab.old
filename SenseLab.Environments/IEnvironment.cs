using CeMaS.Common;
using CeMaS.Common.Collections;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;

namespace SenseLab.Environments
{
    /// <summary>
    /// Environment with devices.
    /// </summary>
    public interface IEnvironment :
        IItemInfo<Guid>,
        ILocatable<ISpatialLocation>,
        IItemLookup<IDeviceProvider, Guid>,
        IItemLookup<IDevice, Guid>,
        IItemLookup<IRecordable, Guid>
    {
        /// <summary>
        /// Available device providers.
        /// </summary>
        INotifyEnumerable<IDeviceProvider, Guid> DeviceProviders { get; }
        /// <summary>
        /// Optional grouping of devices.
        /// </summary>
        INotifyList<IItemGroup<Guid, IDevice>, Guid> DeviceGroups { get; }
    }
}
