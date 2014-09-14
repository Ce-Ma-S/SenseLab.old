using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SenseLab.Common.ViewModels
{
    public static class ExpressionHelper
    {
        public static string PropertyName<T>(this Expression<Func<T>> property)
        {
            return ((PropertyInfo)(((MemberExpression)property.Body).Member)).Name;
        }
    }
}
