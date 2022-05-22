using Interpreter.Exceptions;

namespace Interpreter.Expr
{
    internal class UnaryExpression : Expression
    {
        public Token OperatorToken { get; private set; }

        public Expression Right { get; private set; }

        public UnaryExpression(Token operatorToken, Expression right)
        {
            this.OperatorToken = operatorToken;
            this.Right = right;
        }

        public override string ToString() => Parenthesize(this.OperatorToken.Lexeme, this.Right);

        public override object? EvaluateExpression()
        {
            object? result = this.Right.EvaluateExpression();
            if (result is null)
            {
                return null;
            }
            switch (this.OperatorToken.TokenType)
            {
                case TokenType.BANG:
                    return !IsTruthy(Right);
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
