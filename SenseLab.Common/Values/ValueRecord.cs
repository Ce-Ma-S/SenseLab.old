using SenseLab.Common.Locations;
using SenseLab.Common.Records;

namespace SenseLab.Common.Values
{
    public class ValueRecord<T> :
        Record,
        IValueRecord
    {
        public ValueRecord(
            T value,
            IRecordable recordable = null,
            ISpatialLocation spatialLocation = null,
            ITime temporalLocation = null)
            : base(recordable, spatialLocation, temporalLocation)
        {
            Value = value;
        }

        public T Value
        {
            get { return value; }
            set
            {
                SetProperty(() => Value, ref this.value, value);
            }
        }
        object IValueRecord.Value
        {
            get { return Value; }
        }

        protected override string GetText()
        {
            return string.Format("{0}", Value);
        }

        private T value;
    }
}
