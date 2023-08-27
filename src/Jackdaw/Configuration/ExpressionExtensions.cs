using System.Linq.Expressions;
using System.Reflection;

namespace Jackdaw.Configuration;

internal static class ExpressionExtensions
{
    public static PropertyInfo GetProperty<T, TResult>(this Expression<Func<T, TResult>> expression)
    {
        if (expression.Body is not MemberExpression member)
        {
            throw new ArgumentException("Must be a property expression");
        }

        if (member.Member is not PropertyInfo property)
        {
            throw new ArgumentException("Expression must be a property");
        }

        return property;
    }
}
