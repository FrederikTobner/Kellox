namespace Interpreter.Statements
{
    internal class IfStatement : IStatement
    {

        private readonly IExpression condition;
        private readonly IStatement thenBranch;
        private readonly IStatement? elseBranch;

        public IfStatement(IExpression condition, IStatement ifStatement, IStatement? elseStatement)
        {
            this.condition = condition;
            this.thenBranch = ifStatement;
            this.elseBranch = elseStatement;
        }

        public void ExecuteStatements()
        {
            if (IExpression.IsTruthy(condition.EvaluateExpression()))
            {
                thenBranch.ExecuteStatements();
            }
            else if (elseBranch is not null)
            {
                elseBranch.ExecuteStatements();
            }
        }
    }
}
