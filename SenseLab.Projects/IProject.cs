using CeMaS.Common;
using CeMaS.Common.Collections;
using CeMaS.Data.Storages;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;

namespace SenseLab.Projects
{
    /// <summary>
    /// Project with devices and records.
    /// </summary>
    public interface IProject :
        IItemInfo<Guid>,
        ILocatable<ISpatialLocation>,
        IStorageAware
    {
        /// <summary>
        /// Devices.
        /// </summary>
        /// <value>non-null</value>
        INotifyList<IProjectDevice> Devices { get; }
        /// <summary>
        /// Optional grouping of devices.
        /// </summary>
        /// <value>non-null</value>
        INotifyList<IItemGroup<Guid, IProjectDevice>> DeviceGroups { get; }

        /// <summary>
        /// Stored records of this project.
        /// </summary>
        /// <value>non-null</value>
        IStorageItems<IRecord, uint> Records { get; }
        /// <summary>
        /// Optional grouping of records.
        /// </summary>
        /// <value>non-null</value>
        INotifyList<IItemGroup<Guid, IRecord>> RecordGroups { get; }
    }
}
