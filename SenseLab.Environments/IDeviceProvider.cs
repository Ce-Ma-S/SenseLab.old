using CeMaS.Common;
using CeMaS.Common.Collections;
using System;

namespace SenseLab.Environments
{
    /// <summary>
    /// Provides devices.
    /// </summary>
    public interface IDeviceProvider :
        IItemInfo<Guid>
    {
        /// <summary>
        /// Available devices.
        /// </summary>
        INotifyEnumerable<IDevice, Guid> Devices { get; }
    }
}
