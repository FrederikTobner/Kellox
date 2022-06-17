using Kellox.Tokens;

namespace Kellox.Expressions;

/// <summary>
/// Models a logical expression (a and b/a or b)
/// </summary>
internal class LogicalExpression : IExpression
{
    /// <summary>
    /// The left Subexpression of the Logical Expression
    /// </summary>
    public IExpression Left { get; init; }

    /// <summary>
    /// The right Subexpression of the Logical Expression
    /// </summary>
    public IExpression Right { get; init; }

    /// <summary>
    /// The operator Token of the Logical Expression (and/or)
    /// </summary>
    private readonly Token operatorToken;

    public LogicalExpression(IExpression left, IExpression right, Token operatorToken)
    {
        this.Left = left;
        this.Right = right;
        this.operatorToken = operatorToken;
    }

    public object? EvaluateExpression()
    {
        object? leftResult = Left.EvaluateExpression();
        if (operatorToken.TokenType is TokenType.OR)
        {
            if (IExpression.IsTruthy(leftResult))
            {
                return leftResult;
            }
        }
        else
        {
            if (!IExpression.IsTruthy(leftResult))
            {
                return leftResult;
            }
        }
        return Right.EvaluateExpression();
    }

    /// <summary>
    /// Returns a representation of the Expression as a string
    /// </summary>
    public override string ToString() => IExpression.Parenthesize(operatorToken.Lexeme, Right, Left);
}
