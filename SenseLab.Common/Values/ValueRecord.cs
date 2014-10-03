using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System.Runtime.Serialization;

namespace SenseLab.Common.Values
{
    [DataContract]
    public class ValueRecord<T> :
        Record,
        IValueRecord
    {
        public ValueRecord(
            IRecordSource source,
            T value,
            uint sequenceNumber,
            ISpatialLocation spatialLocation = null,
            ITime temporalLocation = null)
            : base(source, sequenceNumber, spatialLocation, temporalLocation)
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
