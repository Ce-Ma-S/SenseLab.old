using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Values
{
    [DataContract]
    public class ValueRecord<T> :
        Record,
        IValueRecord
    {
        public ValueRecord(
            Guid sourceId,
            uint sequenceNumber,
            ITime temporalLocation,
            T value,
            ISpatialLocation spatialLocation = null)
            : base(sourceId, sequenceNumber, temporalLocation, spatialLocation)
        {
            Value = value;
        }

        [DataMember]
        public T Value { get; private set; }
        object IValueRecord.Value
        {
            get { return Value; }
        }

        protected override string GetText()
        {
            return string.Format("{0}", Value);
        }
    }
}
