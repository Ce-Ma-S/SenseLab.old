using System;
using System.Collections.Generic;

namespace SenseLab.Common
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
    }
}
