using Interpreter.Exceptions;

namespace Interpreter.Expressions
{
    internal class UnaryExpression : IExpression
    {
        /// <summary>
        /// The operator of the unary Expression
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
                case TOKENTYPE.BANG:
                    return !IExpression.IsTruthy(Right);
                case TOKENTYPE.MINUS:
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
}
