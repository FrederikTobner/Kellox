namespace Interpreter.Expr
{
    internal class GroupingExpression : IExpression
    {
        public IExpression Expression { get; init; }

        public GroupingExpression(IExpression expression)
        {
            this.Expression = expression;
        }

        public override string ToString() => IExpression.Parenthesize("group", Expression);

        public object? EvaluateExpression() => this.Expression.EvaluateExpression();
    }
}
