using System.Linq.Expressions;
using System.Reflection;

namespace VehiGate.Web.Infrastructure;

public static class IQueryableExtensions
{
    public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> query, string propertyName, bool ascending = true)
    {
        var entityType = typeof(T);
        var propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found on type '{entityType}'.");
        }

        var parameter = Expression.Parameter(entityType, "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var orderByMethod = ascending ?
            typeof(Queryable).GetMethods().First(m => m.Name == "OrderBy" && m.GetParameters().Length == 2) :
            typeof(Queryable).GetMethods().First(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);

        var orderByGenericMethod = orderByMethod.MakeGenericMethod(entityType, propertyInfo.PropertyType);
        var resultExpression = Expression.Call(null, orderByGenericMethod, query.Expression, orderByExpression);
        return query.Provider.CreateQuery<T>(resultExpression);
    }
}
