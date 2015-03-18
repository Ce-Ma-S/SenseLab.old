using System;

namespace CeMaS.Common.Events
{
    public class ValueChangeEventArgs<T> :
        EventArgs
    {
        public ValueChangeEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
        }
        public ValueChangeEventArgs(T value, bool isNew)
        {
            if (isNew)
                NewValue = value;
            else
                OldValue = value;
        }

        public OptionalValue<T> OldValue { get; private set; }
        public OptionalValue<T> NewValue { get; private set; }
    }
}
