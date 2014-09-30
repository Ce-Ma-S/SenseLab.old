using SenseLab.Common.Environments;
using System;

namespace SenseLab.Devices.SensorTag
{
    public class SensorTags :
        DeviceProvider<SensorTag>
    {
        public SensorTags(Guid id, string name, string description = null)
            : base(id, name, description, /*null,*/ null, null)
        {
        }
    }
}
