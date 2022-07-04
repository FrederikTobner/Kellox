using System.Text;

namespace Kellox.Expressions;

/// <summary>
/// Interface for a Expression that can be evaluated and converted to a string
/// </summary>
internal interface IExpression
{
    /// <summary>
    /// Returns a representation of the Expression as a string
    /// </summary>
    public abstract string ToString();

    /// <summary>
    /// Evaluates the Expression an returns the result 🧐
    /// </summary>
    public abstract object? Evaluate();

    /// <summary>
    /// Adds paranthesize to an array of Expressions
    /// </summary>
    /// <param name="name">The name of the expression -> kind</param>
    /// <param name="expressions">The inner expressions</param>
    /// <returns></returns>
    protected static string Parenthesize(params IExpression[] expressions)
    {
        StringBuilder builder = new();
        builder.Append('(');
        for (int i = 0; i < expressions.Length; i++)
        {
            IExpression expression = expressions[i];
            builder.Append(expression.ToString());
            if (i != expressions.Length - 1)
            {
                builder.Append(' ');
            }
        }
        builder.Append(')');
        return builder.ToString();
    }

    /// <summary>
    /// Behaviour defining whether something is truthy
    /// </summary>
    public static bool IsTruthy(object? obj) => obj switch
    {
        null => false,
        bool b => b,
        _ => true
    };

    /// <summary>
    /// Behaviour defining wheather 2 objects are equal
    /// </summary>
    protected static bool IsEqual(object? obj1, object? obj2)
    {
        if (obj1 is null && obj2 is null)
        {
            return true;
        }
        if (obj1 is null)
        {
            return false;
        }
        return obj1.Equals(obj2);
    }
}
