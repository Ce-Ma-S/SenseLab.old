using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace CeMaS.Common
{
    public static class ExpressionHelper
    {
        public static PropertyInfo PropertyInfo<T>(this Expression<Func<T>> property)
        {
            Contract.Requires(property != null);
            Contract.Requires(property is MemberExpression);

            return (PropertyInfo)(((MemberExpression)property.Body).Member);
        }
        public static string PropertyName<T>(this Expression<Func<T>> property)
        {
            return property.PropertyInfo().Name;
        }
    }
}
