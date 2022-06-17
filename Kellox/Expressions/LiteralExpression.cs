using Lox.i18n;

namespace Lox.Expressions;

/// <summary>
/// Models a literal expression
/// </summary>
internal class LiteralExpression : IExpression
{
    /// <summary>
    /// The Value of the literal of the Expression
    /// </summary>
    public object? Value { get; init; }

    public LiteralExpression(object? value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Returns a representation of the Expression as a string
    /// </summary>
    public override string ToString()
    {
        if (Value is not null)
        {
            string? result = Value.ToString();
            if (result is not null)
            {
                return result;
            }
        }
        return Constants.NilKeyword;
    }

    public object? EvaluateExpression() => Value;
}
