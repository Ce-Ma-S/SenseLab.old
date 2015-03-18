using CeMaS.Common;
using CeMaS.Common.Collections;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;

namespace SenseLab.Environments
{
    /// <summary>
    /// Device with recordables.
    /// </summary>
    public interface IDevice :
        IConnectable,
        ILocatable<ISpatialLocation>,
        IDeviceProvider,
        IAvailable
    {
        /// <summary>
        /// Recordables of this device.
        /// </summary>
        INotifyEnumerable<IRecordable, Guid> Recordables { get; }
    }
}