using System.Text;

namespace Lox.Expressions;

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
    /// Evaluates the Expression an returns the result
    /// </summary>
    public abstract object? EvaluateExpression();

    protected static string Parenthesize(string name, params IExpression[] expressions)
    {
        StringBuilder builder = new();

        builder.Append('(').Append(name);
        foreach (IExpression expression in expressions)
        {
            builder.Append(' ');
            builder.Append(expression.ToString());
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
            return false;
        }
        if (obj1 is null)
        {
            return false;
        }
        return obj1.Equals(obj2);
    }
}
