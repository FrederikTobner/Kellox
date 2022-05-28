namespace Interpreter.Statements
{
    /// <summary>
    /// Models a While Statement in lox
    /// </summary>
    internal class WhileStatement : IStatement
    {
        public IExpression Expression { get; init; }

        public IStatement Body { get; init; }

        public WhileStatement(IExpression expression, IStatement body)
        {
            this.Expression = expression;
            this.Body = body;
        }

        public void ExecuteStatement()
        {
            while (IExpression.IsTruthy(Expression.EvaluateExpression()))
            {
                Body.ExecuteStatement();
            }
        }
    }
}
