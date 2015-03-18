using System;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Validation
{
    public static class ValidationHelper
    {
        public static void ValidateNonDefault<T>(this T argument, string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(argument, default(T)))
            {
                if (name != null)
                    throw new ArgumentNullException(name);
                else
                    throw new ArgumentNullException();
            }
        }
        public static void ValidateNonNull(this object argument, string name = null)
        {
            if (argument == null)
            {
                if (name != null)
                    throw new ArgumentNullException(name);
                else
                    throw new ArgumentNullException();
            }
        }
        public static void ValidateNonNullOrEmpty(this string argument, string name = null)
        {
            if (string.IsNullOrEmpty(argument))
            {
                if (name != null)
                    throw new ArgumentNullException(name);
                else
                    throw new ArgumentNullException();
            }
        }
        public static void ValidateNonNullOrEmpty<T>(this IEnumerable<T> argument, string name = null)
        {
            if (argument == null || !argument.Any())
            {
                if (name != null)
                    throw new ArgumentNullException(name);
                else
                    throw new ArgumentNullException();
            }
        }
        public static void Validate<T>(this bool condition)
                where T : Exception, new()
        {
            if (!condition)
            {
                throw new T();
            }
        }
        public static void Validate(this bool condition, string name = null)
        {
            if (!condition)
            {
                if (name != null)
                    throw new ArgumentException(name);
                else
                    throw new ArgumentException();
            }
        }
        public static void ValidateContainmentOf<T>(this ICollection<T> items, T item, string name = null)
        {
            if (!items.Contains(item))
            {
                if (name == null)
                    throw new ArgumentOutOfRangeException();
                else
                    throw new ArgumentOutOfRangeException(name);
            }
        }
        public static void ValidateGreaterThan<T>(this T value, T baseValue, string name = null)
            where T : IComparable<T>
        {
            int comparison = value.CompareTo(baseValue);
            bool valid = comparison > 0;
            if (!valid)
            {
                if (name == null)
                    throw new ArgumentOutOfRangeException();
                else
                    throw new ArgumentOutOfRangeException(name);
            }
        }
        public static void ValidateGreaterThanOrEqual<T>(this T value, T baseValue, string name = null)
            where T : IComparable<T>
        {
            int comparison = value.CompareTo(baseValue);
            bool valid = comparison >= 0;
            if (!valid)
            {
                if (name == null)
                    throw new ArgumentOutOfRangeException();
                else
                    throw new ArgumentOutOfRangeException(name);
            }
        }
        public static void ValidateSmallerThan<T>(this T value, T baseValue, string name = null)
            where T : IComparable<T>
        {
            int comparison = value.CompareTo(baseValue);
            bool valid = comparison < 0;
            if (!valid)
            {
                if (name == null)
                    throw new ArgumentOutOfRangeException();
                else
                    throw new ArgumentOutOfRangeException(name);
            }
        }
        public static void ValidateSmallerThanOrEqual<T>(this T value, T baseValue, string name = null)
            where T : IComparable<T>
        {
            int comparison = value.CompareTo(baseValue);
            bool valid = comparison <= 0;
            if (!valid)
            {
                if (name == null)
                    throw new ArgumentOutOfRangeException();
                else
                    throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
