using SenseLab.Common.Locations;
using SenseLab.Common.Records;

namespace SenseLab.Common.Values
{
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
            : base(sequenceNumber, spatialLocation, temporalLocation)
        {
            source.ValidateNonNull("source");
            this.source = source;
            Value = value;
        }

        public override IRecordSource Source
        {
            get { return source; }
        }
        public T Value { get; private set; }
        object IValueRecord.Value
        {
            get { return Value; }
        }

        protected override string GetText()
        {
            return string.Format("{0}", Value);
        }

        private IRecordSource source;
    }
}
