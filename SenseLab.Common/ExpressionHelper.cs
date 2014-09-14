using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SenseLab.Common
{
    public static class ExpressionHelper
    {
        public static PropertyInfo PropertyInfo<T>(this Expression<Func<T>> property)
        {
            return (PropertyInfo)(((MemberExpression)property.Body).Member);
        }
        public static string PropertyName<T>(this Expression<Func<T>> property)
        {
            return property.PropertyInfo().Name;
        }
    }
}
