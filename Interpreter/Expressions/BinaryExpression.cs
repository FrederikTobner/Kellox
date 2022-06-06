using Interpreter.Exceptions;

namespace Interpreter.Expressions
{
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
        /// The operator connecting the two expressions
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
                    TOKENTYPE.GREATER => leftNumber > rightNumber,
                    TOKENTYPE.GREATER_EQUAL => leftNumber >= rightNumber,
                    TOKENTYPE.LESS => leftNumber < rightNumber,
                    TOKENTYPE.LESS_EQUAL => leftNumber <= rightNumber,
                    TOKENTYPE.MINUS => leftNumber - rightNumber,
                    TOKENTYPE.PLUS => leftNumber + rightNumber,
                    TOKENTYPE.SLASH => leftNumber / rightNumber,
                    TOKENTYPE.STAR => leftNumber * rightNumber,
                    TOKENTYPE.BANG_EQUAL => !IExpression.IsEqual(leftNumber, rightNumber),
                    TOKENTYPE.EQUAL_EQUAL => IExpression.IsEqual(leftNumber, rightNumber),
                    _ => throw new RunTimeError(OperatorToken, "Operator not supported"),
                };
            }

            if (leftResult is string leftString && rightResult is string rightString)
            {
                return OperatorToken.TokenType switch
                {
                    TOKENTYPE.PLUS => leftString + rightString,
                    _ => throw new RunTimeError(OperatorToken, "Operator not supported")
                };
            }
            return OperatorToken.TokenType switch
            {
                TOKENTYPE.BANG_EQUAL => !IExpression.IsEqual(leftResult, rightResult),
                TOKENTYPE.EQUAL_EQUAL => IExpression.IsEqual(leftResult, rightResult),
                _ => throw new RunTimeError(OperatorToken, "Operator not supported")
            };
        }
    }
}
