using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VehiGate.Web.Infrastructure
{
    public static class QueryableExtensions
    {
        public static IEnumerable<T> OrderByProperty<T>(this IEnumerable<T> source, string propertyName, bool ascending = true)
        {
            var entityType = typeof(T);
            var propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{entityType}'.");
            }

            Func<T, object> orderByFunc = x => propertyInfo.GetValue(x);

            return ascending ? source.OrderBy(orderByFunc) : source.OrderByDescending(orderByFunc);
        }
    }
}
