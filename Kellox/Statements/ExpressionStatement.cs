using Lox.Expressions;

namespace Lox.Statements;

/// <summary>
/// Models a epression statement
/// </summary>
internal class ExpressionStatement : IStatement
{
    /// <summary>
    /// The inner Expression of the statement
    /// </summary>
    public IExpression Expression { get; init; }

    public ExpressionStatement(IExpression expression)
    {
        this.Expression = expression;
    }

    public void ExecuteStatement()
    {
        Expression.EvaluateExpression();
    }
}
