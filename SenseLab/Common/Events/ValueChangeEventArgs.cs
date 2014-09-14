using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.Events
{
    public class ValueChangeEventArgs<T> : EventArgs
    {
        public ValueChangeEventArgs(T oldValue, T newValue, DateTimeOffset? timestamp = null)
            : this(newValue, timestamp)
        {
            OldValue = oldValue;
        }
        public ValueChangeEventArgs(T newValue, DateTimeOffset? timestamp = null)
        {
            NewValue = newValue;
            Timestamp = timestamp;
        }

        public OptionalValue<T> OldValue { get; private set; }
        public T NewValue { get; private set; }
        public DateTimeOffset? Timestamp { get; private set; }
    }
}
