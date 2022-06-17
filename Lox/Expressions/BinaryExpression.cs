using Lox.Interpreter.Exceptions;
using Lox.Messages;
using Lox.Tokens;

namespace Lox.Expressions;

/// <summary>
/// Models a binary expression e.g. a+b, a!=b
/// </summary>
internal class BinaryExpression : IExpression
{


    /// <summary>
    /// The expression left of the operator
    /// </summary>
    public IExpression Left { get; init; }

    /// <summary>
    /// The operator connecting the two expressions called infix operator, because it is fixed in the middle of the two operands
    /// </summary>
    public Token OperatorToken { get; init; }

    /// <summary>
    /// The expression right of the operator
    /// </summary>
    public IExpression Right { get; init; }

    public BinaryExpression(IExpression left, Token operatorToken, IExpression right)
    {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
    }

    /// <summary>
    /// Returns a representation of the Expression as a string
    /// </summary>
    public override string ToString() => IExpression.Parenthesize(OperatorToken.Lexeme, Left, Right);

    public object? EvaluateExpression()
    {
        object? leftResult = Left.EvaluateExpression();
        object? rightResult = Right.EvaluateExpression();
        if (leftResult is double leftNumber && rightResult is double rightNumber)
        {
            return OperatorToken.TokenType switch
            {
                TokenType.GREATER => leftNumber > rightNumber,
                TokenType.GREATER_EQUAL => leftNumber >= rightNumber,
                TokenType.LESS => leftNumber < rightNumber,
                TokenType.LESS_EQUAL => leftNumber <= rightNumber,
                TokenType.MINUS => leftNumber - rightNumber,
                TokenType.PLUS => leftNumber + rightNumber,
                TokenType.SLASH => leftNumber / rightNumber,
                TokenType.STAR => leftNumber * rightNumber,
                TokenType.BANG_EQUAL => !IExpression.IsEqual(leftNumber, rightNumber),
                TokenType.EQUAL_EQUAL => IExpression.IsEqual(leftNumber, rightNumber),
                _ => throw new RunTimeError(OperatorToken, MessageUtils.OperatorNotSupportedErrorMessage),
            };
        }

        if (leftResult is string leftString && rightResult is string rightString)
        {
            return OperatorToken.TokenType switch
            {
                TokenType.PLUS => leftString + rightString,
                _ => throw new RunTimeError(OperatorToken, MessageUtils.OperatorNotSupportedErrorMessage)
            };
        }
        return OperatorToken.TokenType switch
        {
            TokenType.BANG_EQUAL => !IExpression.IsEqual(leftResult, rightResult),
            TokenType.EQUAL_EQUAL => IExpression.IsEqual(leftResult, rightResult),
            _ => throw new RunTimeError(OperatorToken, MessageUtils.OperatorNotSupportedErrorMessage)
        };
    }
}
