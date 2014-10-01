using SenseLab.Common.Environments;
using System;

namespace SenseLab.Devices.SensorTag
{
    public class SensorTags :
        DeviceProvider<SensorTag>
    {
        public SensorTags(Guid id)
            : base(id, "SensorTags")
        {
        }
    }
}
