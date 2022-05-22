namespace Interpreter.Expr
{
    internal class GroupingExpression : Expression
    {
        public Expression Expression { get; init; }

        public GroupingExpression(Expression expression)
        {
            this.Expression = expression;
        }

        public override string ToString() => Parenthesize("group", Expression);
    }
}
