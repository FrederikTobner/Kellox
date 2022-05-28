namespace Interpreter.Statements
{
    /// <summary>
    /// Models a if statement in lox
    /// </summary>
    internal class IfStatement : IStatement
    {

        public IExpression Condition { get; init; }
        public IStatement ThenBranch { get; init; }
        public IStatement? ElseBranch { get; init; }

        public IfStatement(IExpression condition, IStatement ifStatement, IStatement? elseStatement)
        {
            this.Condition = condition;
            this.ThenBranch = ifStatement;
            this.ElseBranch = elseStatement;
        }

        public void ExecuteStatement()
        {
            if (IExpression.IsTruthy(Condition.EvaluateExpression()))
            {
                ThenBranch.ExecuteStatement();
            }
            else if (ElseBranch is not null)
            {
                ElseBranch.ExecuteStatement();
            }
        }
    }
}
