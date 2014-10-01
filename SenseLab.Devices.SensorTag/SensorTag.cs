using SenseLab.Common.Environments;
using System;

namespace SenseLab.Devices.SensorTag
{
    public class SensorTag :
        Device
    {
        public SensorTag(Guid id)
            : base(id, "SensorTag")
        {
        }

        public override bool IsAvailable
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
