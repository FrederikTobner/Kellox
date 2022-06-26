namespace Kellox.Expressions;

/// <summary>
/// Models a grouping expression
/// </summary>
internal class GroupingExpression : IExpression
{
    /// <summary>
    /// The inner Expression e.g. for (a==b) -> a==b
    /// </summary>
    public IExpression Expression { get; init; }

    public GroupingExpression(IExpression expression)
    {
        this.Expression = expression;
    }

    /// <summary>
    /// Returns a representation of the Expression as a string
    /// </summary>
    public override string ToString() => IExpression.Parenthesize(Expression);

    public object? Evaluate() => this.Expression.Evaluate();
}
