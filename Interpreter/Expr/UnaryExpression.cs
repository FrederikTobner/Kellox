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
    }
}
