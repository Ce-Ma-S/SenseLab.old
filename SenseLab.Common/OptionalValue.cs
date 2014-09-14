using System;
using System.Collections.Generic;

namespace SenseLab.Common
{
    public struct OptionalValue<T>
    {
        public OptionalValue(T value)
            : this()
        {
            HasValue = true;
            Value = value;
        }

        public static readonly OptionalValue<T> None = new OptionalValue<T>();

        public bool HasValue { get; private set; }
        public bool HasNonDefaultValue
        {
            get { return HasValue && !EqualityComparer<T>.Default.Equals(Value, default(T)); }
        }
        public T Value { get; private set; }

        public static bool operator ==(OptionalValue<T> value1, OptionalValue<T> value2)
        {
            return
                value1.HasValue == value2.HasValue &&
                EqualityComparer<T>.Default.Equals(value1.Value, value2.Value);
        }
        public static bool operator !=(OptionalValue<T> value1, OptionalValue<T> value2)
        {
            return !(value1 == value2);
        }
        public static bool operator ==(OptionalValue<T> value1, T value2)
        {
            return
                value1.HasValue &&
                EqualityComparer<T>.Default.Equals(value1.Value, value2);
        }
        public static bool operator !=(OptionalValue<T> value1, T value2)
        {
            return !(value1 == value2);
        }
        public static bool operator ==(T value1, OptionalValue<T> value2)
        {
            return
                value2.HasValue &&
                EqualityComparer<T>.Default.Equals(value1, value2.Value);
        }
        public static bool operator !=(T value1, OptionalValue<T> value2)
        {
            return !(value1 == value2);
        }
        public static implicit operator OptionalValue<T>(T value)
        {
            return new OptionalValue<T>(value);
        }
        public static explicit operator T(OptionalValue<T> value)
        {
            if (!value.HasValue)
                throw new InvalidCastException("No value exists.");
            return value.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is OptionalValue<T>)
            {
                return this == (OptionalValue<T>)obj;
            }
            else if (obj is T)
            {
                return this == (T)obj;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HasValue ? int.MinValue : Value.GetHashCode();
        }
    }
}
