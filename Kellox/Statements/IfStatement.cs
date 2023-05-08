using Kellox.Expressions;

namespace Kellox.Statements;

/// <summary>
/// Models a if statement in lox
/// </summary>
internal class IfStatement : IStatement
{

    /// <summary>
    /// The Condition of the if-Statement
    /// </summary>
    public IExpression Condition { get; init; }

    /// <summary>
    /// The Thenbranch of the if-Statement e.g a Block- or Printstatement
    /// </summary>
    public IStatement ThenBranch { get; init; }

    /// @brief The ElseBranch of the if-Statement e.g a Block- or Printstatement
    public IStatement? ElseBranch { get; init; }

    public IfStatement(IExpression condition, IStatement ifStatement, IStatement? elseStatement)
    {
        this.Condition = condition;
        this.ThenBranch = ifStatement;
        this.ElseBranch = elseStatement;
    }

    public void Execute()
    {
        if (IExpression.IsTruthy(Condition.Evaluate()))
        {
            ThenBranch.Execute();
        }
        else if (ElseBranch is not null)
        {
            ElseBranch.Execute();
        }
    }
}
