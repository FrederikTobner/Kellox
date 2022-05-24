using Interpreter.Exceptions;

namespace Interpreter.Expressions
{
    internal class UnaryExpression : IExpression
    {
        public Token OperatorToken { get; private set; }

        public IExpression Right { get; private set; }

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
                    throw new RunTimeError(this.OperatorToken, "Operand must be a number.");
                default:
                    throw new Exception("");
            }
        }
    }
}
