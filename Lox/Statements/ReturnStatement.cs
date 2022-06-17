using Lox.Expressions;
using Lox.Functions.Exceptions;
using Lox.Tokens;

namespace Lox.Statements;

/// <summary>
/// Models a return statement
/// </summary>
internal class ReturnStatement : IStatement
{
    public Token Keyword { get; init; }

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
        throw new Return(value);
    }
}
