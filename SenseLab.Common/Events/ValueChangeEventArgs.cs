using SenseLab.Common.Locations;

namespace SenseLab.Common.Events
{
    public class ValueChangeEventArgs<T> :
        LocatableEventArgs
    {
        public ValueChangeEventArgs(T oldValue, T newValue, ILocation location = null)
            : this(newValue, location)
        {
            OldValue = oldValue;
        }
        public ValueChangeEventArgs(T newValue, ILocation location = null)
            : base(location)
        {
            NewValue = newValue;
        }

        public OptionalValue<T> OldValue { get; private set; }
        public T NewValue { get; private set; }
    }
}
