namespace Interpreter.Statements
{
    /// <summary>
    /// Models a epression statement
    /// </summary>
    internal class ExpressionStatement : IStatement
    {
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
}
