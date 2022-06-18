using Kellox.Exceptions;
using Kellox.Expressions;
using Kellox.Tokens;

namespace Kellox.Statements;

/// <summary>
/// Models a return statement
/// </summary>
internal class ReturnStatement : IStatement
{
    /// <summary>
    /// The Keyword of the Return statement -> always return
    /// </summary>
    public Token Keyword { get; init; }

    /// <summary>
    /// The Expression that shall be returned
    /// </summary>
    public IExpression? Expression { get; init; }

    public ReturnStatement(Token keyword, IExpression? expression)
    {
        this.Keyword = keyword;
        this.Expression = expression;
    }

    public void ExecuteStatement()
    {
        object? value = null;
        if (Expression is not null)
        {
            value = Expression.EvaluateExpression();
        }
        // Throws an return exception to get to the beginning of the call stack
        throw new Return(value);
    }
}
