using Interpreter.Exceptions;

namespace Interpreter.Expr
{
    internal class BinaryExpression : Expression
    {
        public Expression Left { get; init; }

        public Token OperatorToken { get; init; }

        public Expression Right { get; init; }

        public BinaryExpression(Expression left, Token operatorToken, Expression right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override string ToString() => Parenthesize(OperatorToken.Lexeme, Left, Right);

        public override object? EvaluateExpression()
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
                    TokenType.BANG_EQUAL => !IsEqual(leftNumber, rightNumber),
                    TokenType.EQUAL_EQUAL => IsEqual(leftNumber, rightNumber),
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
                TokenType.BANG_EQUAL => !IsEqual(leftResult, rightResult),
                TokenType.EQUAL_EQUAL => IsEqual(leftResult, rightResult),
                _ => throw new RunTimeError(OperatorToken, "Operator not supported")
            };
        }
    }
}
