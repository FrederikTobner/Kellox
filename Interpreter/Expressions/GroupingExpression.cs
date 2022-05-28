namespace Interpreter.Expressions
{
    /// <summary>
    /// Models a grouping expression
    /// </summary>
    internal class GroupingExpression : IExpression
    {
        public IExpression Expression { get; init; }

        public GroupingExpression(IExpression expression)
        {
            this.Expression = expression;
        }

        /// <summary>
        /// Returns a representation of the Expression as a string
        /// </summary>
        public override string ToString() => IExpression.Parenthesize("group", Expression);

        public object? EvaluateExpression() => this.Expression.EvaluateExpression();
    }
}
