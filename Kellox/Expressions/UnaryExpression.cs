using Kellox.Exceptions;
using Kellox.Tokens;

namespace Kellox.Expressions;

/// <summary>
/// Models a unary expression e.g. -5.0 / !true. There is no postfix operator in lox so the operator will always come before the operand
/// </summary>
internal class UnaryExpression : IExpression
{
    /// <summary>
    /// The operator of the unary Expression -> prefix operator, bexause it commed before the operator
    /// </summary>
    public Token OperatorToken { get; init; }

    /// <summary>
    /// The expression right of the operator (e.g. a variable identifier / a literal ("a" / true / 5.0))
    /// </summary>
    public IExpression Right { get; init; }

    public UnaryExpression(Token operatorToken, IExpression right)
    {
        this.OperatorToken = operatorToken;
        this.Right = right;
    }

    /// <summary>
    /// Returns a representation of the Expression as a string
    /// </summary>
    public override string ToString() => IExpression.Parenthesize(this.OperatorToken.Lexeme, this.Right);

    public object? EvaluateExpression()
    {
        object? result = this.Right.EvaluateExpression();
        if (result is null)
        {
            return null;
        }
        switch (this.OperatorToken.TokenType)
        {
            case TokenType.BANG:
                return !IExpression.IsTruthy(Right);
            case TokenType.MINUS:
                if (result is double resultNumber)
                {
                    return -resultNumber;
                }
                throw new RunTimeError(this.OperatorToken, "Operand can only be used on a number.");
            default:
                throw new RunTimeError(this.OperatorToken, "Operand can not be used for a unary Expression.");
        }
    }
}
