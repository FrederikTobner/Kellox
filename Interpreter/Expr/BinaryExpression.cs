using Interpreter.Exceptions;

namespace Interpreter.Expr
{
    internal class BinaryExpression : IExpression
    {
        public IExpression Left { get; init; }

        public Token OperatorToken { get; init; }

        public IExpression Right { get; init; }

        public BinaryExpression(IExpression left, Token operatorToken, IExpression right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

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
                    _ => throw new RunTimeError(OperatorToken, "Operator not supported"),
                };
            }

            if (leftResult is string leftString && rightResult is string rightString)
            {
                return OperatorToken.TokenType switch
                {
                    TokenType.PLUS => leftString + rightString,
                    _ => throw new RunTimeError(OperatorToken, "Operator not supported")
                };
            }
            return OperatorToken.TokenType switch
            {
                TokenType.BANG_EQUAL => !IExpression.IsEqual(leftResult, rightResult),
                TokenType.EQUAL_EQUAL => IExpression.IsEqual(leftResult, rightResult),
                _ => throw new RunTimeError(OperatorToken, "Operator not supported")
            };
        }
    }
}
