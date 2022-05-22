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
    }
}
