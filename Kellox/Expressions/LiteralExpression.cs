using Kellox.Keywords;
using Kellox.Tokens;
using Kellox.Utils;

namespace Kellox.Expressions;

/// <summary>
/// Models a literal expression
/// </summary>
internal class LiteralExpression : IExpression
{
    /// <summary>
    /// The Value of the literal of the Expression
    /// </summary>
    public object? Value { get; init; }

    /// <summary>
    /// The Token of the literal Expression -> null if it is not a string literal and only used for escape sequence specific error logging
    /// </summary>
    public Token? LiteralToken { get; init; }

    public LiteralExpression(object? value, Token? literalToken = null)
    {
        if (value is string text && literalToken is not null)
        {
            if (text.Contains('\\'))
            {
                text = EscapeSequenceFabricator.EnrichString(text, literalToken);
            }
            value = text;
        }
        this.Value = value;
    }

    /// <summary>
    /// Returns a representation of the Expression as a string
    /// </summary>
    public override string ToString()
    {
        if (Value is null)
        {
            return KeywordConstants.NilKeyword;
        }
        if (Value is bool logicalValue)
        {
            return logicalValue ? KeywordConstants.TrueKeyword : KeywordConstants.FalseKeyword;
        }
        string? result = Value.ToString();
        if (result is not null)
        {
            return result;
        }
        return KeywordConstants.NilKeyword;
    }

    public object? Evaluate() => Value;
}
