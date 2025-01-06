using System.Linq.Expressions;

namespace SGuard.Visitor;

/// <summary>
/// An internal class that visits member and unary expressions to check for null values.
/// </summary>
internal sealed class NullVisitor : ExpressionVisitor
{
    /// <summary>
    /// Visits a member expression to check if it is null or has a default value.
    /// </summary>
    /// <param name="node">The member expression to visit.</param>
    /// <returns>A constant expression with a null value if the expression is null or has a default value, otherwise the original expression.</returns>
    protected override Expression VisitMember(MemberExpression node)
    {
        var memberAccessExpression = (MemberExpression)base.VisitMember(node);
        
        if (memberAccessExpression.Expression == null)
        {
            return base.VisitMember(node);
        }

        Expression nullOrDefaultCheckExpression = Expression.Equal(
            Expression.Default(typeof(object)), Expression.Convert(memberAccessExpression.Expression, typeof(object)));
        
       return Expression.Condition(
            nullOrDefaultCheckExpression,
            Expression.Constant(null),
            Expression.Constant(default));
    }
    /// <summary>
    /// Visits a unary expression and converts it to an object type.
    /// </summary>
    /// <param name="node">The unary expression to visit.</param>
    /// <returns>The original expression, converted to an object type.</returns>
    protected override Expression VisitUnary(UnaryExpression node)
    {
        return Expression.Convert(node.Operand, typeof(object));
    }
}