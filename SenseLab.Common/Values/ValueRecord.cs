using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Values
{
    [DataContract]
    public class ValueRecord<T> :
        Record,
        IValueRecord<T>
    {
        public ValueRecord(
            uint id,
            Guid sourceId,
            T value,
            ITimeInterval temporalLocation,
            ISpatialLocation spatialLocation = null)
            : base(id, sourceId, temporalLocation, spatialLocation)
        {
            Value = value;
        }

        public T Value
        {
            get { return value; }
            set { SetProperty(() => Value, ref this.value, value); }
        }
        object IValueRecord.Value
        {
            get { return Value; }
        }

        public override string ToString()
        {
            return string.Format("{0}", Value);
        }

        [DataMember(Name = "Value")]
        private T value;
    }
}
