namespace SenseLab.Common.Records
{
    public class ValueRecord<T> :
        Record,
        IValueRecord
    {
        public T Value
        {
            get { return value; }
            set
            {
                SetProperty(() => Value, ref this.value, value);
            }
        }

        protected override string GetText()
        {
            return string.Format("{0}", Value);
        }
        object IValueRecord.Value
        {
            get { return Value; }
        }

        private T value;
    }
}
