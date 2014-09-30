using SenseLab.Common.Environments;
using System;

namespace SenseLab.Devices.SensorTag
{
    public class SensorTag :
        Device
    {
        public SensorTag(Guid id/*, SensorTags parent*/)
            : base(id, "", null/*, parent*/)
        {
        }

        public override bool IsAvailable
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
